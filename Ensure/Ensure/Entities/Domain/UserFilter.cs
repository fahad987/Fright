using Ensure.Entities.Enum;

namespace Ensure.Entities.Domain;

public class UserFilter:BaseFilter
{
    public IsActiveEnum isActive { get; set; } = IsActiveEnum.Both;
    public List<Guid> branches { get; set; } = new();
    public List<Guid> roles { get; set; } = new();
}
