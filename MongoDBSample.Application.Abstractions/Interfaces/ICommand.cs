using MediatR;
using MongoDBSample.Application.Abstractions.Data;

namespace MongoDBSample.Application.Abstractions.Interfaces
{
    public interface ICommand : IRequest<Response> { }
    public interface ICommand<TResult> : IRequest<Response<TResult>> { }
}