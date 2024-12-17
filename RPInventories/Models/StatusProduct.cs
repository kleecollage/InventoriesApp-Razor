using System.ComponentModel.DataAnnotations;

namespace RPInventories.Models;

public enum StatusProduct
{
    Down = 0,
    Active = 1,
    
    [Display(Name = "On activation process")]
    OnActivationProcess = 2,
}