using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class CourierCompanyService : ICourierCompanyService
{
    private readonly ICourierCompanyRepo _courierCompanyRepo;

    public CourierCompanyService(ICourierCompanyRepo courierCompanyRepo)
    {
        _courierCompanyRepo = courierCompanyRepo;
    }

    public async Task<CourierCompany> AddCourierCompanyAsync(CourierCompany model)
        => await _courierCompanyRepo.AddCourierCompanyAsync(model);

    public async Task<CourierCompany> UpdateCourierCompanyAsync(CourierCompany model)
        => await _courierCompanyRepo.UpdateCourierCompanyAsync(model);

    public async Task<List<CourierCompany>> GetAllCourierCompanyAsync(CourierCompanyFilter model)
        => await _courierCompanyRepo.GetAllCourierCompanyAsync(model);

    public async Task<CourierCompany> GetCourierCompanyByIdAsync(Guid courierCompanyId)
        => await _courierCompanyRepo.GetCourierCompanyByIdAsync(courierCompanyId);

    public async Task UpdateCourierCompanyIsActiveStatusAsync(Guid courierCompanyId, bool isActive)
        => await _courierCompanyRepo.UpdateCourierCompanyIsActiveStatusAsync(courierCompanyId, isActive);
}