namespace Shared.BasketDtos
{
    public class BasketDto
    {
        public string Id { get; set; }
        public IEnumerable<BasketItemDto> Basket { get; set; }
    }
}
