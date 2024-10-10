using System.Text.Json.Serialization;
using Ensure.Entities.Enum;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Entities.Domain;

public class CourierCompany
{
    public Guid id { get; set; }  = Guid.Empty;
    public string name { get; set; } = string.Empty;
    public string contactNumber { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public TypeEnum tariffType { get; set; } = TypeEnum.Cargo;
    public float fuel { get; set; } = 0;
    public float security { get; set; } = 0;
    public float civilAviation { get; set; } = 0;  
    public Guid createBy { get; set; }  = Guid.Empty;
    public DateTime createDate { get; set; }
    public bool isActive { get; set; } = true;
    [JsonIgnore] public IFormFile? imageFile { get; set; } = null;
    public Guid imageId { get; set; }  = Guid.Empty;
    public string image { get; set; } = string.Empty;
    public List<Surcharge> surcharges { get; set; } = new();
}