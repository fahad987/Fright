using Ensure.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface ICourierCompanyRepo
{
    Task<CourierCompany> AddCourierCompanyAsync(CourierCompany model);
    Task<CourierCompany> UpdateCourierCompanyAsync(CourierCompany model);
    Task<List<CourierCompany>> GetAllCourierCompanyAsync(CourierCompanyFilter model);
    Task<CourierCompany> GetCourierCompanyByIdAsync(Guid courierCompanyId);
    Task UpdateCourierCompanyIsActiveStatusAsync(Guid courierCompanyId, bool isActive);
}