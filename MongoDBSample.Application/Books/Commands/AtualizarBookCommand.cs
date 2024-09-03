using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Books.Data;
using System.Text.Json.Serialization;

namespace MongoDBSample.Application.Books.Commands
{
    public class AtualizarBookCommand
        : ICommand<CadastrarBookResponse>
    {
        [JsonIgnore]
        public string? Id { get; set; }
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}