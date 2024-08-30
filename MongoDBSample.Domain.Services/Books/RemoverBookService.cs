using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Books.Commands;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Domain.Model.UnitOfWork;

namespace MongoDBSample.Domain.Services.Books
{
    public class RemoverBookService(IUnitOfWork unitOfWork)
        : CommandHandler<RemoverBookCommand, CadastrarBookResponse>

    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Response<CadastrarBookResponse>> PublicExecute(
           RemoverBookCommand request,
           CancellationToken cancellationToken)
        {
            return await Execute(request, cancellationToken);
        }

        protected async override Task<Response<CadastrarBookResponse>> Execute(
            RemoverBookCommand request,
            CancellationToken cancellationToken)
        {
            string? bookId = request.Id;

            Book? book = await unitOfWork.FindByIdAsync<Book>(bookId);

            if (book == null)
            {
                return MapearResponse(false, "Book não encontrado");
            }

            unitOfWork.Remove(book);

            StatusCommit commitStatus = await unitOfWork.Commit(cancellationToken);

            if (commitStatus == StatusCommit.Sucesso)
            {
                return MapearResponse(true, "Book removido com sucesso");
            }

            return MapearResponse(false, "Falha ao remover o book");
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