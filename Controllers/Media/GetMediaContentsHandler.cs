using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Ecommerce.Controllers.Media
{
    public record GetMediaContentsRequest : IRequest<IActionResult>
    {
        /// <summary>
        /// Filename.
        /// </summary>
        public string Filename { get; set; } = string.Empty;
    }

    public class GetMediaContentsHandler : IRequestHandler<GetMediaContentsRequest, IActionResult>
    {
        private readonly IFileRepository _fileRepository;
        private readonly ILogger<GetMediaContentsHandler> _logger;

        public GetMediaContentsHandler(IFileRepository fileRepository, ILogger<GetMediaContentsHandler> logger)
        {
            _fileRepository = fileRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(GetMediaContentsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                byte[] file = await _fileRepository.DownloadFileAsync(request.Filename)
                    ?? throw new NotFoundException("File not found");

                var fileExtension = new FileExtensionContentTypeProvider();
                if (!fileExtension.TryGetContentType(request.Filename, out string? contentType))
                {
                    contentType = "application/octet-stream";
                }

                return new FileContentResult(file, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting contents for file {filename}", request.Filename);
                throw;
            }
        }
    }
}
