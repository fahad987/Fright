using Ensure.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface ISurchargeRepo
{
    Task<(bool, string)> ValidateSurchargeModel(List<Surcharge> model);
    Task<Surcharge> AddSurchargeAsync(Surcharge model);
    Task<List<Surcharge>> UpdateSurchargeAsync(List<Surcharge> model,Guid airlineId);
    Task<List<Surcharge>> AddSurchargeAsync(List<Surcharge> model, Guid airlineId);
    Task<List<Surcharge>> GetAllSurchargeAsync(List<Guid> airlineIds);
    Task<List<Surcharge>> GetAllSurchargeAsync(Guid airlineId);
}