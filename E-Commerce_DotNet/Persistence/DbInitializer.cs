using System.Text.Json;
using Domain.Contracts;
using Domain.Models.Identity;
using Domain.Models.Order;
using Domain.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Persistence.Data;
using Persistence.Identity;

namespace Persistence
{
    public class DbInitializer(StoreDbContext dbContext, 
        StoreIdentityDbContext identityDbContext, RoleManager<IdentityRole> roleManager, UserManager<User> userManager) : IDbInitializer
    {
        public async Task InitializeStoreDbAsync()
        {
            try
            {
                if (!dbContext.ProductBrands.Any())
                {
                    var productBrands = File.ReadAllText(@"..\Persistence\Data\Seeding\ProductBrands.json");
                    
                    var productBrandsJson = JsonSerializer.Deserialize<List<ProductBrand>>(productBrands);

                    if(productBrandsJson is not null && productBrandsJson.Any())
                    {
                        await dbContext.ProductBrands.AddRangeAsync(productBrandsJson);
                        await dbContext.SaveChangesAsync();
                    }
                }

                if (!dbContext.ProductCategories.Any())
                {
                    var productCategories = File.ReadAllText(@"..\Persistence\Data\Seeding\ProductCategories.json");

                    var productCategoriesJson = JsonSerializer.Deserialize<List<ProductCategory>>(productCategories);

                    if(productCategoriesJson is not null && productCategoriesJson.Any())
                    {
                        await dbContext.ProductCategories.AddRangeAsync(productCategoriesJson);
                        await dbContext.SaveChangesAsync();
                    }
                }

                if (!dbContext.ProductSubCategories.Any())
                {
                    var productSubCategories = File.ReadAllText(@"..\Persistence\Data\Seeding\ProductSubCategories.json");

                    var productSubCategoriesJson = JsonSerializer.Deserialize<List<ProductSubCategory>>(productSubCategories);

                    if(productSubCategoriesJson is not null && productSubCategoriesJson.Any())
                    {
                        await dbContext.ProductSubCategories.AddRangeAsync(productSubCategoriesJson);
                        await dbContext.SaveChangesAsync();
                    }
                }

                if (!dbContext.Products.Any())
                {
                    var products = File.ReadAllText(@"..\Persistence\Data\Seeding\Products.json");

                    var productsJson = JsonSerializer.Deserialize<List<Product>>(products);

                    if (productsJson is not null && productsJson.Any())
                    {
                        await dbContext.Products.AddRangeAsync(productsJson);
                        await dbContext.SaveChangesAsync();
                    }
                }

                if (!dbContext.ProductImages.Any())
                {
                    var productImages = File.ReadAllText(@"..\Persistence\Data\Seeding\ProductImages.json");

                    var productImagesJson = JsonSerializer.Deserialize<List<ProductImage>>(productImages);

                    if (productImagesJson is not null && productImagesJson.Any())
                    {
                        await dbContext.ProductImages.AddRangeAsync(productImagesJson);
                        await dbContext.SaveChangesAsync();
                    }
                }

                if (!dbContext.DeliveryMethods.Any())
                {
                    var deliveryMethods = File.ReadAllText(@"..\Persistence\Data\Seeding\DeliveryMethods.json");

                    var deliveryMethodsDeserilized = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethods);

                    if(deliveryMethodsDeserilized is not null && deliveryMethodsDeserilized.Any())
                    {
                        await dbContext.DeliveryMethods.AddRangeAsync(deliveryMethodsDeserilized);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task InitializeIdentityStoreDbAsync()
        {
            try
            {
                if (identityDbContext.Database.GetPendingMigrations().Any())
                    await identityDbContext.Database.MigrateAsync();
                if (!roleManager.Roles.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!userManager.Users.Any())
                {
                    var admin = new User
                    {
                        DisplayName = "Admin",
                        Email = "Admin@gmail.com",
                        UserName = "Admin1234",
                        PhoneNumber = "12345678910"
                    };
                    var superAdmin = new User
                    {
                        DisplayName = "Super Admin",
                        Email = "SuperAdmin@gmail.com",
                        UserName = "SuperAdmin1234",
                        PhoneNumber = "12345678910"
                    };
                    await userManager.CreateAsync(admin, "password1234#");
                    await userManager.CreateAsync(superAdmin, "password1234#");

                    await userManager.AddToRoleAsync(admin, "Admin");
                    await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
