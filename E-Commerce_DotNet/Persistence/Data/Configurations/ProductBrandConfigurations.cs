using Domain.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class ProductBrandConfigurations : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            builder.HasMany(productBrand => productBrand.Products)
                .WithOne(product => product.ProductBrand)
                .HasForeignKey(product => product.ProductBrandId);
        }
    }
}
