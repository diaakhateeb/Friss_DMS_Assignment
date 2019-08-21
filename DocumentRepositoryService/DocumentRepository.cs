using DocumentRepositoryService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryService
{
    /// <summary>
    /// Class of Document repository to operate over Document entity.
    /// </summary>
    /// <typeparam name="TEntity">Document entity.</typeparam>
    public class DocumentRepository<TEntity> : IDocumentRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _docDbContext;

        /// <summary>
        /// Creates DocumentRepository instance.
        /// </summary>
        /// <param name="docDbContext">Database context object.</param>
        public DocumentRepository(DbContext docDbContext)
        {
            _docDbContext = docDbContext;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _docDbContext.Set<TEntity>().ToList();
        }

        public TEntity Find(int id)
        {
            return _docDbContext.Set<TEntity>().Find(id);
        }

        public void Delete(TEntity entity)
        {
            _docDbContext.Set<TEntity>().Remove(entity);
        }

        public void Dispose()
        {
            _docDbContext?.Dispose();
        }

        public int SaveChanges()
        {
            return _docDbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _docDbContext.SaveChangesAsync();
        }

        public IDbContextTransaction DbContextTransaction => _docDbContext.Database.BeginTransaction();
    }
}
