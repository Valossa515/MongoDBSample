using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Books.Data;

namespace MongoDBSample.Application.Books.Queries
{
    public class ListarBooksPorIdQuery
        : IQuery<BookResponse>
    {
        public string? Id { get; set; }
    }
}