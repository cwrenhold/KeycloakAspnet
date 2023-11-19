using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeycloakAspnet.ASPRazorPages.Pages;

[Authorize]
public class UserModel : PageModel
{
    public List<(string Type, string Value)> Claims { get; private set; } = new();

    public void OnGet()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            this.Claims = User.Claims.Select(c => (c.Type, c.Value)).ToList();
        }
    }
}
