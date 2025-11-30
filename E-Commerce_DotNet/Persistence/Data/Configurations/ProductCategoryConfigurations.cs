using Domain.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class ProductCategoryConfigurations : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasMany(productCategory => productCategory.ProductSubCategories)
                .WithOne(productSubCategory => productSubCategory.ProductCategory)
                .HasForeignKey(productSubCategory => productSubCategory.ProductCategoryId);
        }
    }
}
