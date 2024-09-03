using FluentAssertions;
using MongoDBSample.Application.Books.Commands;

namespace MongoDBSample.API.UnitTests.Application.Books.Commands
{
    public class AtualizarBookCommandTests
    {
        [Fact(DisplayName = "Instnaciar objeto AtualizarBookCommand")]
        public void Instanciar_AtualizarBookCommand_ReturnsAtualizarBookCommand()
        {
            // Arrange
            string id = "1";
            string name = "Book 1";
            string author = "Author 1";
            string category = "Category 1";
            decimal price = 10.0m;
            DateTime date = DateTime.Now;

            // Act
            AtualizarBookCommand actual = new()
            {
                Id = id,
                BookName = name,
                Author = author,
                Category = category,
                Price = price,
                Date = date
            };

            // Assert
            actual.Id.Should().Be(id);
            actual.BookName.Should().Be(name);
            actual.Author.Should().Be(author);
            actual.Category.Should().Be(category);
            actual.Price.Should().Be(price);
            actual.Date.Should().Be(date);
        }
    }
}