using DataModel;
using DocumentRepositoryService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace DocumentRepositoryService
{
    /// <summary>
    /// Class to save upload and download files data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DocumentUnitOfWork<T> : IDocumentUnitOfWork<T> where T : Document
    {
        private readonly DbContext _docDbContext;
        /// <summary>
        /// Creates DocumentHelper instance.
        /// </summary>
        /// <param name="docDbContext">Database context object.</param>
        public DocumentUnitOfWork(DbContext docDbContext)
        {
            _docDbContext = docDbContext;
        }

        public T SaveUpload(FileInfo file, string userId)
        {
            var doc = new Document
            {
                Name = file.Name,
                Path = file.FullName,
                UploadDate = DateTime.Now,
                Size = file.Length,
                Extension = file.Extension,
                UserId = userId
            };

            var newDoc = _docDbContext.Set<T>().Add((T)doc).Entity;

            _docDbContext.SaveChanges();
            return newDoc;
        }

        public T SaveDownload(int id)
        {
            var doc = _docDbContext.Set<T>().Find(id);

            _docDbContext.Entry(doc).State = EntityState.Modified;
            doc.LastAccessDate = DateTime.Now;
            doc.DownloadCounter += 1;
            _docDbContext.Update(doc);
            _docDbContext.SaveChanges();

            return doc;

        }
    }
}
