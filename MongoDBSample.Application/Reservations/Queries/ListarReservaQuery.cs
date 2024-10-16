using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Application.Reservations.Data;

namespace MongoDBSample.Application.Reservations.Queries
{
    public class ListarReservaQuery
        : IQuery<PaginatedResponse<ListarReservaResponse>>
    {
        public string? Id { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}