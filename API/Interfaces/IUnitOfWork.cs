using System;
using System.Threading.Tasks;
using API.Models;

namespace API.Interfaces
{
      public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete();
    }
}