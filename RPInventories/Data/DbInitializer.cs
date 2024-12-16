using RPInventories.Models;

namespace RPInventories.Data;

public static class DbInitializer
{
    public static void Initialize(InventoriesContext context)
    {
        // Check brands existence
        if (context.Brands.Any())
        {
            return; // BD init with info
        }
        
        // Add registers if there aren't any
        var brands = new Brand[]
        {
            new Brand { Name = "Rino" },
            new Brand { Name = "Rocco" },
            new Brand { Name = "Azuri" },
            new Brand { Name = "Reni" },
            new Brand { Name = "Bazi" },
            new Brand { Name = "Asis" },
        };
        context.Brands.AddRange(brands);
        context.SaveChanges();

        var departments = new Department[]
        {
            new Department{
                Name="General Administration",
                Description= "General Administration",
                CreatedAt= DateTime.Now,
            },
            new Department{
                Name="Human Resources",
                Description= "Human Resources",
                CreatedAt= DateTime.Now
            },
            new Department{
                Name="Material Resources",
                Description= "Material Resources",
                CreatedAt= DateTime.Now
            },
            new Department{
                Name="Computing",
                Description= "Computing",
                CreatedAt= DateTime.Now
            },
            new Department{
                Name="Deports",
                Description= "Deports",
                CreatedAt= DateTime.Now
            }
        };
        context.Departments.AddRange(departments);
        context.SaveChanges();

        var products = new Product[]
        {
            new Product
            {
                Name = "Secretarial Chair",
                Description = "imitation leather chair",
                BrandId = context.Brands.First(b => b.Name == "Rino").Id,
                Price = 950.99m,
            },
            new Product
            {
                Name = "Manegerial Desk",
                Description = "Black mate desk with tempered glass",
                BrandId = context.Brands.First(b => b.Name == "Azuri").Id,
                Price = 1150.99m,
            },
            new Product
            {
                Name = "Industrial coffee maker",
                Description = "Coffee maker with 50 cups capabilities.",
                BrandId = context.Brands.First(b => b.Name == "Rocco").Id,
                Price = 9523.99m,
            },
            new Product
            {
                Name = "Industrial coffee maker",
                Description = "Coffee maker with 50 cups capabilities.",
                BrandId = context.Brands.First(b => b.Name == "Reni").Id,
                Price = 9523.99m,
            },
            new Product
            {
                Name = "Computer",
                Description = "Pc Gamer.",
                BrandId = context.Brands.First(b => b.Name == "Bazi").Id,
                Price = 9523.99m,
            },
            new Product
            {
                Name = "Projector",
                Description = "Wireless projector.",
                BrandId = context.Brands.First(b => b.Name == "Asis").Id,
                Price = 9523.99m,
            },
        };
        context.Products.AddRange(products);
        context.SaveChanges();
        
        
        
        
        
    }
}

















