using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Brands;
public class CreateModel : PageModel
{
    private readonly InventoriesContext _context;

    public CreateModel(InventoriesContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Brand Brand { get; set; }

    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Brands.Add(Brand);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}

