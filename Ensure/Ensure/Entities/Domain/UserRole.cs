namespace EnsureFreightInc.Entities.Domain;

public class UserRole
{
    public Guid id { get; set; } = Guid.Empty;
    public Guid roleId { get; set; } = Guid.Empty;
    public Guid userId { get; set; } = Guid.Empty;
    public string role { get; set; } = string.Empty;


}