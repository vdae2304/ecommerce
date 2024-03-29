﻿using FluentValidation;

namespace Ecommerce.Controllers.Categories.UploadImage
{
    public class UploadImageValidator : AbstractValidator<UploadImageRequest>
    {
        public UploadImageValidator()
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
