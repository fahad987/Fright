using Ensure.Application.IHelper;
using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class LoginService : ILoginService
{
    private readonly IUserRepo _userRepo;
    private readonly IAuthRepo _authRepo;
    private readonly IJwtRepo _jwtRepo;
    private readonly IJwtManagerHelper _jwtManagerHelper;

    public LoginService(IUserRepo userRepo, IAuthRepo authRepo, IJwtRepo jwtRepo, IJwtManagerHelper jwtManagerHelper)
    {
        _userRepo = userRepo;
        _authRepo = authRepo;
        _jwtRepo = jwtRepo;
        _jwtManagerHelper = jwtManagerHelper;
    }

    public async Task<object> LoginAsync(Login model)
    {
        var user = await _userRepo.GetUserByEmailAsync(model.email);
        if (user == null)
            throw new Exception("Invalid user");
        if (!await _authRepo.CheckPasswordAsync(user.id,model.password))
            throw new Exception("Invalid password");
        var tokenId = Guid.NewGuid();
        var refreshToken = _jwtManagerHelper.GenerateRefreshToken();
        var accessToken = _jwtManagerHelper.GenerateJwtToken(
            user.id,
            user.roles.Select(x=>x.roleId).ToList(),
            tokenId);

        var refreshTokenInp = new RefreshToken
        {
            id = Guid.NewGuid(),
            userId = user.id,
            token = refreshToken,
            jwtId = tokenId,
            expiryDate = DateTime.UtcNow.AddMonths(6),
            createDate = DateTime.UtcNow
        };
        await _jwtRepo.AddJwtRefreshTokenAsync(refreshTokenInp);
        return new 
        {
            user.id,
            user.firstName,
            user.lastName,
            accessToken,
            refreshToken
        };
    }
}