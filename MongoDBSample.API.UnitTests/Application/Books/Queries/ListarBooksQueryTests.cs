using FluentAssertions;
using MongoDBSample.Application.Books.Queries;

namespace MongoDBSample.API.UnitTests.Application.Books.Queries
{
    public class ListarBooksQueryTests
    {
        [Fact(DisplayName = "Instanciar objeto ListarBooksQuery")]
        public void Instanciar_ListarBooksQuery_ReturnsListarBooksQuery()
        {
            // Arrange
            string id = "1";

            // Act
            ListarBooksQuery actual = new()
            {
                Id = id
            };

            // Assert
            actual.Id.Should().Be(id);
        }
    }
}