using MongoDBSample.API.Controllers.Reservations.enums;
using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Reservations.Data;
using System.Text.Json.Serialization;

namespace MongoDBSample.Application.Reservations.Commands
{
    public class CadastrarReservationCommand
        : ICommand<CadastrarReservationResponse>
    {
        public required IEnumerable<string> BookIds { get; set; }
        public required string UserId { get; set; }
        [JsonIgnore]
        public string? UserName { get; set; }
        public DateTime ReservationDate { get; set; }
        [JsonIgnore]
        public DateTime? ReturnDate { get; set; }
        [JsonIgnore]
        public ReservationStatus Status { get; set; }
    }
}
