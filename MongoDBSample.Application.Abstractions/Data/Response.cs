using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MongoDBSample.Application.Abstractions.Data
{
    public class Response
    {
        public Response()
        {
            TraceId = Elastic.Apm.Agent.Tracer?.CurrentTransaction?.TraceId;
        }

        public Response(object result)
             : this()
        {
            Result = result;
        }

        public Response(Exception exception)
            : this()
        {
            Status = ResponseStatus.InternalServerError;
            Message = exception.Message;
        }

        public Response(ValidationException validationException)
            : this()
        {
            Status = ResponseStatus.BadRequest;
            Message = validationException.Message;
            Errors = new[]
            {
                new Error(validationException)
            };
        }

        public Response(IEnumerable<ValidationFailure> validationFailures)
            : this()
        {
            Status = ResponseStatus.BadRequest;
            Message = "Erro na validação da requisição.";
            Errors = validationFailures.Select(validationFailure => new Error(validationFailure)).ToArray();
        }

        public Response(ResponseStatus responseStatus, string message)
            : this()
        {
            Status = responseStatus;
            Message = message;
        }

        public ResponseStatus Status { get; set; } = ResponseStatus.Ok;

        public string Message { get; set; }

        public Error[] Errors { get; set; }

        [JsonIgnore]
        public object Result { get; set; }

        [JsonIgnore]
        public bool IsSuccess => (Errors == null || Errors.Length == 0) &&
                                 Status == ResponseStatus.Ok;

        public string TraceId { get; set; }
    }

    public class Response<TResult> : Response
    {
        public Response() { }

        public Response(TResult result) : base(result) { }

        public Response(ValidationException validationException) : base(validationException) { }

        public Response(IEnumerable<ValidationFailure> validationFailures) : base(validationFailures) { }

        public Response(Exception exception) : base(exception) { }

        public Response(ResponseStatus responseStatus, string message) : base(responseStatus, message) { }

        public new TResult Result
        {
            get => (TResult)base.Result;
            set => base.Result = value;
        }
    }
}