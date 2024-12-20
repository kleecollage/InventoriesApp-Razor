using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RPInventories.Models;

namespace RPInventories.VewModels;

public class ProductCreateEditViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Product name is required")]
    [MinLength(5, ErrorMessage = "Product name must be at least 5 characters long"), 
     MaxLength(50, ErrorMessage = "Product name cannot exceed 50 characters")]
    public string Name { get; set; }
    
    [StringLength(200, MinimumLength = 5, 
        ErrorMessage = "Product description cannot exceed 200 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Display(Name = "Brand")]
    [Required(ErrorMessage = "Product brand is required")]
    public int BrandId { get; set; }
    
    public virtual Brand Brand { get; set; }
    
    [Required(ErrorMessage = ("Product price is required"))]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Display(Name = "Status")]
    [Required(ErrorMessage = "Product status is required")]
    public StatusProduct Status { get; set; } = StatusProduct.Active;
    
    public byte[] Image { get; set; }
}