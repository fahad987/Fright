using Ensure.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface ICityRepo
{
    Task<List<City>> GetAllCityAsync(CityFilter model);
    Task<City> AddCityAsync(City model);
    Task<City> UpdateCityAsync(City model);
    Task UpdateCityIsActiveStatusAsync(Guid cityId, bool isActive);
}