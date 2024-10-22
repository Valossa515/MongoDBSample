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
    public class ListarReservaPorIdRepository
        : QueryHandler<ListarReservaPorIdQuery, ListarReservaResponse>
    {
        private readonly MongoDBContext context;

        public ListarReservaPorIdRepository(MongoDBContext context)
        {
            this.context = context;
        }

        protected async override Task<ListarReservaResponse> Execute(
            ListarReservaPorIdQuery request,
            CancellationToken cancellationToken)
        {
            Reservation reservation = await ListarReservaPorIdAsync(request, cancellationToken);

            if (reservation == null)
            {
                return null;
            }

            List<BookResponse> books = await BuscarLivrosPorIdsAsync(reservation.BookIds, cancellationToken);

            return new ListarReservaResponse
            {
                Id = reservation.Id.ToString(),
                UserId = reservation.UserId.ToString(),
                Books = books,
                UserName = reservation.UserName,
                ReservationDate = reservation.ReservationDate,
                ReturnDate = reservation.ReturnDate
            };
        }

        private async Task<Reservation> ListarReservaPorIdAsync(
            ListarReservaPorIdQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id) && string.IsNullOrEmpty(request.UserName))
            {
                return null;
            }

            FilterDefinition<Reservation> filter;
            if (!string.IsNullOrEmpty(request.Id) && ObjectId.TryParse(request.Id, out ObjectId objectId))
            {
                filter = Builders<Reservation>.Filter.Eq("_id", objectId);
            }
            else if (!string.IsNullOrEmpty(request.UserName))
            {
                filter = Builders<Reservation>.Filter.Eq("UserName", request.UserName);
            }
            else
            {
                return null;
            }

            IMongoCollection<Reservation> collection = context.GetCollection<Reservation>("Reservation");
            return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
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