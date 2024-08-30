using FluentAssertions;
using MongoDBSample.Application.Books.Data;

namespace MongoDBSample.API.UnitTests.Application.Books.Data
{
    public class CadastrarBookResponseTests
    {
        [Fact(DisplayName = "Instanciar objeto CadastrarBookResponse")]
        public void Instanciar_CadastrarBookResponse_ReturnsCadastrarBookResponse()
        {
            // Arrange
            string id = "1";
            bool sucesso = true;
            string mensagem = "Mensagem";

            // Act
            CadastrarBookResponse actual = new()
            {
                Id = id,
                Sucesso = sucesso,
                Mensagem = mensagem
            };

            // Assert
            actual.Mensagem.Should().Be(mensagem);
            actual.Id.Should().Be(id);
            actual.Sucesso.Should().Be(sucesso);
        }
    }
}