using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPInventories.Models;

namespace RPInventories.Pages.Departments
{
    public class CreateModel : PageModel
    {
        private readonly Data.InventoriesContext _context;
        private readonly INotyfService _serviceNotify;

        public CreateModel(Data.InventoriesContext context, INotyfService serviceNotify)
        {
            _context = context;
            _serviceNotify = serviceNotify;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty] public Department Department { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _serviceNotify.Error($"Fix the problems with {Department.Name} and try again.");
                return Page();
            }

            var departmentExistsOnDb = _context.Departments.Any(b => b.Name.ToLower().Trim() == Department.Name.ToLower().Trim());
            if (departmentExistsOnDb)
            {
                _serviceNotify.Error($"Department {Department.Name} already exists.");
                return Page();
            }
            
            _context.Departments.Add(Department);
            await _context.SaveChangesAsync();
            _serviceNotify.Success($"SUCCESS. Department {Department.Name} added.");
            
            return RedirectToPage("./Index");
        }
    }
}
