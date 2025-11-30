using Domain.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class ProductImageConfigurations : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasOne<Product>().WithMany(product => product.ProductImages).HasForeignKey(productImage => productImage.ProductId);

            builder.HasKey(productImage => new { productImage.ProductId, productImage.Image });
        }
    }
}
