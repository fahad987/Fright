using Ensure.Entities.Enum;

namespace Ensure.Entities.Domain;

public class ServiceTypeFilter : BaseFilter
{
    public IsActiveEnum isActive { get; set; } = IsActiveEnum.Both;

}