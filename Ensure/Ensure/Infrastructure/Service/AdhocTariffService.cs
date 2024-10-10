using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class AdhocTariffService : IAdhocTariffService
{
    private readonly IAdhocTariffRepo _adhocTariffRepo;

    public AdhocTariffService(IAdhocTariffRepo adhocTariffRepo)
    {
        _adhocTariffRepo = adhocTariffRepo;
    }

    public async Task<AdhocTariff> AddAdhocTariffAsync(AdhocTariff model)
        => await _adhocTariffRepo.AddAdhocTariffAsync(model);

    public async Task<AdhocTariff> UpdateAdhocTariffAsync(AdhocTariff model)
        => await _adhocTariffRepo.UpdateAdhocTariffAsync(model);

    public async Task<List<AdhocTariff>> GetAllAdhocTariffAsync(AdhocTariffFilter model)
        => await _adhocTariffRepo.GetAllAdhocTariffAsync(model);

    public async Task<AdhocTariff> GetAdhocTariffByIdAsync(Guid adhocId)
        => await _adhocTariffRepo.GetAdhocTariffByIdAsync(adhocId);

    public async Task UpdateAdhocTariffIsActiveStatusAsync(Guid adhocId, bool isActive)
        => await _adhocTariffRepo.UpdateAdhocTariffIsActiveStatusAsync(adhocId, isActive);
}