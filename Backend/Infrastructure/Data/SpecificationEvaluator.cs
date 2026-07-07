using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class SpecificationEvaluator<T>where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query,ISpecification<T> spec)
        {

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            if (spec.OrderBy!=null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }
            if (spec.Distinct == true)
            {
                query = query.Distinct();
            }
            if (spec.Include != null)
            {
                foreach (var item in spec.Include)
                    query = query.Include(item);
            }
            if (spec.ThenInclude != null)
            {
                foreach (var item in spec.ThenInclude)
                    query = query.Include(item);
            }
            if (spec.HasPagination == true)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }

        public static IQueryable<TResult> GetQuery<TResult>(IQueryable<T> query, ISpecification<T,TResult> spec) 
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.Include != null)
            {
                foreach (var item in spec.Include)
                    query = query.Include(item);
            }
            if (spec.ThenInclude != null)
            {
                foreach (var item in spec.ThenInclude)
                    query = query.Include(item);
            }
            if (spec.HasPagination == true)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

     var selectedQuery= query as IQueryable<TResult>;

            if (spec.Select != null)
            {
                selectedQuery = query.Select(spec.Select);
            }
            if (spec.Distinct == true)
            {
                if (selectedQuery != null)
                    selectedQuery = selectedQuery.Distinct();
                else query = query.Distinct();
            }
            return selectedQuery ?? query.Cast<TResult>();

        }

    }
}
