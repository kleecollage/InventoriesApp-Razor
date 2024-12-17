using System.ComponentModel.DataAnnotations;

namespace RPInventories.VewModels;

public class UserEditViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MinLength(5, ErrorMessage = "Name must be at least 5 characters"),
     MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; }
    
    public string LastName { get; set; }
    
    [Display(Name = "User Account")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Email address is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    public string Phone { get; set; }
    
    [Required(ErrorMessage = "User profile is required")]
    [Display(Name = "Profile")]
    public int ProfileId { get; set; }
}