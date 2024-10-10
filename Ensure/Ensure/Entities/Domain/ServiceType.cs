using System.ComponentModel.DataAnnotations;

namespace EnsureFreightInc.Entities.Domain;

public class ServiceType
{
    public Guid id { get; set; }=Guid.Empty;
    [Required]
    [MaxLength(100,ErrorMessage = "Service can not be greater than {1} characters")]
    public string name { get; set; }=string.Empty;
    [Range(0,999,ErrorMessage = "Rate can not be more than 999")]
    public float rate { get; set; } = 0;

    public bool isActive { get; set; } = true;

}