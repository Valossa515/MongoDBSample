using FluentAssertions;
using MongoDBSample.Application.Books.Commands;

namespace MongoDBSample.API.UnitTests.Application.Books.Commands
{
    public class RemoverBookCommandTests
    {
        [Fact(DisplayName = "Instanciar objeto RemoverBookCommand")]
        public void Instanciar_RemoverBookCommand_ReturnsRemoverBookCommand()
        {
            // Arrange
            string id = "1";

            // Act
            RemoverBookCommand actual = new()
            {
                Id = id
            };

            // Assert
            actual.Should().NotBeNull();
            actual.Id.Should().Be(id);
        }
    }
}
