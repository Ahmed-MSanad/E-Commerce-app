using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Product
{
    public class ProductSubCategory : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Image { get; set; } = default!;
        public int ProductCategoryId { get; set; } = default!; // Foreign key to ProductCategory
        [InverseProperty("ProductSubCategories")]
        public ProductCategory ProductCategory { get; set; } = default!; // Navigation property to ProductCategory
        [InverseProperty("ProductSubCategory")]
        public List<Product> Products { get; set; } = new(); // Navigation property to Product
    }
}
