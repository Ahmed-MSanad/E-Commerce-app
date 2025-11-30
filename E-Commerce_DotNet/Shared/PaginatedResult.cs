namespace Shared
{
    public record PaginatedResult<TEntity>(int totalItemsCount, int pageIndex, int pageSize, IEnumerable<TEntity> itemsList)
    {
    }
}
