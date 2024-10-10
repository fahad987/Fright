namespace Ensure.Entities.Domain;

public class Airport
{
    public Guid id { get; set; } = Guid.Empty;
    public string name { get; set; } = string.Empty;
    public string code { get; set; } = string.Empty;
    public Guid cityId { get; set; } = Guid.Empty;
    public string city { get; set; } = string.Empty;
    public string cityCode { get; set; } = string.Empty;
    public Guid countryId { get; set; } = Guid.Empty;
    public string country { get; set; } = string.Empty;
    public string countryCode { get; set; } = string.Empty;
    public bool isActive { get; set; } = true; 
}