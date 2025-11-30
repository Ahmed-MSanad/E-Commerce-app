using Domain.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasMany(product => product.ProductImages).WithOne().HasForeignKey(productImage => productImage.ProductId);

            builder.HasOne(product => product.ProductBrand)
                .WithMany(productBrand => productBrand.Products).HasForeignKey(product => product.ProductBrandId).IsRequired(true);

            builder.HasOne(product => product.ProductSubCategory)
                .WithMany(productSubCategory => productSubCategory.Products).HasForeignKey(product => product.ProductSubCategoryId);
        }
    }
}
