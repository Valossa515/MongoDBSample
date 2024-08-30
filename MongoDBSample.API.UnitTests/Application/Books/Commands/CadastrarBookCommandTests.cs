using FluentAssertions;
using MongoDBSample.Application.Books.Commands;

namespace MongoDBSample.API.UnitTests.Application.Books.Commands
{
    public class CadastrarBookCommandTests
    {
        [Fact(DisplayName = "Instanciar objeto CadastrarBookCommand")]
        public void Instanciar_CadastrarBookCommand_ReturnsCadastrarBookCommand()
        {
            // Arrange
            string bookName = "Book Name";
            string author = "Author";
            string category = "Category";
            decimal price = 10.0m;
            DateTime date = DateTime.Now;

            // Act
            CadastrarBookCommand actual = new()
            {
                BookName = bookName,
                Author = author,
                Category = category,
                Price = price,
                Date = date
            };

            // Assert
            actual.Author.Should().Be(author);
            actual.BookName.Should().Be(bookName);
            actual.Category.Should().Be(category);
            actual.Price.Should().Be(price);
            actual.Date.Should().Be(date);
        }
    }
}