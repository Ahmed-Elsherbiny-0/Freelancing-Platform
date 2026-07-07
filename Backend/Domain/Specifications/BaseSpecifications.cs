using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Specifications
{
    public class BaseSpecifications<T>(Expression<Func<T, bool>> criteria) : ISpecification<T> where T : class
    {
        public BaseSpecifications() : this(null) { }
        public Expression<Func<T, bool>>? Criteria => criteria;

        public IList<Expression<Func<T, object>>>? Include { get; private set; } = [];

        public IList<string>? ThenInclude { get; private set; } = [];

        public Expression<Func<T, object>>? OrderBy { get; private set; }

        public Expression<Func<T, object>>? OrderByDesc { get; private set; }

        public bool? Distinct { get; private set; }

        public bool HasPagination { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public IQueryable<T> AddCriteriaOnly(IQueryable<T> query)
        {
            if (Criteria!=null)
            query = query.Where(Criteria);
            return query;
        }
        public void AddInclude(Expression<Func<T, object>>include)
        {
            this.Include.Add(include);
        }
        public void AddDistinct()
        {
            Distinct = true;
        }
        public void AddOrderByDesc(Expression<Func<T, object>> orderByDesc)
        {
            this.OrderByDesc = orderByDesc;
        }
        public void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            this.OrderBy = orderBy;
        }
        public void AddThenInclude(string thenInclude)
        {
            this.ThenInclude.Add(thenInclude);
        }
        public void AddPagination(int take, int skip)
        {
            Take = take;
            Skip = skip;
            HasPagination = true;
        }
    }

    public class BaseSpecifications<T, TResult>(Expression<Func<T, bool>>? criteria) : BaseSpecifications<T>(criteria), ISpecification<T, TResult> where T : class
    {
        public BaseSpecifications() : this(null) { }
        public Expression<Func<T, TResult>>? Select { get; private set; }
        public void AddSelect(Expression<Func<T, TResult>> selectExpression)
        {
            this.Select = selectExpression;
        }

    }
}
