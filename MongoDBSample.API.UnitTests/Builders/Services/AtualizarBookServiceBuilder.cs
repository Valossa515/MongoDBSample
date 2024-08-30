using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Domain.Model.UnitOfWork;
using MongoDBSample.Domain.Services.Books;
using Moq;
using Moq.AutoMock;

namespace MongoDBSample.API.UnitTests.Builders.Services
{
    internal class AtualizarBookServiceBuilder
    {
        private readonly AutoMocker autoMocker;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly AtualizarBookService service;

        public AtualizarBookServiceBuilder()
        {
            autoMocker = new();
            unitOfWork = autoMocker.GetMock<IUnitOfWork>();
            service = autoMocker.CreateInstance<AtualizarBookService>();
        }

        public AtualizarBookServiceBuilder FindByIdAsync(Book? book)
        {
            unitOfWork.Setup(u => u.FindByIdAsync<Book>(It.IsAny<string>()))
                .ReturnsAsync(book);

            return this;
        }

        public AtualizarBookServiceBuilder SetupCommit(StatusCommit commitStatus)
        {
            unitOfWork.Setup(u => u.Commit(It.IsAny<CancellationToken>()))
                .ReturnsAsync(commitStatus);

            return this;
        }



        public Mock<IUnitOfWork> GetUnitOfWork()
        {
            return unitOfWork;
        }

        public AtualizarBookService Build()
        {
            return service;
        }
    }
}