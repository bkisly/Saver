using System.Security.Cryptography;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Saver.Client.Services;

public class TokenService(ProtectedLocalStorage localStorage)
{
    public async Task<string?> GetAccessToken()
    {
        try
        {
            var result = await localStorage.GetAsync<string>("AccessToken");
            return result.Value;
        }
        catch (CryptographicException)
        {
            return null;
        }
    }

    public async Task SetAccessToken(string token)
    {
        await localStorage.SetAsync("AccessToken", token);
    }
}