using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Brands;
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

    [BindProperty] public Brand Brand { get; set; }

    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _serviceNotify.Error($"Fix the problems with {Brand.Name} and try again.");
            return Page();
        }

        var brandExistsOnDb = _context.Brands.Any(b => b.Name.ToLower().Trim() == Brand.Name.ToLower().Trim());
        if (brandExistsOnDb)
        {
            _serviceNotify.Error($"Brand {Brand.Name} already exists.");
            return Page();
        }
        
        _context.Brands.Add(Brand);
        await _context.SaveChangesAsync();
        _serviceNotify.Success($"SUCCESS. Brand {Brand.Name} added.");

        return RedirectToPage("./Index");
    }
}

