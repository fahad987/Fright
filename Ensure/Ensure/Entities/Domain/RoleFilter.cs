
using Ensure.Entities.Enum;

namespace Ensure.Entities.Domain;

public class RoleFilter:BaseFilter
{
     public IsActiveEnum isActive { get; set; } = IsActiveEnum.Both;
}