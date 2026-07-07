using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> (ApplicationDbContext _context): IGenericRepository<T> where T : class
    {
        public async Task AddItem(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
        }

        public  void DeleteItem(T entity)
        {
            _context.Set<T>().Remove(entity);
        }



        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
          return await _context.Set<T>().ToListAsync();
        }

        public  void UpdateItem(T entity)
        {
             _context.Set<T>().Update(entity);
        }



        public async Task<T?> GetEntityWithspec(ISpecification<T> spec)
        {
            return await Apply(spec).FirstOrDefaultAsync();
        }
        public async Task<TResult?> GetEntityWithspec<TResult>(ISpecification<T, TResult> spec)
        {
            return await Apply(spec).FirstOrDefaultAsync();
        }


        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await Apply(spec).ToListAsync();
        }
        public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
        {
            return await Apply(spec).ToListAsync();

        }


        public  IQueryable<T> GetQuery(ISpecification<T> spec)
        {
           return  Apply(spec);
        }

        public IQueryable<TResult> GetQuery<TResult>(ISpecification<T, TResult> spec)
        {
            return Apply(spec);
        }

        private IQueryable<T>  Apply( ISpecification<T> spec)
        {
          return  SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }
        private IQueryable<TResult> Apply<TResult>(ISpecification<T,TResult> spec) 
        {
            return SpecificationEvaluator<T>.GetQuery<TResult>(_context.Set<T>().AsQueryable(), spec);

        }

        public async Task<bool> SaveChangesAsync()
        {
             return  await _context.SaveChangesAsync()>0;
            
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var query = spec.AddCriteriaOnly(_context.Set<T>().AsQueryable());
          return  await query.CountAsync();  
        }
    }
}
