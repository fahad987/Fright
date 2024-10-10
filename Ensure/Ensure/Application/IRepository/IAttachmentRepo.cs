using Ensure.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface IAttachmentRepo
{
    Task<Attachment> AddAttachmentAsync(string path, string name);
}