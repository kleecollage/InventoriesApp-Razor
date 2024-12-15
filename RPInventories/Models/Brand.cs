using System.ComponentModel.DataAnnotations;

namespace RPInventories.Models;

public class Brand
{
    public int Id { get; set; }
    
    [Required (ErrorMessage = "Brand name is required")]
    [MinLength(5, ErrorMessage = "Brand name must be at least 5 characters")]
    [MaxLength(50, ErrorMessage = "Brand name cannot be more than 50 characters")]
    [Display(Name ="Brand")]
    public string Name { get; set; }
    public virtual ICollection<Product> Products { get; set; }
}