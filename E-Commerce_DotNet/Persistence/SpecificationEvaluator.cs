using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> baseQuery, Specification<T> specification) where T : class
        {
            var query = baseQuery; // baseQuery -> sets the DbSet of the meant Entity.

            if (specification.Criteria is not null) query = query.Where(specification.Criteria);

            query = specification.Includes.Aggregate(query, (currentQuery, include) => currentQuery.Include(include));

            return query;
        }
    }
}
