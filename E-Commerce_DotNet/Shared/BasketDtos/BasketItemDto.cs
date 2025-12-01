using System.ComponentModel.DataAnnotations;

namespace Shared.BasketDtos
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        [Range(1, double.MaxValue)]
        public decimal Price { get; set; } = default!;
        [Range(1, 10)]
        public int BasketQuantity { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public List<string> ProductImages { get; set; } = new();
    }
}
