namespace Ensure.Entities.Constant;

public class Settings
{
    public ConnectionString connectionString { get; set; }
    public string JwtKey { get; set; } = string.Empty;
    public string JwtIssuer { get; set; } = string.Empty;
    public string JwtAudience { get; set; } = string.Empty;
    public string JwtAJwtSubjectudience { get; set; } = string.Empty;
    public double jwtTimeSpan { get; set; } = 0;
    public string resourceDirectory { get; set; } = string.Empty;
}