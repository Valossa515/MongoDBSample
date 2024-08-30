using FluentAssertions;
using MongoDBSample.Application.Books.Data;

namespace MongoDBSample.API.UnitTests.Application.Books.Data
{
    public class BookResponseTests
    {
        [Fact(DisplayName = "Instanciar objeto BookResponse")]
        public void Instanciar_BookResponse_ReturnsBookResponse()
        {
            //Arrange
            string id = "1";
            string name = "Book 1";
            string author = "Author 1";
            string category = "Category 1";
            decimal price = 10.0m;
            DateTime date = DateTime.Now;

            //Act
            BookResponse actual = new()
            {
                Id = id,
                Name = name,
                Author = author,
                Category = category,
                Price = price,
                Date = date
            };

            //Assert
            actual.Id.Should().Be(id);
            actual.Name.Should().Be(name);
            actual.Author.Should().Be(author);
            actual.Category.Should().Be(category);
            actual.Price.Should().Be(price);
            actual.Date.Should().Be(date);

        }
    }
}