using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;

namespace MongoDBSample.Application.Abstractions.Data
{
    public class Error
    {
        public Error()
        {
            Property = string.Empty;
            Message = string.Empty;
        }

        public Error(ValidationException validationException)
        {
            Property = validationException.ValidationResult.MemberNames?.FirstOrDefault() ?? string.Empty;
            Message = validationException.ValidationResult.ErrorMessage ?? string.Empty;
        }

        public Error(ValidationFailure validationFailure)
        {
            Property = validationFailure.PropertyName ?? string.Empty;
            Message = validationFailure.ErrorMessage ?? string.Empty;
        }

        public Error(string property, string message)
        {
            Property = property;
            Message = message;
        }

        public string Property { get; set; }

        public string Message { get; set; }
    }
}