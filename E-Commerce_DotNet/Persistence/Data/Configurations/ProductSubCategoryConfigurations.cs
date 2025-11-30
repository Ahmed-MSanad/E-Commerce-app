using Domain.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class ProductSubCategoryConfigurations : IEntityTypeConfiguration<ProductSubCategory>
    {
        public void Configure(EntityTypeBuilder<ProductSubCategory> builder)
        {
            builder.HasMany(productSubCategory => productSubCategory.Products)
                .WithOne(product => product.ProductSubCategory)
                .HasForeignKey(product => product.ProductSubCategoryId);

            builder.HasOne(productSubCategory => productSubCategory.ProductCategory)
                .WithMany(productCategory => productCategory.ProductSubCategories)
                .HasForeignKey(productSubCategory => productSubCategory.ProductCategoryId);
        }
    }
}
