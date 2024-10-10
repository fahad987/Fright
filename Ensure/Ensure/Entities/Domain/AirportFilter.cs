using Ensure.Entities.Enum;

namespace Ensure.Entities.Domain;

public class AirportFilter : BaseFilter
{
    public List<Guid> countries { get; set; } = new();
    public List<Guid> cities { get; set; } = new();
    public IsActiveEnum isActive { get; set; } = IsActiveEnum.Both;
}