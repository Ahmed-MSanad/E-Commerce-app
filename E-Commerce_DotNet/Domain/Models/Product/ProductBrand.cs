using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Product
{
    public class ProductBrand : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Logo { get; set; } = default!;
        [InverseProperty("ProductBrand")]
        public List<Product> Products { get; set; } = new(); // Navigation property to Product
    }
}
