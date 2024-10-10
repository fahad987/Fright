using Ensure.Entities.Domain;

namespace Ensure.Application.IService;

public interface ICityService
{
    Task<List<City>> GetAllCityAsync(CityFilter model);
    Task<City> AddCityAsync(City model);
    Task<City> UpdateCityAsync(City model);
    Task UpdateCityIsActiveStatusAsync(Guid cityId, bool isActive);
}