using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task AddItem(T entity);
        
        public void DeleteItem(T entity);
        public void UpdateItem(T entity);
        public Task<bool> SaveChangesAsync();

        public Task<IReadOnlyList<T>> ListAllAsync();

        public Task<T?> GetEntityWithspec(ISpecification<T> spec);
        public Task<TResult?> GetEntityWithspec<TResult>(ISpecification<T, TResult> spec);
        public Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        public IQueryable<T> GetQuery(ISpecification<T> spec);

        public Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);
        public IQueryable<TResult> GetQuery<TResult>(ISpecification<T,TResult> spec);
        Task<int> CountAsync(ISpecification<T> spec);


    }
}
