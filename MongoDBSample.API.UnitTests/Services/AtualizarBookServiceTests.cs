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
    public class AtualizarBookServiceTests
    {
        [Fact(DisplayName = "Deve atualizar book com sucesso")]
        public async Task DeveAtualizarBookComSucesso()
        {
            // Arrange
            AtualizarBookCommand request = new()
            {
                Id = "1",
                Name = "Book 1",
                Price = 10,
                Category = "Category 1",
                Author = "Author 1",
                Date = DateTime.Now
            };

            Book expected = new()
            {
                Id = "2",
                BookName = "Book 2",
                Price = 10,
                Category = "Category 2",
                Author = "Author 2",
                Date = DateTime.Now
            };

            AtualizarBookServiceBuilder serviceBuilder = new();

            AtualizarBookService atualizarBookService = serviceBuilder
                .FindByIdAsync(expected)
                .SetupCommit(StatusCommit.Sucesso)
                .Build();

            // Act
            Response<CadastrarBookResponse> response = await atualizarBookService.PublicExecute(request, CancellationToken.None);
            Mock<IUnitOfWork> unitOfWork = serviceBuilder.GetUnitOfWork();

            // Assert
            string messageExpected = "Book atualizado com sucesso";
            response.Result.Mensagem.Should().BeEquivalentTo(messageExpected);
            response.Result.Sucesso.Should().BeTrue();
            unitOfWork.Verify(u => u.UpdateAsync(It.IsAny<Book>()), Times.Once);
            unitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact(DisplayName = "Deve retornar mensagem de erro ao tentar atualizar book inexistente")]
        public async Task DeveRetornarMensagemDeErroAoTentarAtualizarBookInexistente()
        {
            // Arrange
            AtualizarBookCommand request = new()
            {
                Id = "1",
                Name = "Book 1",
                Price = 10,
                Category = "Category 1",
                Author = "Author 1",
                Date = DateTime.Now
            };

            AtualizarBookServiceBuilder serviceBuilder = new();

            AtualizarBookService atualizarBookService = serviceBuilder
                .FindByIdAsync(null)
                .SetupCommit(StatusCommit.Falha)
                .Build();

            // Act
            Response<CadastrarBookResponse> response = await atualizarBookService.PublicExecute(request, CancellationToken.None);
            Mock<IUnitOfWork> unitOfWork = serviceBuilder.GetUnitOfWork();

            // Assert
            string messageExpected = "Book não encontrado";
            response.Result.Mensagem.Should().BeEquivalentTo(messageExpected);
            response.Result.Sucesso.Should().BeFalse();
            unitOfWork.Verify(u => u.UpdateAsync(It.IsAny<Book>()), Times.Never);
            unitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Deve retornar mensagem de erro ao tentar atualizar book com falha no commit")]
        public async Task DeveRetornarMensagemDeErroAoTentarAtualizarBookComFalhaNoCommit()
        {
            // Arrange
            AtualizarBookCommand request = new()
            {
                Id = "1",
                Name = "Book 1",
                Price = 10,
                Category = "Category 1",
                Author = "Author 1",
                Date = DateTime.Now
            };

            Book expected = new()
            {
                Id = "2",
                BookName = "Book 2",
                Price = 10,
                Category = "Category 2",
                Author = "Author 2",
                Date = DateTime.Now
            };

            AtualizarBookServiceBuilder serviceBuilder = new();

            AtualizarBookService atualizarBookService = serviceBuilder
                .FindByIdAsync(expected)
                .SetupCommit(StatusCommit.Falha)
                .Build();

            // Act
            Response<CadastrarBookResponse> response = await atualizarBookService.PublicExecute(request, CancellationToken.None);
            Mock<IUnitOfWork> unitOfWork = serviceBuilder.GetUnitOfWork();

            // Assert
            string messageExpected = "Falha ao atualizar o book";
            response.Result.Mensagem.Should().BeEquivalentTo(messageExpected);
            response.Result.Sucesso.Should().BeFalse();
            unitOfWork.Verify(u => u.UpdateAsync(It.IsAny<Book>()), Times.Once);
            unitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}