using FluentAssertions;
using FluentValidation.Results;
using MongoDBSample.Application.Books.Commands;
using MongoDBSample.Application.Books.Validator;

namespace MongoDBSample.API.UnitTests.Application.Books.Validator
{
    public class CadastrarBookValidatorTests
    {
        private readonly CadastrarBookValidator validator;

        public CadastrarBookValidatorTests()
        {
            validator = new();
        }

        [Fact(DisplayName = "O nome do autor é obrigatorio")]
        public void CadastrarBookValidator_AuthorObrigatorio_ReturnsValidationResult()
        {
            //Arrange
            CadastrarBookCommand command = new()
            {
                Author = "",
                Category = "Category",
                Price = 10,
                Date = DateTime.Now
            };

            //Act
            ValidationResult result = validator.Validate(command);
            string actual = result.Errors[0].ErrorMessage;


            // Assert
            actual.Should().Contain("Author is required");
        }

        [Fact(DisplayName = "O nome do autor deve ter no máximo 50 caracteres")]
        public void CadastrarBookValidator_AuthorMaximo50Caracteres_ReturnsValidationResult()
        {
            //Arrange
            CadastrarBookCommand command = new()
            {
                Author = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!",
                Category = "Category",
                Price = 10,
                Date = DateTime.Now
            };

            //Act
            ValidationResult result = validator.Validate(command);
            string actual = result.Errors[0].ErrorMessage;

            // Assert
            actual.Should().Contain("Author MaximumLength is 50 characters");
        }

        [Fact(DisplayName = "A categoria é obrigatória")]
        public void CadastrarBookValidator_CategoryObrigatorio_ReturnsValidationResult()
        {
            //Arrange
            CadastrarBookCommand command = new()
            {
                Author = "Author",
                Category = "",
                Price = 10,
                Date = DateTime.Now
            };

            //Act
            ValidationResult result = validator.Validate(command);
            string actual = result.Errors[0].ErrorMessage;

            // Assert
            actual.Should().Contain("Category is required");
        }

        [Fact(DisplayName = "A categoria deve ter no máximo 50 caracteres")]
        public void CadastrarBookValidator_CategoryMaximo50Caracteres_ReturnsValidationResult()
        {
            //Arrange
            CadastrarBookCommand command = new()
            {
                Author = "Author",
                Category = "",
                Price = 10,
                Date = DateTime.Now
            };
        }

        [Fact(DisplayName = "O preço é obrigatório")]
        public void CadastrarBookValidator_PriceObrigatorio_ReturnsValidationResult()
        {
            //Arrange
            CadastrarBookCommand command = new()
            {
                Author = "Author",
                Category = "Category",
                Price = 0,
                Date = DateTime.Now
            };

            //Act
            ValidationResult result = validator.Validate(command);
            string actual = result.Errors[0].ErrorMessage;

            // Assert
            actual.Should().Contain("Price is required");
        }

        [Fact(DisplayName = "A data é obrigatória")]
        public void CadastrarBookValidator_DateObrigatorio_ReturnsValidationResult()
        {
            //Arrange
            CadastrarBookCommand command = new()
            {
                Author = "Author",
                Category = "Category",
                Price = 10,
                Date = DateTime.MinValue
            };

            //Act
            ValidationResult result = validator.Validate(command);
            string actual = result.Errors[0].ErrorMessage;

            // Assert
            actual.Should().Contain("Date is required");
        }

        [Fact(DisplayName = "A data não pode ser no passado")]
        public void CadastrarBookValidator_DateNaoPodeSerPassado_ReturnsValidationResult()
        {
            //Arrange
            CadastrarBookCommand command = new()
            {
                Author = "Author",
                Category = "Category",
                Price = 10,
                Date = DateTime.Now.AddDays(-1)
            };

            //Act
            ValidationResult result = validator.Validate(command);
            string actual = result.Errors[0].ErrorMessage;

            // Assert
            actual.Should().Contain("Date cannot be in the past");
        }
    }
}