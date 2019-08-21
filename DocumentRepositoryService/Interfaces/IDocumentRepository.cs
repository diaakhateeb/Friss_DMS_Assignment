using System;
using System.Collections.Generic;

namespace DocumentRepositoryService.Interfaces
{
    /// <summary>
    /// Interface of Document repository to operate over Document entity.
    /// </summary>
    /// <typeparam name="TEntity">Document entity.</typeparam>
    public interface IDocumentRepository<TEntity> : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Retrieves all stored Documents ordered by Last Access Date descending.
        /// </summary>
        /// <returns>Collection of stored Documents.</returns>
        IEnumerable<TEntity> GetAll();
        /// <summary>
        /// Finds specific Document.
        /// </summary>
        /// <param name="id">Document Id.</param>
        /// <returns>Returns Document object.</returns>
        TEntity Find(int id);
        /// <summary>
        /// Deletes Document.
        /// </summary>
        /// <param name="entity">Document to delete.</param>
        void Delete(TEntity entity);
    }
}
