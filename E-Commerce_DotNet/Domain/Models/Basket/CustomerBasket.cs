namespace Domain.Models.Basket
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public IEnumerable<BasketItem> Basket { get; set; }
    }
}
