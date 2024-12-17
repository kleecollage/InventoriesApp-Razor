using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Users;
public class EditModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;

    public EditModel(InventoriesContext context, INotyfService serviceNotify)
    {
        _context = context;
        _serviceNotify = serviceNotify;
    }

    [BindProperty] public new User User { get; set; }
    public SelectList Profiles { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotify.Warning($"User ID must not be null");
            return NotFound();
        }

        User =  await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        
        if (User == null)
        {
            _serviceNotify.Warning($"User with ID {id} does not exist");
            return NotFound();
        }
        
        Profiles = new SelectList(_context.Profile, "Id", "Name");
        
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Profiles = new SelectList(_context.Profile.AsNoTracking(), "Id", "Name");
            return Page();
        }
        
        var existsUserDb = await _context.Users.AnyAsync(m => 
            m.Name.ToLower().Trim() == User.Email.ToLower().Trim() && m.Id != User.Id);
        
        if (existsUserDb)
        {
            Profiles = new SelectList(_context.Brands.AsNoTracking(), "Id", "Email");
            _serviceNotify.Warning($"User with email ${User.Email} already exists");
            return Page();
        }

        _context.Attach(User).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(User.Id))
            {
                return NotFound();
            }

            throw;
        }
        _serviceNotify.Success($"SUCCESS. {User.Name} updated correctly");
        return RedirectToPage("./Index");
    }

    private bool UserExists(int id)
    {
        return _context.User.Any(e => e.Id == id);
    }
}
