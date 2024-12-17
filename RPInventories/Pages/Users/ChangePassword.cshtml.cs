using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Helpers;
using RPInventories.VewModels;

namespace RPInventories.Pages.Users;

public class ChangePasswordModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;
    private readonly FactoryUser _factoryUser;

    public ChangePasswordModel(InventoriesContext context, INotyfService serviceNotify, FactoryUser factoryUser)
    {
        _context = context;
        _serviceNotify = serviceNotify;
        _factoryUser = factoryUser;
    }
    
    [BindProperty] public new UserChangePasswordViewModel User {get; set;}

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotify.Warning("Please provide a valid ID.");
            return NotFound();
        }
        
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userDb == null)
        {
            _serviceNotify.Warning("User not found.");
            return NotFound();
        }
        
        User = _factoryUser.CreateUserChangePassword(userDb);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();
        
        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == User.Id);
        _factoryUser.UpdatePasswordUser(User, userDb);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
        _serviceNotify.Success($"SUCCESS. {User.Username} password updated.");
        return RedirectToPage("./Index");
    }
}