using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Saver.IdentityService.Configuration;

namespace Saver.IdentityService.Services;

public class JwtTokenProvider(IIdentityConfigurationProvider config) : IJwtTokenProvider
{
    public string ProvideToken(IdentityUser user)
    {
        var handler = new JwtSecurityTokenHandler();
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(new ReadOnlySpan<byte>(Convert.FromBase64String(config.PrivateKey)), out _);

        var signingCredentials = new SigningCredentials(
            new RsaSecurityKey(rsa),
            SecurityAlgorithms.RsaSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = config.Issuer,
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