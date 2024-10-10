using Ensure.Entities.Domain;

namespace Ensure.Application.IRepository;

public interface IBranchRepo
{
    Task<Branch> AddBranchAsync(Branch model);
    Task<Branch> UpdateBranchAsync(Branch model);
    Task<List<Branch>> GetAllBranchAsync();
    Task RemoveBranchAsync(Guid id);

}