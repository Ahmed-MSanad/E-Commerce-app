using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Product
{
    // Product has a table
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public int StockQuantity { get; set; } = default!;
        public List<ProductImage> ProductImages { get; set; } = new();
        public int ProductBrandId { get; set; } = default!; // Foreign key to ProductBrand
        [InverseProperty("Products")]
        public ProductBrand ProductBrand { get; set; } = default!; // Navigation property to ProductBrand
        public int ProductSubCategoryId { get; set; } = default!; // Foreign key to ProductSubCategory
        [InverseProperty("Products")]
        public ProductSubCategory ProductSubCategory { get; set; } = default!; // Navigation property to ProductSubCategory
    }
}
