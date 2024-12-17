using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;

namespace RPInventories.Pages.Profiles
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

        [BindProperty] public Profile Profile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profile.FirstOrDefaultAsync(m => m.Id == id);

            if (profile == null)
            {
                _serviceNotify.Warning($"Product {Profile.Name} not found");
                return NotFound();
            }

            Profile = profile;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var profile = await _context.Profile.FindAsync(id);
            if (profile != null)
            {
                Profile = profile;
                _context.Profile.Remove(Profile);
                await _context.SaveChangesAsync();
            }
            
            _serviceNotify.Success($"SUCCESS. {Profile.Name} removed correctly!");

            return RedirectToPage("./Index");
        }
    }
}
