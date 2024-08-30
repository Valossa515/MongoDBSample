using MongoDBSample.Domain.Model.UnitOfWork;
using MongoDBSample.Domain.Services.Books;
using Moq;
using Moq.AutoMock;

namespace MongoDBSample.API.UnitTests.Builders.Services
{
    internal class CadastrarBookServiceBuilder
    {
        private readonly AutoMocker autoMocker;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly CadastrarBookService service;

        public CadastrarBookServiceBuilder()
        {
            autoMocker = new();
            unitOfWork = autoMocker.GetMock<IUnitOfWork>();
            service = autoMocker.CreateInstance<CadastrarBookService>();
        }

        public CadastrarBookServiceBuilder SetupCommit(StatusCommit commitStatus)
        {
            unitOfWork.Setup(u => u.Commit(It.IsAny<CancellationToken>()))
                .ReturnsAsync(commitStatus);

            return this;
        }

        public Mock<IUnitOfWork> GetUnitOfWork()
        {
            return unitOfWork;
        }

        public CadastrarBookService Build()
        {
            return service;
        }
    }
}