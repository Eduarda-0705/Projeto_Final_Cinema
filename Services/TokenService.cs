using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims;
using System.Text;       
namespace CinemaApi.Services;
public class TokenService
{
    public string GenerateToken(string email)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MinhaChaveJWTSuperSeguraComMaisDe32CaracteresParaFuncionarCorretamente123456789"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email)
        };
       var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}