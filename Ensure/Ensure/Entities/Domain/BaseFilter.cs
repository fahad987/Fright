using System.Text.Json.Serialization;
using Ensure.Entities.Constant;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ensure.Entities.Domain;

public abstract class BaseFilter
{
    // [Range(0, int.MaxValue, ErrorMessage = "Page should not be negative!")]
    public virtual int pageNo { get; set; } = 0;

    public virtual int pageSize { get; set; } = Util.pageSize;

    public virtual string search { get; set; } = string.Empty;
    [JsonIgnore] [BindNever] public virtual Guid userId { get; set; }
    [JsonIgnore] [BindNever] public virtual Guid branchId { get; set; }
}