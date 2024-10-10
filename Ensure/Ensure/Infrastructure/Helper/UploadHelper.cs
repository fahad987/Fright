using Ensure.Application.IHelper;
using Ensure.Application.IRepository;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Microsoft.Extensions.Options;

namespace Ensure.Infrastructure.Helper;

public class UploadHelper : IUploadHelper
{
    #region Fields & constructor
    private readonly Settings _settings;
    private readonly IAttachmentRepo _attachmentRepo;
    public UploadHelper(IOptions<Settings> settings, IAttachmentRepo attachmentRepo)
    {
        _attachmentRepo = attachmentRepo;
        _settings = settings.Value;
    }
    #endregion
    
    private async Task SaveFileAsync(IFormFile? file, string directoryPath, string fileName)
    {
        if (directoryPath is null) throw new ArgumentNullException();
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        await using var stream = new FileStream(Path.Combine(directoryPath, fileName), FileMode.Create);
        await file.CopyToAsync(stream);
    }

    public async Task<Guid> GetUploadIdAsync(IFormFile? file, Guid id)
    {
        if (id != Guid.Empty)
            return id;
        else if (id == Guid.Empty && file == null)
            return Guid.Empty;
        else
           return (await UploadFileAsync(file)).id;
    }

    public async Task<List<Attachment>> UploadFileAsync(List<IFormFile?> files)
    {
        var list = new List<Attachment>();
        var root = _settings.resourceDirectory;
        var directory = Path.Combine("Resources",DateTime.UtcNow.Year.ToString(),DateTime.UtcNow.Month.ToString());
        var physicalDirectory = Path.Combine(root, directory);
        foreach (var row in files)
        {
            var extension = Path.GetExtension(row.FileName);
            var id = Guid.NewGuid();
            var fileName = id + extension;
            await SaveFileAsync(row, physicalDirectory, fileName);
            list.Add(await _attachmentRepo
                .AddAttachmentAsync(Path.Combine(directory, fileName), row.FileName));
        }

        return list;
    }
    public async Task<Attachment> UploadFileAsync(IFormFile? file)
        => (await UploadFileAsync(new List<IFormFile?>() {file})).FirstOrDefault();
    
}