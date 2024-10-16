using MongoDBSample.API.Controllers.Reservations.enums;
using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Reservations.Data;
using System.Text.Json.Serialization;

namespace MongoDBSample.Application.Reservations.Commands
{
    public class AtualizarReservationCommand
        : ICommand<CadastrarReservationResponse>
    {
        [JsonIgnore]
        public string? Id { get; set; }
        public ReservationStatus Status { get; set; }
    }
}