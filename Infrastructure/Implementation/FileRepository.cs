using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;

namespace Ecommerce.Infrastructure.Implementation
{
    /// <inheritdoc/>
    public class FileRepository : IFileRepository
    {
        private readonly string _rootPath;

        public FileRepository(IConfiguration config)
        {
            _rootPath = config["FileOptions:RootPath"] ?? string.Empty;
        }

        /// <inheritdoc/>
        public string GetFileUrl(string filename)
        {
            return Path.Combine(_rootPath, filename);
        }

        /// <inheritdoc/>
        public void UploadFile(Stream file, string filename)
        {
            var stream = new FileStream(GetFileUrl(filename), FileMode.Create);
            file.CopyTo(stream);
            stream.Close();
        }

        /// <inheritdoc/>
        public Task UploadFileAsync(Stream file, string filename)
        {
            UploadFile(file, filename);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void DeleteFile(string filename)
        {
            File.Delete(GetFileUrl(filename));
        }

        /// <inheritdoc/>
        public Task DeleteFileAsync(string filename)
        {
            DeleteFile(filename);
            return Task.CompletedTask;
        }
    }
}
