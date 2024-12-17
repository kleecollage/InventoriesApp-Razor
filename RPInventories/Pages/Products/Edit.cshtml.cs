using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Products;
public class EditModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;

    public EditModel(InventoriesContext context, INotyfService serviceNotify)
    {
        _context = context;
        _serviceNotify = serviceNotify;
    }

    [BindProperty] public Product Product { get; set; }
    public SelectList Brands { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotify.Warning($"Product ID must not be null");
            return NotFound();
        }

        Product =  await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (Product == null)
        {
            _serviceNotify.Warning($"Product with ID {id} does not exist");
            return NotFound();
        }
        
        Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
        
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
            return Page();
        }
        
        var existsProductDb = await _context.Products.AnyAsync(m => 
            m.Name.ToLower().Trim() == Product.Name.ToLower().Trim() && m.Id != Product.Id);
        
        if (existsProductDb)
        {
            Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
            _serviceNotify.Warning($"Product with name ${Product.Name} already exists");
            return Page();
        }
        
        _context.Attach(Product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(Product.Id))
            {
                return NotFound();
            }

            throw;
        }
        _serviceNotify.Success($"SUCCESS. {Product.Name} updated correctly");
        return RedirectToPage("./Index");
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
