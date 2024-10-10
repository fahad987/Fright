using Ensure.Entities.Domain;

namespace Ensure.Application.IService;

public interface IBranchService
{
    Task<Branch> AddBranchAsync(Branch model);
    Task<Branch> UpdateBranchAsync(Branch model);
    Task<List<Branch>> GetAllBranchAsync();
    Task RemoveBranchAsync(Guid id);
}