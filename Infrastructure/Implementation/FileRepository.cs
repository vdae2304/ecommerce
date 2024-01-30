using Ecommerce.Common.Interfaces;

namespace Ecommerce.Infrastructure.Implementation
{
    /// <inheritdoc/>
    public class FileRepository : IFileRepository
    {
        private readonly string _directory;
        private readonly string _host;

        public FileRepository(IConfiguration configuration)
        {
            _directory = configuration["FileStorage:Directory"] ?? string.Empty;
            _host = configuration["FileStorage:Host"] ?? string.Empty;
        }

        /// <inheritdoc/>
        public string GetFileUrl(string filename)
        {
            return $"{_host}/{filename}";
        }

        /// <inheritdoc/>
        public byte[]? DownloadFile(string filename)
        {
            string path = Path.Combine(_directory, filename);
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }

        /// <inheritdoc/>
        public async Task<byte[]?> DownloadFileAsync(string filename)
        {
            string path = Path.Combine(_directory, filename);
            if (File.Exists(path))
            {
                return await File.ReadAllBytesAsync(path);
            }
            return null;
        }

        /// <inheritdoc/>
        public void UploadFile(Stream file, string filename)
        {
            string path = Path.Combine(_directory, filename);
            var stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);
            stream.Close();
        }

        /// <inheritdoc/>
        public async Task UploadFileAsync(Stream file, string filename)
        {
            string path = Path.Combine(_directory, filename);
            var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            stream.Close();
        }

        /// <inheritdoc/>
        public void DeleteFile(string filename)
        {
            string path = Path.Combine(_directory, filename);
            File.Delete(path);
        }

        /// <inheritdoc/>
        public Task DeleteFileAsync(string filename)
        {
            DeleteFile(filename);
            return Task.CompletedTask;
        }
    }
}
