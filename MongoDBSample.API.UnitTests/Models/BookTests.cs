using FluentAssertions;
using MongoDBSample.Domain.Model.Books;

namespace MongoDBSample.API.UnitTests.Models
{
    public class BookTests
    {
        [Fact(DisplayName = "Instanciar objeto Book")]
        public void Instanciar_Book_ReturnsBook()
        {
            // Arrange
            string id = "1";
            string bookName = "Book Name";
            decimal price = 10.0m;
            string category = "Category";
            string author = "Author";
            DateTime date = DateTime.Now;

            // Act
            Book actual = new()
            {
                Id = id,
                BookName = bookName,
                Price = price,
                Category = category,
                Author = author,
                Date = date
            };

            // Assert
            actual.Should().NotBeNull();
            actual.Id.Should().Be(id);
            actual.BookName.Should().Be(bookName);
            actual.Price.Should().Be(price);
            actual.Category.Should().Be(category);
            actual.Author.Should().Be(author);
            actual.Date.Should().Be(date);
            actual.Author.Should().Be(author);
        }
    }
}