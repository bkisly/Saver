using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Saver.IdentityService.Configuration;

namespace Saver.IdentityService.Services;

public class JwtTokenProvider(IIdentityConfigurationProvider config) : IJwtTokenProvider
{
    public string ProvideToken(IdentityUser user)
    {
        var handler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.UTF8.GetBytes(config.SecretKey);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(secretKey),
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = signingCredentials,
            Expires = DateTime.UtcNow.AddMinutes(config.ExpirationTimeMinutes),
            Subject = GenerateClaims(user)
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(IdentityUser user)
    {
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim("id", user.Id));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

        if (user.UserName != null)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
        }

        if (user.Email != null)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        }

        return claimsIdentity;
    }
}