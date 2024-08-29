using MediatR;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Interfaces;

namespace MongoDBSample.Application.Abstractions.Handlers
{
    public abstract class QueryHandler<TQuery, TResult> : IRequestHandler<TQuery, Response<TResult>>
        where TQuery : IQuery<TResult>
    {
        protected abstract Task<TResult> Execute(TQuery request, CancellationToken cancellationToken);

        async Task<Response<TResult>> IRequestHandler<TQuery, Response<TResult>>.Handle(TQuery request, CancellationToken cancellationToken)
        {
            TResult result = await Execute(request, cancellationToken);
            Response<TResult> response = result == null ?
                          new Response<TResult>(ResponseStatus.NoContent, "Registro não encontrado.") :
                          new Response<TResult> { Status = ResponseStatus.Ok, Result = result };

            return response;
        }
    }
}