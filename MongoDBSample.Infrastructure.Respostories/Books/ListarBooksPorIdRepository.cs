using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Application.Books.Queries;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Infrastructure.Respostories.Context;

namespace MongoDBSample.Infrastructure.Respositories.Books
{
    public class ListarBooksPorIdRepository
        : QueryHandler<ListarBooksPorIdQuery, BookResponse>
    {
        private readonly MongoDBContext context;

        public ListarBooksPorIdRepository(
            MongoDBContext context)
        {
            this.context = context;
        }

        protected async override Task<BookResponse> Execute(
            ListarBooksPorIdQuery request,
            CancellationToken cancellationToken)
        {
            Book book = await ListarBookPorIdAsync(request, cancellationToken);

            if (book == null)
            {
                return null;
            }

            return new BookResponse
            {
                Id = book.Id.ToString(),
                BookName = book.BookName,
                Author = book.Author,
                Category = book.Category,
                Price = book.Price
            };
        }

        private async Task<Book> ListarBookPorIdAsync(
            ListarBooksPorIdQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                return null;
            }

            if (ObjectId.TryParse(request.Id, out ObjectId objectId))
            {
                FilterDefinition<Book> filter = Builders<Book>.Filter.Eq("_id", objectId);
                IMongoCollection<Book> collection = context.GetCollection<Book>("Book");

                return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            }

            return null;
        }
    }
}