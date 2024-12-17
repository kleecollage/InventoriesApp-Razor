using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Products;
public class DeleteModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;

    public DeleteModel(InventoriesContext context, INotyfService serviceNotify)
    {
        _context = context;
        _serviceNotify = serviceNotify;
    }

    [BindProperty] public Product Product { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FirstOrDefaultAsync(d => d.Id == id);

        if (product == null)
        {
            _serviceNotify.Warning($"Product ID must not be null");
            return NotFound();
        }

        Product = product;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            Product = product;
            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();
        }
        _serviceNotify.Success($"SUCCESS. {Product.Name} removed correctly!");
        return RedirectToPage("./Index");
    }
}
