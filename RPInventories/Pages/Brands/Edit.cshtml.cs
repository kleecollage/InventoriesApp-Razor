using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Brands;
public class EditModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;

    public EditModel(InventoriesContext context, INotyfService serviceNotify)
    {
        _context = context;
        _serviceNotify = serviceNotify;
    }

    [BindProperty]
    public Brand Brand { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotify.Warning($"Brand ID must not be null");
            return NotFound();
        }

        var brand =  await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);
        if (brand == null)
        {
            _serviceNotify.Warning($"Brand with ID {id} does not exist");
            return NotFound();
        }
        Brand = brand;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _serviceNotify.Error($"Fix the problems before editing brand {Brand.Name}");
            return Page();
        }

        _context.Attach(Brand).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BrandExists(Brand.Id))
            {
                return NotFound();
            }

            throw;
        }
        _serviceNotify.Success($"SUCCESS. {Brand.Name} updated correctly");
        return RedirectToPage("./Index");
    }

    private bool BrandExists(int id)
    {
        return _context.Brands.Any(e => e.Id == id);
    }
}
