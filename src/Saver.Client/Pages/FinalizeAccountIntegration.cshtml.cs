using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Saver.Client.Pages;

public class FinalizeAccountIntegrationModel : PageModel
{
    public IActionResult OnGet(string code)
    {
        return Redirect("/accounts");
    }
}