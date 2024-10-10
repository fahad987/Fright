using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class CityService : ICityService
{
    private readonly ICityRepo _cityRepo;

    public CityService(ICityRepo cityRepo)
    {
        _cityRepo = cityRepo;
    }

    public async Task<List<City>> GetAllCityAsync(CityFilter model)
        => await _cityRepo.GetAllCityAsync(model);
    public async Task<City> AddCityAsync(City model)
        =>await _cityRepo.AddCityAsync(model);

    public async Task<City> UpdateCityAsync(City model)
        =>await _cityRepo.UpdateCityAsync(model);

    public async Task UpdateCityIsActiveStatusAsync(Guid cityId,bool isActive)
        =>await _cityRepo.UpdateCityIsActiveStatusAsync(cityId,isActive);
}