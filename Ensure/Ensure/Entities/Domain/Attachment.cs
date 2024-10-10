namespace Ensure.Entities.Domain;

public class Attachment
{
    public Guid id { get; set; } = Guid.Empty;
    public Guid referenceId { get; set; } = Guid.Empty;
    public string path { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
}