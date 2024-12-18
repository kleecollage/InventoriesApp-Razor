using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Helpers;
using RPInventories.Models;
using RPInventories.VewModels;

namespace RPInventories.Pages.Users;
public class EditModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly INotyfService _serviceNotify;
    private readonly FactoryUser _factoryUser;

    public EditModel(InventoriesContext context, INotyfService serviceNotify, FactoryUser factoryUser)
    {
        _context = context;
        _serviceNotify = serviceNotify;
        _factoryUser = factoryUser;
    }

    [BindProperty] public new UserEditViewModel User { get; set; }
    public SelectList Profiles { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _serviceNotify.Warning($"User ID must not be null");
            return NotFound();
        }

        var userDb =  await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        
        if (userDb == null)
        {
            _serviceNotify.Warning($"User with ID {id} does not exist");
            return NotFound();
        }
        
        Profiles = new SelectList(_context.Profiles.AsNoTracking(), "Id", "Name");
        User = _factoryUser.CreateUserEdit(userDb);
        
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Profiles = new SelectList(_context.Profiles.AsNoTracking(), "Id", "Name");
            return Page();
        }
        
        var existsUserDb = await _context.Users.AnyAsync(u => 
            u.Username.ToLower().Trim() == User.Username.ToLower().Trim() && u.Id != User.Id);
        
        if (existsUserDb)
        {
            Profiles = new SelectList(_context.Profiles.AsNoTracking(), "Id", "Name");
            _serviceNotify.Warning($"Username ${User.Username} already taken");
            return Page();
        }

        var userDb = await _context.Users.FindAsync(User.Id);
        _factoryUser.UpdateDataUser(User, userDb);

        if (Request.Form.Files.Count > 0)
        {
            IFormFile file = Request.Form.Files.FirstOrDefault();
            using var dataStream = new MemoryStream();
            await file.CopyToAsync(dataStream);
            userDb.Photo = dataStream.ToArray();
        }
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(User.Id))
                return NotFound();

            throw;
        }
        _serviceNotify.Success($"SUCCESS. {User.Name} updated correctly");
        return RedirectToPage("./Index");
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}
