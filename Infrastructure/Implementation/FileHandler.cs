using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;

namespace Ecommerce.Infrastructure.Implementation
{
    /// <inheritdoc/>
    public class FileHandler : IFileHandler
    {
        private readonly string _rootPath;

        public FileHandler(IConfiguration config)
        {
            _rootPath = config["FileOptions:RootPath"] ?? string.Empty;
        }

        /// <inheritdoc/>
        public string GetFileUrl(string fileId)
        {
            return Path.Combine(_rootPath, fileId);
        }

        /// <inheritdoc/>
        public string UploadFile(IFormFile file)
        {
            try
            {
                string fileId = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var stream = new FileStream(GetFileUrl(fileId), FileMode.Create);
                file.CopyTo(stream);
                stream.Close();
                return fileId;
            }
            catch (Exception ex)
            {
                throw new FileHandlerException(ex.Message);
            }
        }

        /// <inheritdoc/>
        public void DeleteFile(string fileId)
        {
            try
            {
                File.Delete(GetFileUrl(fileId));
            }
            catch (Exception ex)
            {
                throw new FileHandlerException(ex.Message);
            }
        }
    }
}
