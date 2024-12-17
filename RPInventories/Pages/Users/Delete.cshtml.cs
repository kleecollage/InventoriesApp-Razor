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

        var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);

        if (user == null)
        {
            _serviceNotify.Warning($"User ID must not be null");
            return NotFound();
        }

        User = user;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.User.FindAsync(id);
        if (user != null)
        {
            User = user;
            _context.User.Remove(User);
            await _context.SaveChangesAsync();
        }
        
        _serviceNotify.Success($"SUCCESS. {User.Name} removed correctly!");
        
        return RedirectToPage("./Index");
    }
}
