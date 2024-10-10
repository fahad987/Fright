using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class SurchargeService : ISurchargeService
{
    private readonly ISurchargeRepo _surchargeRepo;

    public SurchargeService(ISurchargeRepo surchargeRepo)
    {
        _surchargeRepo = surchargeRepo;
    }

    public async Task<Surcharge> AddSurchargeAsync(Surcharge model)
        => await _surchargeRepo.AddSurchargeAsync(model);
    
}