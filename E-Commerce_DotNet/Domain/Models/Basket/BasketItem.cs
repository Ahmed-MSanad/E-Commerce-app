namespace Domain.Models.Basket
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public int BasketQuantity { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public List<string> ProductImages { get; set; } = new();
    }
}
