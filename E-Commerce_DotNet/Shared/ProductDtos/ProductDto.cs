using System.Threading.Channels;

namespace Shared.ProductDtos
{
    public record ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public int StockQuantity { get; set; } = default!;
        public List<ProductImageDto> ProductImages { get; set; } = new();
        public string ProductBrand { get; set; } = default!;
        public string ProductSubCategory { get; set; } = default!;
    }
}
