using FluentAssertions;
using FluentValidation.Results;
using MongoDBSample.Application.Books.Queries;
using MongoDBSample.Application.Books.Validator;

namespace MongoDBSample.API.UnitTests.Application.Books.Validator
{
    public class ListarBooksPorIdQueryValidatorTests
    {
        private readonly ListarBooksPorIdQueryValidator validator;

        public ListarBooksPorIdQueryValidatorTests()
        {
            validator = new();
        }

        [Fact(DisplayName = "Id is required")]
        public void ListarBooksPorIdQueryValidator_IdRequired_ReturnsValidationResult()
        {
            //Arrange
            ListarBooksPorIdQuery query = new()
            {
                Id = ""
            };

            //Act
            ValidationResult result = validator.Validate(query);
            string actual = result.Errors[0].ErrorMessage;

            // Assert
            actual.Should().Contain("Id is required");
        }
    }
}
