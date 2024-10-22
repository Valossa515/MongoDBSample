using MongoDBSample.Application.Abstractions.Interfaces;
using MongoDBSample.Application.Users.Data;

namespace MongoDBSample.Application.Users.Commands
{
    public class LoginCommand
        : ICommand<LoginResponse>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}