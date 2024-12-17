using System.ComponentModel.DataAnnotations;

namespace RPInventories.VewModels;

public class UserRegisterViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MinLength(5, ErrorMessage = "Name must be at least 5 characters"),
     MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; }
    
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Username account is required")]
    [MinLength(5, ErrorMessage = "Username must be at least 5 characters"),
     MaxLength(20, ErrorMessage = "Username cannot exceed 20 characters")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Password account is required")]
    [MinLength(5, ErrorMessage = "Password must be at least 5 characters"),
     MaxLength(20, ErrorMessage = "Password cannot exceed 20 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Password confirmation is required")]
    [MinLength(5, ErrorMessage = "Password must be at least 5 characters"),
     MaxLength(20, ErrorMessage = "Password cannot exceed 20 characters")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "Email address is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    public string Phone { get; set; }
    
    [Required(ErrorMessage = "User profile is required")]
    [Display(Name = "Profile")]
    public int ProfileId { get; set; }
}