using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Books.Commands;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Domain.Model.UnitOfWork;

namespace MongoDBSample.Domain.Services.Books
{
    public class CadastrarBookService(
        IUnitOfWork unitOfWork)
                : CommandHandler<CadastrarBookCommand, CadastrarBookResponse>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        protected async override Task<Response<CadastrarBookResponse>> Execute(
            CadastrarBookCommand request,
            CancellationToken cancellationToken)
        {
            Book book = new()
            {
                BookName = request.BookName,
                Price = request.Price,
                Category = request.Category,
                Author = request.Author,
                Date = request.Date
            };

            // Use the generic AddAsync method here
            await unitOfWork.AddAsync(book);

            StatusCommit commitStatus = await unitOfWork.Commit(cancellationToken);

            if (commitStatus == StatusCommit.Sucesso)
            {
                return MapearResponse(true, "Book cadastrado com sucesso", book.Id);
            }

            return MapearResponse(false, "Falha ao cadastrar o book");
        }

        private Response<CadastrarBookResponse> MapearResponse(
            bool sucesso,
            string mensagem,
            string? id = null)
        {
            CadastrarBookResponse response = new()
            {
                Id = id,
                Sucesso = sucesso,
                Mensagem = mensagem
            };

            return Ok(response);
        }
    }
}
