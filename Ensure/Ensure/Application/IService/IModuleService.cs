using Ensure.Entities.Domain;

namespace Ensure.Application.IService;

public interface IModuleService
{
    Task<List<ModulePermission>> GetAllModulePermissionAsync();
}