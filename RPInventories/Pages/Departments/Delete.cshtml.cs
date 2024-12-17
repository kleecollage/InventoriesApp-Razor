using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Departments
{
    public class DeleteModel : PageModel
    {
        private readonly InventoriesContext _context;
        private readonly INotyfService _serviceNotify;

        public DeleteModel(InventoriesContext context, INotyfService serviceNotify)
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

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                _serviceNotify.Warning($"Department with ID {id} does not exist");
                return NotFound();
            }

            Department = department;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                Department = department;
                _context.Departments.Remove(Department);
                await _context.SaveChangesAsync();
            }
            _serviceNotify.Success($"SUCCESS. {Department.Name} removed correctly!");
            return RedirectToPage("./Index");
        }
    }
}
