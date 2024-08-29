using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Books.Data;

namespace MongoDBSample.Application.Books.Commands
{
    public class CadastrarBookCommand
        : ICommand<CadastrarBookResponse>
    {
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}