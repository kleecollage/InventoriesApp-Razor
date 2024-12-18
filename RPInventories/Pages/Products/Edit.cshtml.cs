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
public class EditModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;

    public EditModel(InventoriesContext context, INotyfService serviceNotify)
    {
        _context = context;
        _serviceNotify = serviceNotify;
    }

    [BindProperty] public ProductCreateEditViewModel Product { get; set; }
    public SelectList Brands { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotify.Warning($"Product ID must not be null");
            return NotFound();
        }

        var productDb =  await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (productDb == null)
        {
            _serviceNotify.Warning($"Product with ID {id} does not exist");
            return NotFound();
        }
        
        Brands = new SelectList(_context.Brands.AsNoTracking(), "Id", "Name");
        Product = new ProductCreateEditViewModel
        {
            Id = productDb.Id,
            Name = productDb.Name,
            Description = productDb.Description,
            Price = productDb.Price,
            Status = productDb.Status,
            BrandId = productDb.BrandId,
        };

        if (!String.IsNullOrEmpty(productDb.Image))
        {
            Product.Image = await Utilerias.ConvertirImagenABytes(productDb.Image);
        }
        
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

        var productDb = await _context.Products.FindAsync(Product.Id);
        productDb.Name = Product.Name;
        productDb.Description = Product.Description;
        productDb.Price = Product.Price;
        productDb.Status = Product.Status;
        productDb.BrandId = Product.BrandId;
        
        /*var productDb = new Product
        {
            Id = Product.Id,
            Name = Product.Name,
            Description = Product.Description,
            Price = Product.Price,
            BrandId = Product.BrandId,
            Status = Product.Status,
        };*/

        if (Request.Form.Files.Count > 0)
        {
            IFormFile file = Request.Form.Files.FirstOrDefault();
            productDb.Image = await Utilerias.LeerImagen(file);
        }
        
        // _context.Attach(productDb).State = EntityState.Modified;

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
