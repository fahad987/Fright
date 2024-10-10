using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ensure.Application.IHelper;
using Ensure.Entities.Constant;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Ensure.Infrastructure.Helper;

public class JwtManagerHelper : IJwtManagerHelper
{
  private readonly IConfiguration _configuration;
    private readonly IOptions<Settings> _settings;
   // private readonly long _seconds;
    //private readonly string? _key;

    public JwtManagerHelper(IConfiguration configuration,IOptions<Settings> settings)
    {
        _configuration = configuration;
        _settings = settings;
        //_seconds = Convert.ToInt64(configuration.GetSection("JwtExpireSeconds").Value);
        //_key = configuration.GetSection("JwtKey").ToString();
    }

    public string GenerateJwtToken(Guid userId, List<Guid> roles, Guid jti)
    {
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("userId", userId.ToString()),
            new Claim("roles", Util.GetString(roles)),
            new Claim("jti", jti.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: signIn);
        return new JwtSecurityTokenHandler().WriteToken(token);
       
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}