namespace EnsureFreightInc.Entities.Domain;

public class FileInp
{
    public Guid id { get; set; } = Guid.Empty;
    public IFormFile? file { get; set; } 

}