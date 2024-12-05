using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Saver.Client.Services;
using Saver.ServiceDefaults;

namespace Saver.Client.Infrastructure;

public class JwtAuthenticationStateProvider(TokenService protectedStorage, IConfiguration configuration) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await protectedStorage.GetAccessToken();
        var notAuthenticatedPrincipal = new ClaimsPrincipal(new ClaimsIdentity([]));

        if (token is null)
        {
            return new AuthenticationState(notAuthenticatedPrincipal);
        }

        var claims = ParseClaims(token).ToArray();
        if (claims.Length == 0)
        {
            return new AuthenticationState(notAuthenticatedPrincipal);
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return new AuthenticationState(claimsPrincipal);
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private IEnumerable<Claim> ParseClaims(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            using var rsa = RSA.Create();
            var configSection = configuration.GetSection("Identity");
            var publicKey = configSection.GetRequiredValue<string>("PublicKey");
            var issuer = configSection.GetRequiredValue<string>("Issuer");
            rsa.ImportRSAPublicKey(new ReadOnlySpan<byte>(Convert.FromBase64String(publicKey)), out _);

            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa)
            }, out _);

            var readToken = handler.ReadJwtToken(token);
            return readToken.Claims;
        }
        catch
        {
            return [];
        }
    }
}