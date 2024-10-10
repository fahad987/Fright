namespace Ensure.Entities.Constant;

public class ActiveSession
{
    public Guid userId { get; set; } = Guid.Empty;
    public Guid branchId { get; set; } = Guid.Empty;
    public Guid userPermissionId { get; set; } = Guid.Empty;
}