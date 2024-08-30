using FluentAssertions;
using MongoDBSample.Application.Books.Queries;

namespace MongoDBSample.API.UnitTests.Application.Books.Queries
{
    public class ListarBooksPorIdQueryTests
    {
        [Fact(DisplayName = "Instanciar objeto ListarBooksPorIdQuery")]
        public void Instanciar_ListarBooksPorIdQuery_ReturnsListarBooksPorIdQuery()
        {
            // Arrange
            string id = "1";

            // Act
            ListarBooksPorIdQuery actual = new()
            {
                Id = id
            };

            // Assert
            actual.Id.Should().Be(id);
        }
    }
}