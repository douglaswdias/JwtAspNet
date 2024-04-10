using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAspNet.Models;
using Microsoft.IdentityModel.Tokens;

namespace JwtAspNet.Services;

public class TokenService
{
  public string Create(User user)
  {
    var handler = new JwtSecurityTokenHandler();

    var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);
    var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      SigningCredentials = credentials,
      Expires = DateTime.UtcNow.AddHours(2),
      Subject = GenerateClaims(user)
    };
    var token = handler.CreateToken(tokenDescriptor);
    return handler.WriteToken(token);
  }

  private static ClaimsIdentity GenerateClaims(User user)
  {
    var claims = new ClaimsIdentity();
    claims.AddClaim(new Claim("id", user.Id.ToString()));
    claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));
    claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
    claims.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
    claims.AddClaim(new Claim("image", user.Image));

    foreach (var role in user.Roles)
      claims.AddClaim(new Claim(ClaimTypes.Role, role));

    return claims;
  }
}