namespace Ambev.DeveloperEvaluation.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public IEnumerable<string>? Errors { get; }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(IEnumerable<string> errors)
            : base("Business validation failed.")
        {
            Errors = errors;
        }
    }
}
