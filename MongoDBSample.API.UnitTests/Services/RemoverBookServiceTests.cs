using FluentAssertions;
using MongoDBSample.API.UnitTests.Builders.Services;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Books.Commands;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Domain.Model.UnitOfWork;
using MongoDBSample.Domain.Services.Books;
using Moq;

namespace MongoDBSample.API.UnitTests.Services
{
    public class RemoverBookServiceTests
    {
        [Fact(DisplayName = "Deve remover um livro encontrado")]
        public async Task DeveRemoverLivroEncontrado()
        {
            // Arrange
            RemoverBookCommand request = new()
            {
                Id = "1"
            };

            Book expected = new()
            {
                Id = "1",
                BookName = "Book 1",
                Price = 10,
                Category = "Category 1",
                Author = "Author 1",
                Date = DateTime.Now
            };

            RemoverBookServiceBuilder serviceBuilder = new();

            RemoverBookService removerBookService = serviceBuilder
                .FindByIdAsync(expected)
                .SetupCommit(StatusCommit.Sucesso)
                .Build();

            // Act
            Response<CadastrarBookResponse> response = await removerBookService.PublicExecute(request, CancellationToken.None);
            Mock<IUnitOfWork> unitOfWork = serviceBuilder.GetUnitOfWork();

            // Assert
            string messageExpected = "Book removido com sucesso";
            response.Result.Mensagem.Should().BeEquivalentTo(messageExpected);
            response.Result.Sucesso.Should().BeTrue();
            unitOfWork.Verify(u => u.Remove(It.IsAny<Book>()), Times.Once);
            unitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar mensagem de erro ao tentar remover um livro inexistente")]
        public async Task DeveRetornarMensagemDeErroAoTentarRemoverLivroInexistente()
        {
            // Arrange
            RemoverBookCommand request = new()
            {
                Id = "1"
            };

            RemoverBookServiceBuilder serviceBuilder = new();

            RemoverBookService removerBookService = serviceBuilder
                .FindByIdAsync(null)
                .Build();

            // Act
            Response<CadastrarBookResponse> response = await removerBookService.PublicExecute(request, CancellationToken.None);
            Mock<IUnitOfWork> unitOfWork = serviceBuilder.GetUnitOfWork();

            // Assert
            string messageExpected = "Book não encontrado";
            response.Result.Mensagem.Should().BeEquivalentTo(messageExpected);
            response.Result.Sucesso.Should().BeFalse();
            unitOfWork.Verify(u => u.Remove(It.IsAny<Book>()), Times.Never);
            unitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Deve retornar mensagem de erro ao falhar ao remover um livro")]
        public async Task DeveRetornarMensagemDeErroAoFalharAoRemoverLivro()
        {
            // Arrange
            RemoverBookCommand request = new()
            {
                Id = "1"
            };

            Book expected = new()
            {
                Id = "2",
                BookName = "Book 1",
                Price = 10,
                Category = "Category 1",
                Author = "Author 1",
                Date = DateTime.Now
            };

            RemoverBookServiceBuilder serviceBuilder = new();

            RemoverBookService removerBookService = serviceBuilder
                .FindByIdAsync(expected)
                .SetupCommit(StatusCommit.Falha)
                .Build();

            // Act
            Response<CadastrarBookResponse> response = await removerBookService.PublicExecute(request, CancellationToken.None);
            Mock<IUnitOfWork> unitOfWork = serviceBuilder.GetUnitOfWork();

            // Assert
            string messageExpected = "Falha ao remover o book";
            response.Result.Mensagem.Should().BeEquivalentTo(messageExpected);
            response.Result.Sucesso.Should().BeFalse();
            unitOfWork.Verify(u => u.Remove(It.IsAny<Book>()), Times.Once);
            unitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}