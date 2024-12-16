using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Departments;
public class EditModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;

    public EditModel(InventoriesContext context, INotyfService serviceNotify)
    {
        _context = context;
        _serviceNotify = serviceNotify;
    }

    [BindProperty] public Department Department { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotify.Warning($"Department ID must not be null");
            return NotFound();
        }

        var department =  await _context.Departments.FirstOrDefaultAsync(m => m.Id == id);
        if (department == null)
        {
            _serviceNotify.Warning($"Department with ID {id} does not exist");
            return NotFound();
        }
        Department = department;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _serviceNotify.Error($"Fix the problems before editing depertment {Department.Name}");
            return Page();
        }
        
        var departmetnExistsOnDb = _context.Departments.Any(b => 
            b.Name.ToLower().Trim() == Department.Name.ToLower().Trim() && b.Id != Department.Id);
        
        if (departmetnExistsOnDb)
        {
            _serviceNotify.Warning($"Department {Department.Name} already exists");
            return Page();
        }


        _context.Attach(Department).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DepartmentExists(Department.Id))
            {
                return NotFound();
            }

            throw;
        }
        _serviceNotify.Success($"SUCCESS. {Department.Name} updated correctly");
        return RedirectToPage("./Index");
    }

    private bool DepartmentExists(int id)
    {
        return _context.Departments.Any(e => e.Id == id);
    }
}
