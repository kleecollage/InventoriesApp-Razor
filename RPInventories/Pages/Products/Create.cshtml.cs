using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Products;
public class CreateModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;

    public CreateModel(InventoriesContext context, INotyfService serviceNotify)
    {
        _context = context;
        _serviceNotify = serviceNotify;
    }

    public IActionResult OnGet()
    {
    Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
        return Page();
    }

    [BindProperty] public Product Product { get; set; }
    public SelectList Brands { get; set; }

    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
            _serviceNotify.Error("Fix the problems before continue");
            return Page();
        }
        
        var existsProductDb = _context.Products.Any(p => p.Name.ToLower().Trim() == Product.Name.ToLower().Trim());
        if (existsProductDb)
        {
            Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
            _serviceNotify.Error($"Product with name ${Product.Name} already exists");
            return Page();
        }

        _context.Products.Add(Product);
        await _context.SaveChangesAsync();
        _serviceNotify.Success($"SUCCESS. Product {Product.Name} added.");

        return RedirectToPage("./Index");
    }
}
