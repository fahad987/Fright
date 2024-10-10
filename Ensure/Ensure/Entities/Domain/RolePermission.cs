using Ensure.Entities.Enum;

namespace Ensure.Entities.Domain;

public class RolePermission
{
    public Guid id { get; set; }=Guid.Empty;
    public Guid roleId { get; set; }=Guid.Empty;
    public ModulePermissionEnum permissionId { get; set; }=0;
}