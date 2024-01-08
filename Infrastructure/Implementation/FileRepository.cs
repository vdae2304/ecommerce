using Ecommerce.Common.Interfaces;

namespace Ecommerce.Infrastructure.Implementation
{
    /// <inheritdoc/>
    public class FileRepository : IFileRepository
    {
        private readonly string _path;

        public FileRepository(IConfiguration config)
        {
            _path = config["FileStorage:Path"] ?? string.Empty;
        }

        /// <inheritdoc/>
        public string GetFileUrl(string filename)
        {
            return Path.Combine(_path, filename);
        }

        /// <inheritdoc/>
        public byte[]? DownloadFile(string filename)
        {
            string path = GetFileUrl(filename);
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            return null;
        }

        /// <inheritdoc/>
        public async Task<byte[]?> DownloadFileAsync(string filename)
        {
            string path = GetFileUrl(filename);
            if (File.Exists(path))
            {
                return await File.ReadAllBytesAsync(path);
            }
            return null;
        }

        /// <inheritdoc/>
        public void UploadFile(Stream file, string filename)
        {
            var stream = new FileStream(GetFileUrl(filename), FileMode.Create);
            file.CopyTo(stream);
            stream.Close();
        }

        /// <inheritdoc/>
        public async Task UploadFileAsync(Stream file, string filename)
        {
            var stream = new FileStream(GetFileUrl(filename), FileMode.Create);
            await file.CopyToAsync(stream);
            stream.Close();
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
