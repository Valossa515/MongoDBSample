using FluentValidation;
using MongoDBSample.Application.Books.Commands;

namespace MongoDBSample.Application.Books.Validator
{
    public class CadastrarBookValidator
        : AbstractValidator<CadastrarBookCommand>
    {
        public CadastrarBookValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Author)
                .NotEmpty()
                .WithMessage("Author is required")
                .MaximumLength(50)
                .WithMessage("Author MaximumLength is 50 characters");

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Category is required")
                .MaximumLength(50)
                .WithMessage("Category MaximumLength is 50 characters");

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Price is required")
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor(x => x.Date)
                 .NotEmpty()
                 .WithMessage("Date is required")
                 .Must(date => date >= DateTime.Now)
                 .WithMessage("Date cannot be in the past");
        }
    }
}