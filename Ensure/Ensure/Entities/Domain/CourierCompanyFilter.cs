using Ensure.Entities.Enum;

namespace Ensure.Entities.Domain;

public class CourierCompanyFilter : BaseFilter
{
    public IsActiveAirlineEnum isActive { get; set; } = IsActiveAirlineEnum.Enable;
}