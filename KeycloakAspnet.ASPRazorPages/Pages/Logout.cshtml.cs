using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeycloakAspnet.ASPRazorPages.Pages;

[AllowAnonymous]
public class LogoutModel : PageModel
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public LogoutModel(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var context = this._httpContextAccessor.HttpContext;

        if (context is not null)
        {
            await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var logoutUri = $"{_configuration["KEYCLOAK_ROOT_URL"]}/auth/realms/{_configuration["KEYCLOAK_REALM"]}/protocol/openid-connect/logout?redirect_uri={Uri.EscapeDataString(context.Request.Scheme + "://" + context.Request.Host + "/")}";

            return Redirect(logoutUri);
        }

        return RedirectToPage("/Index");
    }
}
