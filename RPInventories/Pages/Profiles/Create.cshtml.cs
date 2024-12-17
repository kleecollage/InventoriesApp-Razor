using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Profiles;
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
        return Page();
    } 
    
    [BindProperty] public Profile Profile { get; set; }

    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _serviceNotify.Error("Fix the problems before continue");
            return Page();
        }
        
        var existsProfileDb = _context.Profiles.Any(p => 
            p.Name.ToLower().Trim() == Profile.Name.ToLower().Trim());
        if (existsProfileDb)
        {
            _serviceNotify.Error($"Profile with name ${Profile.Name} already exists");
            return Page();
        }

        _context.Profile.Add(Profile);
        await _context.SaveChangesAsync();
        _serviceNotify.Success($"SUCCESS. Profile {Profile.Name} added.");
        
        return RedirectToPage("./Index");
    }
}
