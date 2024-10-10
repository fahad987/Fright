using Ensure.Entities.Domain;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Application.IService;

public interface IUserService
{
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> AddUserAsync(NewUser model);
    Task<User> UpdateUserAsync(User model);
    Task<List<User>> GetAllUserAsync(UserFilter model);
    Task UpdateUserIsActiveStatusAsync(Guid userId, bool isActive);

}