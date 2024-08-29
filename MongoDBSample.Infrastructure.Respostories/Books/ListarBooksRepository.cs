using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Application.Books.Queries;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Infrastructure.Respostories.Context;

namespace MongoDBSample.Infrastructure.Respositories.Books
{
    public class ListarBooksRepository
        : QueryHandler<ListarBooksQuery, IEnumerable<BookResponse>>
    {
        private readonly MongoDBContext context;

        public ListarBooksRepository(
            MongoDBContext context)
        {
            this.context = context;
        }

        protected async override Task<IEnumerable<BookResponse>> Execute(
            ListarBooksQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<Book> books = await ListarBooksAsync(request, cancellationToken);
            return books.Select(b => new BookResponse
            {
                Id = b.Id.ToString(),
                Name = b.BookName,
                Author = b.Author,
                Category = b.Category,
                Price = b.Price
            }).ToList();
        }

        private async Task<IEnumerable<Book>> ListarBooksAsync(
             ListarBooksQuery request,
             CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                return [];
            }

            if (ObjectId.TryParse(request.Id, out ObjectId objectId))
            {
                FilterDefinition<Book> filter = Builders<Book>.Filter.Eq("_id", objectId);
                IMongoCollection<Book> collection = context.GetCollection<Book>("Book");

                return await collection.Find(filter).ToListAsync(cancellationToken);
            }

            return [];
        }
    }
}