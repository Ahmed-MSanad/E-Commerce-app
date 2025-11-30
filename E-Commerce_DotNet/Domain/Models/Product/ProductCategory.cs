using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Product
{
    public class ProductCategory : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Image { get; set; } = default!;
        [InverseProperty("ProductCategory")]
        public List<ProductSubCategory> ProductSubCategories { get; set; } = new(); // Navigation property to ProductSubCategory
    }
}
