using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Books.Data;

namespace MongoDBSample.Application.Books.Queries
{
    public class ListarBooksQuery
        : IQuery<IEnumerable<BookResponse>>
    {
        public string? Id { get; set; }
    }
}