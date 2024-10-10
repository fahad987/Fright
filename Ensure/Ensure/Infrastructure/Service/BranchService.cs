using Ensure.Application.IRepository;
using Ensure.Application.IService;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Service;

public class BranchService :IBranchService
{
    private readonly IBranchRepo _branchRepo;

    public BranchService(IBranchRepo branchRepo)
    {
        _branchRepo = branchRepo;
    }

    public async Task<Branch> AddBranchAsync(Branch model)
        => await _branchRepo.AddBranchAsync(model);

    public async Task<Branch> UpdateBranchAsync(Branch model)
        => await _branchRepo.UpdateBranchAsync(model);

    public async Task<List<Branch>> GetAllBranchAsync()
        => await _branchRepo.GetAllBranchAsync();

    public async Task RemoveBranchAsync(Guid id)
        => await _branchRepo.RemoveBranchAsync(id);
}