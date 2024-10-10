using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class ServiceTypeService : IServiceTypeService
{
    private readonly IServiceTypeRepo _serviceTypeRepo;

    public ServiceTypeService(IServiceTypeRepo serviceTypeRepo)
    {
        _serviceTypeRepo = serviceTypeRepo;
    }

    public async Task<ServiceType> AddServiceTypeAsync(ServiceType model)
        => await _serviceTypeRepo.AddServiceTypeAsync(model);

    public async Task<ServiceType> UpdateServiceTypeAsync(ServiceType model)
        => await _serviceTypeRepo.UpdateServiceTypeAsync(model);

    public async Task<List<ServiceType>> GetAllServiceTypeAsync(ServiceTypeFilter model)
        => await _serviceTypeRepo.GetAllServiceTypeAsync(model);

    public async Task<bool> RemoveServiceTypeAsync(Guid id)
        => await _serviceTypeRepo.RemoveServiceTypeAsync(id);
    public async Task UpdateServiceTypeIsActiveStatusAsync(Guid serviceTypeId, bool isActive)
        =>await _serviceTypeRepo.UpdateServiceTypeIsActiveStatusAsync(serviceTypeId,isActive);
}