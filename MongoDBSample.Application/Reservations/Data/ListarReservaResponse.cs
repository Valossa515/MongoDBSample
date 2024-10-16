using MongoDBSample.API.Controllers.Reservations.enums;
using MongoDBSample.Application.Books.Data;

namespace MongoDBSample.Application.Reservations.Data
{
    public class ListarReservaResponse
    {
        public string? Id { get; set; }
        public List<BookResponse> Books { get; set; } = new List<BookResponse>();
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public ReservationStatus Status { get; set; }
    }
}