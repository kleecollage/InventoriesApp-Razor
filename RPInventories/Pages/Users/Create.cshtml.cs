using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Users;
public class CreateModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;

    public CreateModel(InventoriesContext context, INotyfService serviceNotify)
    {
        _context = context;
        _serviceNotify = serviceNotify;
    }

    public IActionResult OnGet()
    { 
        Profiles = new SelectList(_context.Profile, "Id", "Name");
        return Page();
    }

    [BindProperty] public new User User { get; set; }
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
        
        var existsUserDb = _context.Users.Any(p => 
            p.Name.ToLower().Trim() == User.Name.ToLower().Trim());
        if (existsUserDb)
        {
            Profiles = new SelectList(_context.Users.AsNoTracking(), "Id", "Email");
            _serviceNotify.Error($"User with email ${User.Email} already exists");
            return Page();
        }

        _context.User.Add(User);
        await _context.SaveChangesAsync();
        _serviceNotify.Success($"SUCCESS. Product {User.Username} added.");
        
        return RedirectToPage("./Index");
    }
}