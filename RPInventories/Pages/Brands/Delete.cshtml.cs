using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Brands;
public class DeleteModel : PageModel
{
    private readonly InventoriesContext _context;

    public DeleteModel(InventoriesContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Brand Brand { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var brand = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);

        if (brand == null)
        {
            return NotFound();
        }

        Brand = brand;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var brand = await _context.Brands.FindAsync(id);
        if (brand != null)
        {
            Brand = brand;
            _context.Brands.Remove(Brand);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
