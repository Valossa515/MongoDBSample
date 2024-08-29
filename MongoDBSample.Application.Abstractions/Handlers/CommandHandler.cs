using MediatR;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MongoDBSample.Application.Abstractions.Handlers
{
    public abstract class CommandHandler<TCommand> : IRequestHandler<TCommand, Response>
        where TCommand : ICommand
    {
        protected abstract Task<Response> Execute(TCommand request, CancellationToken cancellationToken);

        protected Response Ok() => new();

        protected Response BadRequest(string message) =>
            new(ResponseStatus.BadRequest, message);

        protected Response BadRequest(ValidationException ex) =>
            new(ex);
        protected Response Forbidden(string message) =>
            new(ResponseStatus.Forbidden, message);

        protected Response InternalServerError(Exception ex) =>
            new(ex);

        protected Response NotFound(string message) =>
            new(ResponseStatus.NotFound, message);

        async Task<Response> IRequestHandler<TCommand, Response>.Handle(TCommand request, CancellationToken cancellationToken)
        {
            return await Execute(request, cancellationToken);
        }
    }

    public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, Response<TResult>>
        where TCommand : ICommand<TResult>
    {
        protected abstract Task<Response<TResult>> Execute(TCommand request, CancellationToken cancellationToken);

        protected Response<TResult> Ok(TResult result) =>
            new(result);

        protected Response<TResult> BadRequest(string message) =>
            new(ResponseStatus.BadRequest, message);

        protected Response<TResult> BadRequest(ValidationException ex) =>
            new(ex);

        protected Response<TResult> Forbidden(string message) =>
            new(ResponseStatus.Forbidden, message);

        protected Response<TResult> InternalServerError(Exception ex) =>
            new(ex);

        protected Response<TResult> NotFound(string message) =>
            new(ResponseStatus.NotFound, message);

        async Task<Response<TResult>> IRequestHandler<TCommand, Response<TResult>>.Handle(TCommand request, CancellationToken cancellationToken)
        {
            return await Execute(request, cancellationToken);
        }
    }
}
