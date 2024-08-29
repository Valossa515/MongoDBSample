using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;

namespace MongoDBSample.Application.Abstractions.Data
{
    public class Error
    {
        public Error()
        {
        }

        public Error(ValidationException validationException)
        {
            Property = validationException.ValidationResult.MemberNames?.FirstOrDefault();
            Message = validationException.ValidationResult.ErrorMessage;
        }

        public Error(ValidationFailure validationFailure)
        {
            Property = validationFailure.PropertyName;
            Message = validationFailure.ErrorMessage;
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