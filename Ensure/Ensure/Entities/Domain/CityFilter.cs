using Ensure.Entities.Enum;

namespace Ensure.Entities.Domain;

public class CityFilter : BaseFilter
{
    public Guid countryId { get; set; }=Guid.Empty;
    public IsActiveEnum isActive { get; set; } = IsActiveEnum.Both;
}