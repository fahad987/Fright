namespace EnsureFreightInc.Entities.Domain;

public class Country
{
    public Guid id { get; set; } = Guid.Empty;
    public string name { get; set; }=string.Empty;
    public string code { get; set; }=string.Empty;
    public bool isActive { get; set; } = true;

}