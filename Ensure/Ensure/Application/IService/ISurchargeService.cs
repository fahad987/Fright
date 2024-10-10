using Ensure.Entities.Domain;

namespace Ensure.Application.IService;

public interface ISurchargeService
{
    Task<Surcharge> AddSurchargeAsync(Surcharge model);
}