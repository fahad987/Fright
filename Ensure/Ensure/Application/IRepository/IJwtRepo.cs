using Ensure.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface IJwtRepo
{
    Task AddJwtRefreshTokenAsync(RefreshToken model);
}