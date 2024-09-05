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
        : QueryHandler<ListarBooksQuery, PaginatedResponse<BookResponse>>
    {
        private readonly MongoDBContext context = context;

        protected override async Task<PaginatedResponse<BookResponse>> Execute(
            ListarBooksQuery request,
            CancellationToken cancellationToken)
        {
            IMongoCollection<Book> collection = context.GetCollection<Book>("Book");

            // Calcular o número total de documentos
            long totalCount = await collection.CountDocumentsAsync(FilterDefinition<Book>.Empty, cancellationToken: cancellationToken);

            // Pular documentos conforme a página atual e tamanho da página
            List<Book> books = await collection.Find(FilterDefinition<Book>.Empty)
                .Skip((request.Page - 1) * request.PageSize)
                .Limit(request.PageSize)
                .ToListAsync(cancellationToken);

            PaginatedResponse<BookResponse> paginatedResponse = new()
            {
                Data = books.Select(b => new BookResponse
                {
                    Id = b.Id.ToString(),
                    BookName = b.BookName,
                    Author = b.Author,
                    Category = b.Category,
                    Price = b.Price,
                    Date = b.Date
                }).ToList(),
                TotalCount = (int)totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return paginatedResponse;
        }

        private async Task<IEnumerable<Book>> ListarTodosBooksAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IMongoCollection<Book> collection = context.GetCollection<Book>("Book");

            return await collection
                .Find(FilterDefinition<Book>.Empty)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);
        }

        private async Task<int> GetTotalBookCountAsync(CancellationToken cancellationToken)
        {
            IMongoCollection<Book> collection = context.GetCollection<Book>("Book");
            return (int)await collection.CountDocumentsAsync(FilterDefinition<Book>.Empty, cancellationToken: cancellationToken);
        }
    }
}
