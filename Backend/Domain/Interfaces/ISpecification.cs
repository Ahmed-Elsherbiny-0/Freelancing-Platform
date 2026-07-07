using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>>? Criteria { get; }
        public IList<Expression<Func<T, object>>>? Include { get; }
        IList<string>? ThenInclude { get; }
        public Expression<Func<T, object>>? OrderBy { get; }
        public Expression<Func<T, object>>? OrderByDesc { get; }
        public bool? Distinct { get; }
        public bool HasPagination { get; }

        public int Take { get; }
        public int Skip { get; }
        public IQueryable<T> AddCriteriaOnly(IQueryable<T> query);

    }
    public interface ISpecification<T,TResult> : ISpecification<T> where T: class
    {
        public Expression<Func<T, TResult>>? Select { get; }

    }
}
