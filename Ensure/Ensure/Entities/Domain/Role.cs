using System.ComponentModel.DataAnnotations;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Entities.Domain;

public class Role
{
    public Guid id { get; set; } = Guid.Empty;
    [Required]
    [MaxLength(20,ErrorMessage = "Role can not be more than {1} characters")]
    public string name { get; set; }=string.Empty;
    public Guid createBy { get; set; }=Guid.Empty;
    public DateTime createDate { get; set; }
    public Guid updateBy { get; set; }=Guid.Empty;
    public DateTime updateDate { get; set; }
    public bool isActive { get; set; } = true;
    public List<RolePermission> permissions { get; set; } = new();

}