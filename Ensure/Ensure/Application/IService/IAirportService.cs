using Ensure.Entities.Domain;

namespace Ensure.Application.IService;

public interface IAirportService
{
    Task<List<Airport>> GetAllAirportAsync(AirportFilter model);
    Task<Airport> AddAirportAsync(Airport model);
    Task<Airport> UpdateAirportAsync(Airport model);
    Task UpdateAirportIsActiveStatusAsync(Guid airportId, bool isActive);
}