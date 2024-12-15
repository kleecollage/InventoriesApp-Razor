using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Brands;
public class IndexModel : PageModel
{
    private readonly InventoriesContext _context;

    public IndexModel(InventoriesContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Brand> Brands { get;set; }

    public async Task OnGetAsync()
    {
        Brands = await _context.Brands.ToListAsync();
    }
}

