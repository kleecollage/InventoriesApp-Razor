using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Helpers;
using RPInventories.Models;
using RPInventories.VewModels;

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
        Product = new ProductCreateEditViewModel();
        return Page();
    }

    [BindProperty] public ProductCreateEditViewModel Product { get; set; }
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
        
        var existsProductDb = _context.Products.Any(p => 
            p.Name.ToLower().Trim() == Product.Name.ToLower().Trim());
        if (existsProductDb)
        {
            Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
            _serviceNotify.Error($"Product with name ${Product.Name} already exists");
            return Page();
        }

        var newProduct = new Product
        {
            Id = Product.Id,
            Name = Product.Name,
            Description = Product.Description,
            Price = Product.Price,
            BrandId = Product.BrandId,
            Status = Product.Status,
        };

        if (Request.Form.Files.Count > 0)
        {
            IFormFile file = Request.Form.Files.FirstOrDefault();
            newProduct.Image = await Utilerias.LeerImagen(file);
        }

        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();
        _serviceNotify.Success($"SUCCESS!. Product {Product.Name} added.");

        return RedirectToPage("./Index");
    }
}
