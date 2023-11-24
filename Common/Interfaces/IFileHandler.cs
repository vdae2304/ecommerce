namespace Ecommerce.Common.Interfaces
{
    /// <summary>
    /// File handler.
    /// </summary>
    public interface IFileHandler
    {
        /// <summary>
        /// Return the public URL from a file ID.
        /// </summary>
        /// <param name="fileId">File ID.</param>
        /// <returns></returns>
        public string GetFileUrl(string fileId);

        /// <summary>
        /// Upload a file to the server.
        /// </summary>
        /// <param name="file">File to upload.</param>
        /// <returns>File ID.</returns>
        public string UploadFile(IFormFile file);

        /// <summary>
        /// Delete a file from the server.
        /// </summary>
        /// <param name="fileId">File ID.</param>
        public void DeleteFile(string fileId);
    }
}
