namespace Ensure.Application.IHelper;

public interface IJwtManagerHelper
{
    string GenerateJwtToken(Guid userId, List<Guid> roles,Guid jti);
    string GenerateRefreshToken();
}