using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Books.Data;

namespace MongoDBSample.Application.Books.Queries
{
    public class ListarBooksQuery
        : IQuery<PaginatedResponse<BookResponse>>
    {
        public string? Id { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}