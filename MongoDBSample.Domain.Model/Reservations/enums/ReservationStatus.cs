using System.Text.Json.Serialization;

namespace MongoDBSample.API.Controllers.Reservations.enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReservationStatus
    {
        Active,
        Completed,
        Canceled,
        Expired
    }
}