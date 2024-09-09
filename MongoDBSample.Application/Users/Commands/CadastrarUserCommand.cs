using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Users.Data;

namespace MongoDBSample.Application.Users.Commands
{
    public class CadastrarUserCommand
        : ICommand<CadastrarUserResponse>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}