using Ensure.Entities.Domain;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface ICountryRepo
{
    Task<List<Country>> GetAllCountryAsync(CountryFilter model);
    Task<Country> AddCountryAsync(Country model);
    Task<Country> UpdateCountryAsync(Country model);
    Task UpdateCountryIsActiveStatusAsync(Guid countryId, bool isActive);
}