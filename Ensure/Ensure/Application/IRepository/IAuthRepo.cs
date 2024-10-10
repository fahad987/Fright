namespace Ensure.Application.IRepository;

public interface IAuthRepo
{
    Task AddAuthAsync(Guid id, string password);
    Task<bool> CheckPasswordAsync(Guid id,string password);
}