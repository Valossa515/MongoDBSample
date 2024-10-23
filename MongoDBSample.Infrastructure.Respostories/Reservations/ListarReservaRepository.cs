using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Application.Reservations.Data;
using MongoDBSample.Application.Reservations.Queries;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Domain.Model.Reservations;
using MongoDBSample.Infrastructure.Respostories.Context;

namespace MongoDBSample.Infrastructure.Respostories.Reservations
{
    public class ListarReservaRepository
        : QueryHandler<ListarReservaQuery, PaginatedResponse<ListarReservaResponse>>
    {
        private readonly MongoDBContext context;

        public ListarReservaRepository(MongoDBContext context)
        {
            this.context = context;
        }

        protected async override Task<PaginatedResponse<ListarReservaResponse>> Execute(
            ListarReservaQuery request,
            CancellationToken cancellationToken)
        {
            IMongoCollection<Reservation> reservationCollection = context.GetCollection<Reservation>("Reservation");
            long totalCount = await reservationCollection.CountDocumentsAsync(FilterDefinition<Reservation>.Empty, cancellationToken: cancellationToken);

            List<Reservation> reservations = await reservationCollection.Find(FilterDefinition<Reservation>.Empty)
                .Skip((request.Page - 1) * request.PageSize)
                .Limit(request.PageSize)
                .ToListAsync(cancellationToken);

            List<string> bookIds = reservations.SelectMany(r => r.BookIds).Distinct().ToList();
            List<BookResponse> allBooks = await BuscarLivrosPorIdsAsync(bookIds, cancellationToken);

            PaginatedResponse<ListarReservaResponse> paginatedResponse = new()
            {
                Data = reservations.Select(r => new ListarReservaResponse
                {
                    Id = r.Id.ToString(),
                    UserId = r.UserId.ToString(),
                    Books = r.BookIds.Select(bookId => allBooks.FirstOrDefault(b => b.Id == bookId)).ToList(),
                    UserName = r.UserName,
                    ReservationDate = r.ReservationDate,
                    ReturnDate = r.ReturnDate,
                    Status = r.Status
                }).ToList(),
                TotalCount = (int)totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return paginatedResponse;
        }

        private async Task<List<BookResponse>> BuscarLivrosPorIdsAsync(
            IEnumerable<string> bookIds,
            CancellationToken cancellationToken)
        {
            if (bookIds == null || !bookIds.Any())
            {
                return new List<BookResponse>();
            }

            List<ObjectId> objectIdList = bookIds.Select(id => ObjectId.Parse(id)).ToList();
            FilterDefinition<Book> bookFilter = Builders<Book>.Filter.In("_id", objectIdList);
            IMongoCollection<Book> bookCollection = context.GetCollection<Book>("Book");

            List<Book> books = await bookCollection.Find(bookFilter).ToListAsync(cancellationToken);
            return books.Select(book => new BookResponse
            {
                Id = book.Id.ToString(),
                BookName = book.BookName,
                Author = book.Author,
                Category = book.Category,
                Price = book.Price,
                Date = book.Date
            }).ToList();
        }
    }
}