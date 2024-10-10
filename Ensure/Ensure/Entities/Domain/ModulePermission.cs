using Ensure.Entities.Enum;

namespace Ensure.Entities.Domain;

public class ModulePermission
{
    public ModulePermissionEnum id { get; set; } = 0;
    public ModuleEnum moduleId { get; set; } = 0;
    public string name { get; set; } = string.Empty;
    public string module { get; set; } = string.Empty;
}