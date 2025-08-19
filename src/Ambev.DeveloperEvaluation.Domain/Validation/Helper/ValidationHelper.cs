using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Domain.Validation.Helper
{
    public static class ValidationHelper
    {
        public static IEnumerable<ValidationFailure> GetValidationFailures(this ValidationResultDetail validationResult) => 
            validationResult.Errors.Select(e => new ValidationFailure(nameof(Sale), e.Error));
    }
}
