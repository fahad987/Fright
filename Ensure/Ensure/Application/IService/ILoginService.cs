using Ensure.Entities.Domain;

namespace Ensure.Application.IService;

public interface ILoginService
{
    Task<object> LoginAsync(Login model);
}