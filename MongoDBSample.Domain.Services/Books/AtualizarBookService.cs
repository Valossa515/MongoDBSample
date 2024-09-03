using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Books.Commands;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Domain.Model.UnitOfWork;

namespace MongoDBSample.Domain.Services.Books
{
    public class AtualizarBookService(
        IUnitOfWork unitOfWork)
                : CommandHandler<AtualizarBookCommand, CadastrarBookResponse>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Response<CadastrarBookResponse>> PublicExecute(
           AtualizarBookCommand request,
           CancellationToken cancellationToken)
        {
            return await Execute(request, cancellationToken);
        }

        protected async override Task<Response<CadastrarBookResponse>> Execute(
            AtualizarBookCommand request,
            CancellationToken cancellationToken)
        {
            Book? book = await unitOfWork.FindByIdAsync<Book>(request.Id);

            if (book == null)
            {
                return MapearResponse(false, "Book não encontrado", request.Id);
            }

            book.AtualizarDados(request.BookName, request.Price, request.Category, request.Author, request.Date);

            await unitOfWork.UpdateAsync(book);

            StatusCommit commitStatus = await unitOfWork.Commit(cancellationToken);

            if (commitStatus == StatusCommit.Sucesso)
            {
                return MapearResponse(true, "Book atualizado com sucesso", book.Id);
            }

            return MapearResponse(false, "Falha ao atualizar o book", book.Id);
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