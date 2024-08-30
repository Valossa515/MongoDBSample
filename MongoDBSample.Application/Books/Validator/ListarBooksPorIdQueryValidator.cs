using FluentValidation;
using MongoDBSample.Application.Books.Queries;

namespace MongoDBSample.Application.Books.Validator
{
    public class ListarBooksPorIdQueryValidator
        : AbstractValidator<ListarBooksPorIdQuery>
    {
        public ListarBooksPorIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }
}