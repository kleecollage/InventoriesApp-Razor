using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventarios.ViewModels;
using RPInventories.Data;
using RPInventories.Helpers;
using RPInventories.Models;

namespace RPInventories.Pages.Account;

public class LoginModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly INotyfService _serviceNotify;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(InventoriesContext context, 
        IPasswordHasher<User> passwordHasher, 
        INotyfService serviceNotify,
        ILogger<LoginModel> logger )
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _serviceNotify = serviceNotify;
        _logger = logger;
    }

    [BindProperty] public LoginViewModel LoginVm { get; set; }
    public string ReturnUrl { get; set; }

    public async Task OnGetAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        
        if (ModelState.IsValid)
        {
            var userDb = _context.Users
                .Include(u => u.Profile)
                .FirstOrDefault(u => u.Username.ToLower().Trim() == LoginVm.Username.ToLower().Trim() );

            if (userDb == null)
            {
                _serviceNotify.Warning("We sorry, user does not exist");
                return Page();
            }
            
            var result = _passwordHasher.VerifyHashedPassword(userDb, userDb.Password, LoginVm.Password);

            if (result == PasswordVerificationResult.Success)
            {
                // Password ok
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userDb.Username),
                    new Claim("FullName", userDb.Name + " " + userDb.LastName),
                    new Claim(ClaimTypes.Role, userDb.Profile.Name)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = LoginVm.RememberMe,
                };
                
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                
                _logger.LogInformation($"User {userDb.Username} has accessed the system" + DateTime.UtcNow); 

                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }
            else
            {
                _serviceNotify.Warning("Wrong username or password");
                return Page();
            }
        }
        return Page();
    }
}

















