using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Helpers;
using RPInventories.Models;
using RPInventories.VewModels;

namespace RPInventories.Pages.Users;

public class CreateModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;
    private readonly FactoryUser _factoryUser;

    public CreateModel(InventoriesContext context, INotyfService serviceNotify, FactoryUser factoryUser)
    {
        _context = context;
        _serviceNotify = serviceNotify;
        _factoryUser = factoryUser;
    }

    public IActionResult OnGet()
    {
        Profiles = new SelectList(_context.Profile, "Id", "Name");
        return Page();
    }

    [BindProperty] public new UserRegisterViewModel User { get; set; }
    public SelectList Profiles { get; set; }

    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Profiles = new SelectList(_context.Profiles.AsNoTracking(), "Id", "Email");
            _serviceNotify.Error("Fix the problems before continue");
            return Page();
        }

        var existsUserDb = _context.Users.Any(u =>
            u.Username.ToLower().Trim() == User.Username.ToLower().Trim());
        if (existsUserDb)
        {
            Profiles = new SelectList(_context.Users.AsNoTracking(), "Id", "Name");
            _serviceNotify.Error($"User with username ${User.Username} already taken");
            return Page();
        }

        var addUser = _factoryUser.CreateUser(User);
        _context.Users.Add(addUser);
        await _context.SaveChangesAsync();
        _serviceNotify.Success($"SUCCESS. Product {User.Username} added.");

        return RedirectToPage("./Index");
    }
    public async Task<JsonResult> OnGetExistsUsername(string username)
    {
        var existsUserDb = await _context.Users.AnyAsync(u =>
            u.Username.ToLower().Trim() == username.ToLower().Trim());
        
        var existsUser = existsUserDb ? true : false;

        return new JsonResult(new { existsUser });
    }

}








