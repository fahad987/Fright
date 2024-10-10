using Ensure.Entities.Domain;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Application.IService;

public interface IServiceTypeService
{
    Task<ServiceType> AddServiceTypeAsync(ServiceType model);
    Task<ServiceType> UpdateServiceTypeAsync(ServiceType model);
    Task<List<ServiceType>> GetAllServiceTypeAsync(ServiceTypeFilter model);
    Task<bool> RemoveServiceTypeAsync(Guid id);
    Task UpdateServiceTypeIsActiveStatusAsync(Guid serviceTypeId, bool isActive);
}