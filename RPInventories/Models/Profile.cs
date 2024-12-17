using System.ComponentModel.DataAnnotations;

namespace RPInventories.Models;

public class Profile
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MinLength(5, ErrorMessage = "Name must be at least 5 characters"),
        MaxLength(50, ErrorMessage = "Name must be between 5 and 50 characters")]
    public string Name { get; set; }
    
    public virtual ICollection<User> Users { get; set; }
    
}