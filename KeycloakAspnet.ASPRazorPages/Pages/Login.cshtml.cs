using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeycloakAspnet.ASPRazorPages.Pages;

public class LoginModel : PageModel
{
    public IActionResult OnGet(string returnUrl = "/")
    {
        if (!User.Identity?.IsAuthenticated ?? false)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl });
        }
        else
        {
            return RedirectToPage("/Index");
        }
    }
}
