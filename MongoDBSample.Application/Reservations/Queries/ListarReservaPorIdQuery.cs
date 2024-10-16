using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Reservations.Data;

namespace MongoDBSample.Application.Reservations.Queries
{
    public class ListarReservaPorIdQuery
        : IQuery<ListarReservaResponse>
    {
        public string? Id { get; set; }
    }
}