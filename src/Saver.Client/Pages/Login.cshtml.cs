using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Saver.Client.Infrastructure;
using Saver.Client.ViewModels;
using Saver.IdentityService.Contracts;

namespace Saver.Client.Pages;

public class LoginModel(IIdentityApiClient identityApiClient) : PageModel
{
    [BindProperty] public LoginViewModel LoginViewModel { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var loginResponse = await identityApiClient.LoginAsync(new LoginRequest { Email = LoginViewModel.Email, Password = LoginViewModel.Password });
        if (!loginResponse.IsSuccessful)
        {
            ModelState.AddModelError(nameof(LoginViewModel), "Invalid credentials. Please try again.");
            return Page();
        }

        HttpContext.Response.Cookies.Append(SecurityTokenCookieNames.AccessToken, loginResponse.Content.Token, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddMinutes(30),
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        return Redirect("/");
    }
}