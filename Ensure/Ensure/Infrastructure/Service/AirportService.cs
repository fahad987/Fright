using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class AirportService :IAirportService
{
    private readonly IAirportRepo _airportRepo;

    public AirportService(IAirportRepo airportRepo)
    {
        _airportRepo = airportRepo;
    }


    public async Task<List<Airport>> GetAllAirportAsync(AirportFilter model)
        => await _airportRepo.GetAllAirportAsync(model);

    public async Task<Airport> AddAirportAsync(Airport model)
        => await _airportRepo.AddAirportAsync(model);

    public async Task<Airport> UpdateAirportAsync(Airport model)
        => await _airportRepo.UpdateAirportAsync(model);

    public async  Task UpdateAirportIsActiveStatusAsync(Guid airportId,bool isActive)
        => await _airportRepo.UpdateAirportIsActiveStatusAsync(airportId, isActive);
}