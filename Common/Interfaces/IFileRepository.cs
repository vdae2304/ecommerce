using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Common.Interfaces
{
    /// <summary>
    /// File repository.
    /// </summary>
    public interface IFileRepository
    {
        /// <summary>
        /// Return the public URL of a file.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <returns></returns>
        public string GetFileUrl(string filename);

        /// <summary>
        /// Download a file from the server.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <returns>File contents.</returns>
        public byte[]? DownloadFile(string filename);

        /// <summary>
        /// Asynchronously download a file from the server.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <returns>File contents.</returns>
        public Task<byte[]?> DownloadFileAsync(string filename);

        /// <summary>
        /// Upload a file to the server.
        /// </summary>
        /// <param name="file">File contents to upload.</param>
        /// <param name="filename">Filename to save as.</param>
        public void UploadFile(Stream file, string filename);

        /// <summary>
        /// Asynchronously upload a file to the server.
        /// </summary>
        /// <param name="file">File contents to upload.</param>
        /// <param name="filename">Filename to save as.</param>
        public Task UploadFileAsync(Stream file, string filename);

        /// <summary>
        /// Delete a file from the server.
        /// </summary>
        /// <param name="filename">Filename.</param>
        public void DeleteFile(string filename);

        /// <summary>
        /// Asynchronously delete a file from the server.
        /// </summary>
        /// <param name="filename">Filename.</param>
        public Task DeleteFileAsync(string filename);
    }
}
