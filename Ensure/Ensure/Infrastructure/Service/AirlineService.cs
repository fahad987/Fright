using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class AirlineService : IAirlineService
{
    private readonly IAirlineRepo _airlineRepo;
    private readonly ISurchargeRepo _surchargeRepo;

    public AirlineService(IAirlineRepo airlineRepo, ISurchargeRepo surchargeRepo)
    {
        _airlineRepo = airlineRepo;
        _surchargeRepo = surchargeRepo;
    }

    public async Task<Airline> AddAirlineAsync(Airline model)
    {
        var (status, message) = await _surchargeRepo.ValidateSurchargeModel(model.surcharges);
        if (!status)
            throw new Exception(message);
        var result =  await _airlineRepo.AddAirlineAsync(model);
        result.surcharges = await _surchargeRepo.AddSurchargeAsync(model.surcharges, result.id);
        return result;
    }

    public async Task<Airline> UpdateAirlineAsync(Airline model)
        => await _airlineRepo.UpdateAirlineAsync(model);

    public async Task<List<Airline>> GetAllAirlineAsync(AirlineFilter model)
    {
        var result=await _airlineRepo.GetAllAirlineAsync(model);
            var surcharge = await _surchargeRepo.GetAllSurchargeAsync(
            result.Select(x => x.id).ToList());
        foreach (var row in result)
        {
            row.surcharges = surcharge.Where(x => x.airlineId == row.id).ToList();
        }
        return result;
    }

    public async Task<Airline> GetAirlineByIdAsync(Guid airlineId)
    {
        var result=await _airlineRepo.GetAirlineByIdAsync(airlineId);
        result.surcharges = await _surchargeRepo.GetAllSurchargeAsync(airlineId);
        return result;
    } 

    public async Task UpdateAirlineIsActiveStatusAsync(Guid airlineId, bool isActive)
        => await _airlineRepo.UpdateAirlineIsActiveStatusAsync(airlineId, isActive);
}