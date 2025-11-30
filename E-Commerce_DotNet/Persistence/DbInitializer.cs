using System.Text.Json;
using Domain.Contracts;
using Domain.Models.Product;
using Persistence.Data;

namespace Persistence
{
    public class DbInitializer(StoreDbContext dbContext) : IDbInitializer
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
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
