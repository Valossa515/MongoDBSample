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
                .MaximumLength(50)
                .WithMessage("Author is required and with maximum of 50 characters");

            RuleFor(x => x.Category)
                .NotEmpty()
                .MaximumLength(200)
                .WithMessage("Category is required and with maximum of 200 characters");

            RuleFor(x => x.Price)
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