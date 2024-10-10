using Ensure.Entities.Enum;

namespace Ensure.Entities.Domain;

public class AdhocTariffFilter : BaseFilter
{
    public List<Guid> origins { get; set; } = new();
    public List<Guid> destination { get; set; } = new();
    public IsActiveEnum isActive { get; set; } = IsActiveEnum.Both;
}