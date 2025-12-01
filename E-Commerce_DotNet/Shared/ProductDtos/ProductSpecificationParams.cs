namespace Shared.ProductDtos
{
    public class ProductSpecificationParams
    {
        public string? search { get; set; } = "";
        private const int MAX_PAGE_SIZE = 8;
        private const int DEFAULT_PAGE_SIZE = 5;
        public int PageIndex { get; set; } = 1;
        private int pageSize = DEFAULT_PAGE_SIZE;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
        }
    }
}
