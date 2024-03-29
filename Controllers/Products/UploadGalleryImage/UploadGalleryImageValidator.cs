﻿using FluentValidation;

namespace Ecommerce.Controllers.Products.UploadGalleryImage
{
    public class UploadGalleryImageValidator : AbstractValidator<UploadGalleryImageRequest>
    {
        public UploadGalleryImageValidator()
        {
            RuleFor(x => x.ImageFile).SetValidator(new ImageValidator());
        }
    }

    public class ImageValidator : AbstractValidator<IFormFile>
    {
        public ImageValidator()
        {
            RuleFor(x => x.ContentType)
                .Matches(@"^image/(png|jpe?g)$").WithMessage("Unsupported file format");

            RuleFor(x => x.Length)
                .LessThanOrEqualTo(10*1000*1000).WithMessage("Maximum supported size is {ComparisonValue} bytes");
        }
    }
}
