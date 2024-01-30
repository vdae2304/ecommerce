namespace Ecommerce.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public string[] Errors { get; set; }

        public BadRequestException()
        {
            Errors = Array.Empty<string>();
        }

        public BadRequestException(params string[] errors)
        {
            Errors = errors;
        }

        public BadRequestException(IEnumerable<string> errors)
        {
            Errors = errors.ToArray();
        }
    }
}
