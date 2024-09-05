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
            int page = 1;
            int pageSize = 10;


            // Act
            ListarBooksQuery actual = new()
            {
                Id = id,
                Page = page,
                PageSize = pageSize
            };

            // Assert
            actual.Id.Should().Be(id);
            actual.Page.Should().Be(page);
            actual.PageSize.Should().Be(pageSize);
        }
    }
}