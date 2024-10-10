namespace Ensure.Entities.Domain;

public class RefreshToken
{
    public Guid id { get; set; } = Guid.Empty;
    public Guid userId { get; set; } = Guid.Empty;
    public string token { get; set; } = string.Empty;
    public Guid jwtId { get; set; } = Guid.Empty;
    public DateTime expiryDate { get; set; }
    public DateTime createDate { get; set; }
}