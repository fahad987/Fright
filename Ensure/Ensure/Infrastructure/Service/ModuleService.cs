using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class ModuleService : IModuleService
{
    private readonly IModuleRepo _moduleRepo;

    public ModuleService(IModuleRepo moduleRepo)
    {
        _moduleRepo = moduleRepo;
    }

    public async Task<List<ModulePermission>> GetAllModulePermissionAsync()
        => await _moduleRepo.GetAllModulePermissionAsync();
}