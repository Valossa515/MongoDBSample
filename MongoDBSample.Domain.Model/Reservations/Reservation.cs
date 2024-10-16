using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDBSample.API.Controllers.Reservations.enums;

namespace MongoDBSample.Domain.Model.Reservations
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public IEnumerable<string>? BookIds { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        [BsonRepresentation(BsonType.String)]
        public ReservationStatus Status { get; set; }


        public void AtualizarDados(
            ReservationStatus status)
        {
            Status = status;
        }
    }
}