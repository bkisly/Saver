using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Saver.Client.Infrastructure;

namespace Saver.Client.Pages;

public class LogoutModel : PageModel
{
    public IActionResult OnPost()
    {
        HttpContext.Response.Cookies.Delete(SecurityTokenCookieNames.AccessToken);
        HttpContext.Response.Cookies.Delete(SecurityTokenCookieNames.RefreshToken);
        return Redirect("/");
    }
}