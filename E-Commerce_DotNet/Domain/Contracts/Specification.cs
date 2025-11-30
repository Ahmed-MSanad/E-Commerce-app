using System.Linq.Expressions;

namespace Domain.Contracts
{
    public abstract class Specification<T> where T : class
    {
        protected Specification(Expression<Func<T, bool>> _Criteria) {
            Criteria = _Criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool isPaginated { get; private set; }
        protected void AddInclude(Expression<Func<T, object>> include) => Includes.Add(include);
        protected void ApplyPaging(int pageIndex, int pageSize)
        {
            isPaginated = true;
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
        }
    }
}
