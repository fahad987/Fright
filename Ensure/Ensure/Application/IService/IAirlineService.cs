using Ensure.Entities.Domain;

namespace Ensure.Application.IService;

public interface IAirlineService
{
    Task<Airline> AddAirlineAsync(Airline model);
    Task<Airline> UpdateAirlineAsync(Airline model);
    Task<List<Airline>> GetAllAirlineAsync(AirlineFilter model);
    Task<Airline> GetAirlineByIdAsync(Guid airlineId);
    Task UpdateAirlineIsActiveStatusAsync(Guid airlineId, bool isActive);
    
}