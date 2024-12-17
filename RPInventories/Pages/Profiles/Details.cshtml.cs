using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Profiles;
public class DetailsModel : PageModel
{
    private readonly InventoriesContext _context;

    public DetailsModel(InventoriesContext context)
    {
        _context = context;
    }

    public Profile Profile { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var profile = await _context.Profile
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (profile == null)
            return NotFound();

        Profile = profile;
        return Page();
    }
}
