using Ensure.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface IModuleRepo
{
    Task<List<ModulePermission>> GetAllModulePermissionAsync();
}