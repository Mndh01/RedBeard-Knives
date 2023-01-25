using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data.Specifications;
using API.Models;

namespace API.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {   
        /// <summary> Gets any type of entity from the DB by it's id </summary>
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();          
        Task<T> GetEntityWithSpec(ISpecification<T> spec);          
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
