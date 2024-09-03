using MongoDB.Driver;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Application.Books.Queries;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Infrastructure.Respostories.Context;

namespace MongoDBSample.Infrastructure.Respostories.Books
{
    public class ListarBooksRepository(
        MongoDBContext context)
                : QueryHandler<ListarBooksQuery, IEnumerable<BookResponse>>
    {
        private readonly MongoDBContext context = context;

        protected async override Task<IEnumerable<BookResponse>> Execute(
            ListarBooksQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<Book> books = await ListarTodosBooksAsync(cancellationToken);

            return books.Select(b => new BookResponse
            {
                Id = b.Id.ToString(),
                BookName = b.BookName,
                Author = b.Author,
                Category = b.Category,
                Price = b.Price,
                Date = b.Date
            }).ToList();
        }

        private async Task<IEnumerable<Book>> ListarTodosBooksAsync(
            CancellationToken cancellationToken)
        {
            IMongoCollection<Book> collection = context.GetCollection<Book>("Book");

            // Buscando todos os documentos da coleção
            return await collection.Find(FilterDefinition<Book>.Empty).ToListAsync(cancellationToken);
        }
    }
}