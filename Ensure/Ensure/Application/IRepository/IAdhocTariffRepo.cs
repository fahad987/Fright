using Ensure.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface IAdhocTariffRepo
{
    Task<AdhocTariff> AddAdhocTariffAsync(AdhocTariff model);
    Task<AdhocTariff> UpdateAdhocTariffAsync(AdhocTariff model);
    Task<List<AdhocTariff>> GetAllAdhocTariffAsync(AdhocTariffFilter model);
    Task<AdhocTariff> GetAdhocTariffByIdAsync(Guid adhocId);
    Task UpdateAdhocTariffIsActiveStatusAsync(Guid adhocId, bool isActive);
}