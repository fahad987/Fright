using EnsureFreightInc.Entities.Domain;

namespace Ensure.Entities.Domain;

public class Module
{
    // public ModuleEnum id { get; set; } = 0;
    public string name { get; set; }=string.Empty;
    public List<ModulePermission> permissions { get; set; } = new();
}



