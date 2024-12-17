using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Users;
public class DeleteModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;

    public DeleteModel(InventoriesContext context, INotyfService serviceNotify)
    {
        _context = context;
        _serviceNotify = serviceNotify;
    }

    [BindProperty] public new User User { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) 
            return NotFound();

        User = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (User == null)
        {
            _serviceNotify.Warning($"User not found");
            return NotFound();
        }
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
            return NotFound();

        User = await _context.Users.FindAsync(id);
        
        if (User != null)
        {
            _context.Users.Remove(User);
            await _context.SaveChangesAsync();
        }
        
        _serviceNotify.Success($"SUCCESS. {User.Username} removed correctly!");
        
        return RedirectToPage("./Index");
    }
}
