using MediatR;
using MongoDBSample.Application.Abstractions.Data;

namespace MongoDBSample.Application.Abstractions.Interfaces
{
    public interface IQuery<TResult> : IRequest<Response<TResult>> { }
}