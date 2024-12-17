using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Profiles;

public class EditModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;

    public EditModel(InventoriesContext context, INotyfService serviceNotify)
    {
        _context = context;
        _serviceNotify = serviceNotify;
    }

    [BindProperty] public Profile Profile { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotify.Warning($"Profile ID must not be null");
            return NotFound();
        }

        Profile = await _context.Profile.FirstOrDefaultAsync(p => p.Id == id);
        if (Profile == null)
        {
            _serviceNotify.Warning($"Profile with ID {id} does not exist");
            return NotFound();
        }

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existsProfileDb = await _context.Profiles.AnyAsync(p =>
            p.Name.ToLower().Trim() == Profile.Name.ToLower().Trim() && p.Id != Profile.Id);

        if (existsProfileDb)
        {
            _serviceNotify.Warning($"Profile with name ${Profile.Name} already exists");
            return Page();
        }

        _context.Attach(Profile).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProfileExists(Profile.Id))
                return NotFound();

            throw;
        }

        _serviceNotify.Success($"SUCCESS. {Profile.Name} updated correctly");

        return RedirectToPage("./Index");
    }

    private bool ProfileExists(int id)
    {
        return _context.Profile.Any(p => p.Id == id);
    }
}