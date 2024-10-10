using System.ComponentModel.DataAnnotations;

namespace Ensure.Entities.Domain;

public class Branch
{
    public Guid id { get; set; } = Guid.Empty;
    [Required]
    [MaxLength(200,ErrorMessage = "Branch can not be greater than {1} characters")]
    public string name { get; set; }=string.Empty;
    [MaxLength(50,ErrorMessage = "Code can not be greater than {1} characters")]
    public string code { get; set; }=string.Empty;
    [MaxLength(500,ErrorMessage = "Address can not be greater than {1} characters")]
    public string address { get; set; }=string.Empty;
    public Guid cityId { get; set; } = Guid.Empty;
    public string city { get; set; }=string.Empty;
    public string cityCode { get; set; }=string.Empty;
}