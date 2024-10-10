using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class RoleService : IRoleService
{
    private readonly IRoleRepo _roleRepo;

    public RoleService(IRoleRepo roleRepo)
    {
        _roleRepo = roleRepo;
    }

    public async Task<Role> AddRoleAsync(Role model)
        => await _roleRepo.AddRoleAsync(model);

    public async Task<Role> UpdateRoleAsync(Role model)
        => await _roleRepo.UpdateRoleAsync(model);

    public async Task<List<Role>> GetAllRoleAsync(RoleFilter model)
        => await _roleRepo.GetAllRoleAsync(model);

    public async Task<bool> RemoveRoleAsync(Guid id)
        => await _roleRepo.RemoveRoleAsync(id);

    public async Task UpdateRoleIsActiveStatusAsync(Guid roleId, bool isActive)
        => await _roleRepo.UpdateRoleIsActiveStatusAsync(roleId,isActive);
}