using Ensure.Entities.Domain;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface IUserRepo
{
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> AddUserAsync(User model);
    Task<User> UpdateUserAsync(User model);
    Task<List<User>> GetAllUserAsync(UserFilter model);
    Task<bool> RemoveUserAsync(Guid id);
    Task<User> GetUserByEmailAsync(string email);
    Task UpdateUserIsActiveStatusAsync(Guid userId, bool isActive);
}