namespace Domain.Models.Product
{
    // ProductImage has a table
    public class ProductImage
    {
        public string Image { get; set; } = default!;
        public int ProductId { get; set; } = default!;
    }
}
