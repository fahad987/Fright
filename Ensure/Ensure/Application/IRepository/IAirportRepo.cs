using Ensure.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface IAirportRepo
{
    Task<List<Airport>> GetAllAirportAsync(AirportFilter model);
    Task<Airport> AddAirportAsync(Airport model);
    Task<Airport> UpdateAirportAsync(Airport model);
    Task UpdateAirportIsActiveStatusAsync(Guid airportId, bool isActive);
}