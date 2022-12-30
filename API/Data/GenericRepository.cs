using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Specifications;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {   
        private readonly DataContext _context;
        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec) 
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public async Task<bool> AddAsync(T entityToAdd) 
        {
            await _context.Set<T>().AddAsync(entityToAdd);

            return await _context.SaveChangesAsync() > 0;
        }

        public bool Delete(T entity)
        {
            var entityToDelete = _context.Set<T>().Find(entity.Id);
            
            if (entityToDelete != null)
            {
                _context.Set<T>().Remove(entityToDelete);
                _context.SaveChanges();
                return true;
            }   
            
            return false;
        }

        public void Update(T product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}