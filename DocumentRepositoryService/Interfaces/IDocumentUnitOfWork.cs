using System.IO;

namespace DocumentRepositoryService.Interfaces
{
    /// <summary>
    /// Interface to save upload and download files data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDocumentUnitOfWork<out T>
    {
        /// <summary>
        /// Saves uploaded file information.
        /// </summary>
        /// <param name="file">file entity.</param>
        /// <param name="userId">User Id whom uploading the file.</param>
        /// <returns>Returns uploaded file object.</returns>
        T SaveUpload(FileInfo file, string userId);

        /// <summary>
        /// Updates file data Last Access Date and Counter values.
        /// </summary>
        /// <param name="id">file Id.</param>
        /// <returns>Returns file object with new data.</returns>
        T SaveDownload(int id);
    }
}
