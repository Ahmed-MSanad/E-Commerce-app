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

        protected void AddInclude(Expression<Func<T, object>> include) => Includes.Add(include);
    }
}
