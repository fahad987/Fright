namespace Ensure.Entities.Domain;

public class Surcharge
{
    public Guid id { get; set; }  = Guid.Empty;
    public Guid airlineId { get; set; }  = Guid.Empty;
    public string name { get; set; } = string.Empty;
    public float amount { get; set; } = 0;
}