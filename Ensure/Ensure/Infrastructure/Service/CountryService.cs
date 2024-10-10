using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class CountryService : ICountryService
{
    private readonly ICountryRepo _countryRepo;

    public CountryService(ICountryRepo countryRepo)
    {
        _countryRepo = countryRepo;
    }

    public async Task<List<Country>> GetAllCountryAsync(CountryFilter model)
        =>await _countryRepo.GetAllCountryAsync(model);

    public async Task<Country> AddCountryAsync(Country model)
        =>await _countryRepo.AddCountryAsync(model);

    public async Task<Country> UpdateCountryAsync(Country model)
        =>await _countryRepo.UpdateCountryAsync(model);

    public async Task UpdateCountryIsActiveStatusAsync(Guid countryId, bool isActive)
        =>await _countryRepo.UpdateCountryIsActiveStatusAsync(countryId,isActive);
}