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
        : QueryHandler<ListarReservaPorIdQuery, IEnumerable<ListarReservaResponse>>
    {
        private readonly MongoDBContext context;

        public ListarReservaPorIdRepository(MongoDBContext context)
        {
            this.context = context;
        }

        protected async override Task<IEnumerable<ListarReservaResponse>> Execute(
            ListarReservaPorIdQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<Reservation> reservations = await ListarReservaPorIdAsync(request, cancellationToken);

            if (reservations == null || !reservations.Any())
            {
                return new List<ListarReservaResponse>();
            }

            List<ListarReservaResponse> responseList = new();

            foreach (Reservation reservation in reservations)
            {
                List<BookResponse> books = await BuscarLivrosPorIdsAsync(reservation.BookIds, cancellationToken);

                ListarReservaResponse response = new()
                {
                    Id = reservation.Id.ToString(),
                    UserId = reservation.UserId.ToString(),
                    Books = books,
                    UserName = reservation.UserName,
                    ReservationDate = reservation.ReservationDate,
                    ReturnDate = reservation.ReturnDate,
                    Status = reservation.Status
                };

                responseList.Add(response);
            }

            return responseList;
        }

        private async Task<List<Reservation>> ListarReservaPorIdAsync(
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

            List<Reservation> reservations = await collection.Find(filter).ToListAsync(cancellationToken);

            return reservations;
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