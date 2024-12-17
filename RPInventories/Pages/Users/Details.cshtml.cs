using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Users;
public class DetailsModel : PageModel
{
    private readonly InventoriesContext _context;

    public DetailsModel(InventoriesContext context)
    {
        _context = context;
    }

    public new User User { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.User
            .Include(u => u.Profile)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (user == null)
        {
            return NotFound();
        }

        User = user;
        return Page();
    }
}
