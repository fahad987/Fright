using Ensure.Entities.Enum;

namespace Ensure.Entities.Domain;

public class AirlineFilter : BaseFilter
{
    public IsActiveAirlineEnum isActive { get; set; } = IsActiveAirlineEnum.Enable;
}