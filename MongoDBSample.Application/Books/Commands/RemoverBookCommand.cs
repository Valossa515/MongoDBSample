using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Books.Data;

namespace MongoDBSample.Application.Books.Commands
{
    public class RemoverBookCommand
        : ICommand<CadastrarBookResponse>
    {
        public string? Id { get; set; }
    }
}