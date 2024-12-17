using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Brands;
public class DetailsModel : PageModel
{
    private readonly InventoriesContext _context;

    public DetailsModel(InventoriesContext context)
    {
        _context = context;
    }

    public Brand Brand { get; set; }
    
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var brand = await _context.Brands
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (brand == null)
        {
            return NotFound();
        }

        Brand = brand;
        return Page();
    }
}
