

using Ensure.Entities.Domain;

namespace Ensure.Application.IHelper;

public interface IUploadHelper
{
    Task<Guid> GetUploadIdAsync(IFormFile? file,Guid id);
    Task<Attachment> UploadFileAsync(IFormFile? file);
    Task<List<Entities.Domain.Attachment>> UploadFileAsync(List<IFormFile?> files);
}