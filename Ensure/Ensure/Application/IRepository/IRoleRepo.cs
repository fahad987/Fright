using Ensure.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface IRoleRepo
{
    Task<Role> AddRoleAsync(Role model);
    Task<Role> UpdateRoleAsync(Role model);
    Task<List<Role>> GetAllRoleAsync(RoleFilter model);
    Task<bool> RemoveRoleAsync(Guid id);
    Task UpdateRoleIsActiveStatusAsync(Guid roleId, bool isActive);


}