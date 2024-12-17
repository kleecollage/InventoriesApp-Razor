using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Models;

namespace RPInventories.Pages.Departments;
public class DetailsModel : PageModel
{
    private readonly Data.InventoriesContext _context;

    public DetailsModel(Data.InventoriesContext context)
    {
        _context = context;
    }

    public Department Department { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var department = await _context.Departments
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        if (department == null)
        {
            return NotFound();
        }
        else
        {
            Department = department;
        }
        return Page();
    }
}
