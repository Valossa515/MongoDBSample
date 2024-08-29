namespace MongoDBSample.Application.Abstractions.Data
{
    public enum ResponseStatus
    {
        Ok = 200,
        NoContent = 204,
        BadRequest = 400,
        Forbidden = 403,
        NotFound = 404,
        RequestTimeout = 408,
        InternalServerError = 500
    }
}