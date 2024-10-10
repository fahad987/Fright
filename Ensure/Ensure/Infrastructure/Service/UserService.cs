using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class UserService :IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly IAuthRepo _authRepo;

    public UserService(IUserRepo userRepo, IAuthRepo authRepo)
    {
        _userRepo = userRepo;
        _authRepo = authRepo;
    }


    public async Task<User> GetUserByIdAsync(Guid userId)
        => await _userRepo.GetUserByIdAsync(userId);

    public async Task<User> AddUserAsync(NewUser model)
    {
        var result = await _userRepo.AddUserAsync(model.user);
        await _authRepo.AddAuthAsync(result.id, model.password);
        return result;
    }

    public async Task<User> UpdateUserAsync(User model)
        => await _userRepo.UpdateUserAsync(model);

    public async Task<List<User>> GetAllUserAsync(UserFilter model)
        => await _userRepo.GetAllUserAsync(model);

    public async Task UpdateUserIsActiveStatusAsync(Guid userId, bool isActive)
        => await _userRepo.UpdateUserIsActiveStatusAsync(userId, isActive);
}