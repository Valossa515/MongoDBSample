using FluentAssertions;
using MongoDBSample.Application.Books.Data.Payloads;

namespace MongoDBSample.API.UnitTests.Application.Books.Data.Payloads
{
    public class BookPayloadTests
    {
        [Fact(DisplayName = "Instanciar objeto BookPayloadTests")]
        public void Instanciar_BookPayloadTests_ReturnsBookPayloadTests()
        {
            // Arrange
            string bo0okName = "BookName";
            string author = "Author";
            string category = "Category";
            decimal price = 10.0m;
            DateTime date = DateTime.Now;

            // Act
            BookPayload actual = new()
            {
                BookName = bo0okName,
                Author = author,
                Category = category,
                Price = price,
                Date = date
            };

            // Assert
            actual.Author.Should().Be(author);
            actual.BookName.Should().Be(bo0okName);
            actual.Category.Should().Be(category);
            actual.Price.Should().Be(price);
            actual.Date.Should().Be(date);
        }
    }
}