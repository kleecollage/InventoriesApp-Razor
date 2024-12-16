using System.ComponentModel.DataAnnotations;

namespace RPInventories.Models;

public class Department
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Department name is required.")]
    [MinLength(4, ErrorMessage = "Department name must be at least 4 characters long.")]
    [MaxLength(50, ErrorMessage = "Department name must be between 4 and 50 characters.")]
    public string Name { get; set; }
    
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Department number must be between 5 and 200 characters.")]
    public string Description { get; set; }
    
    [Display(Name = "Creation Date")]
    [Required(ErrorMessage = "Creation date is required.")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
}