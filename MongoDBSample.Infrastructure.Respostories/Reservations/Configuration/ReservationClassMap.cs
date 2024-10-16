using MongoDB.Bson.Serialization;
using MongoDBSample.Domain.Model.Reservations;
using MongoDBSample.Infrastructure.Respostories.Context;

namespace MongoDBSample.Infrastructure.Respostories.Reservations.Configuration
{
    public class ReservationClassMap
            : IEntityClassMap
    {
        public void RegisterClassMap()
        {
            BsonClassMap.RegisterClassMap<Reservation>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.MapMember(c => c.ReservationDate).SetElementName("ReservationDate");
                cm.MapMember(c => c.Status).SetElementName("ReservationStatus");
                cm.MapMember(c => c.BookIds).SetElementName("BookId");
                cm.MapMember(c => c.UserId).SetElementName("UserId");
                cm.MapMember(c => c.UserName).SetElementName("UserName");
            });
        }
    }
}