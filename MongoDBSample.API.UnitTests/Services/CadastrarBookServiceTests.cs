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
    public class CadastrarBookServiceTests
    {
        [Fact(DisplayName = "Deve cadastrar um book com sucesso")]
        public async Task DeveCadastrarBookComSucesso()
        {
            // Arrange
            CadastrarBookCommand command = new()
            {
                BookName = "Book 1",
                Price = 10.0m,
                Category = "Category 1",
                Author = "Author 1",
                Date = DateTime.Now
            };

            CadastrarBookServiceBuilder serviceBuilder = new();

            CadastrarBookService service = serviceBuilder
                .SetupCommit(StatusCommit.Sucesso)
                .Build();

            // Act
            Response<CadastrarBookResponse> actual = await service.PublicExecute(command, CancellationToken.None);
            Mock<IUnitOfWork> unitOfWork = serviceBuilder.GetUnitOfWork();

            // Assert
            string messageExpected = "Book cadastrado com sucesso";
            actual.Result.Mensagem.Should().BeEquivalentTo(messageExpected);
            unitOfWork.Verify(u => u.AddAsync(It.IsAny<Book>()), Times.Once);
            unitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve falhar ao cadastrar um book")]
        public async Task DeveFalharAoCadastrarBook()
        {
            // Arrange
            CadastrarBookCommand command = new()
            {
                BookName = "Book 1",
                Price = 10.0m,
                Category = "Category 1",
                Author = "Author 1",
                Date = DateTime.Now
            };

            CadastrarBookServiceBuilder serviceBuilder = new();

            CadastrarBookService service = serviceBuilder
                .SetupCommit(StatusCommit.Falha)
                .Build();

            // Act
            Response<CadastrarBookResponse> actual = await service.PublicExecute(command, CancellationToken.None);
            Mock<IUnitOfWork> unitOfWork = serviceBuilder.GetUnitOfWork();

            // Assert
            string messageExpected = "Falha ao cadastrar o book";
            actual.Result.Mensagem.Should().BeEquivalentTo(messageExpected);
            unitOfWork.Verify(u => u.AddAsync(It.IsAny<Book>()), Times.Once);
            unitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}