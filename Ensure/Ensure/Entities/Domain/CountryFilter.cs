using Ensure.Entities.Enum;

namespace Ensure.Entities.Domain;

public class CountryFilter : BaseFilter
{
    public IsActiveEnum isActive { get; set; } = IsActiveEnum.Both;

}