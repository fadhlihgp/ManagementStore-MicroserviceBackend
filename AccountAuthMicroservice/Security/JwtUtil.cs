using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountAuthMicroservice.Models;
using Microsoft.IdentityModel.Tokens;

namespace AccountAuthMicroservice.Security;

public class JwtUtil : IJwtUtil
{
    // Generate configuration has been made
    private readonly IConfiguration _configuration;

    public JwtUtil(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GenerateToken(Account account)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
        
        // Content of payload
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _configuration["JwtSettings:Audience"],
            Expires = DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiresInMinutes"])),
            Issuer = _configuration["JwtSettings:Issuer"],
            IssuedAt = DateTime.Now,
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Email, account.Email),
                new ("UserName", account.UserName),
                new(ClaimTypes.Role, account.Role.Name),
                new ("RoleId", account.Role.Id),
                new("AccountId", account.Id),
                new ("StoreId", account.Member.StoreId),
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}