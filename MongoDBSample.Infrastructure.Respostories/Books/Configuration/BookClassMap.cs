using MongoDB.Bson.Serialization;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Infrastructure.Respostories.Context;

namespace MongoDBSample.Infrastructure.Respostories.Books.Configuration
{
    public class BookClassMap
        : IEntityClassMap
    {
        public void RegisterClassMap()
        {
            BsonClassMap.RegisterClassMap<Book>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.MapMember(c => c.BookName).SetElementName("Name");
                cm.MapMember(c => c.Price).SetElementName("Price");
                cm.MapMember(c => c.Category).SetElementName("Category");
                cm.MapMember(c => c.Author).SetElementName("Author");
                cm.MapMember(c => c.Date).SetElementName("Date");
            });
        }
    }
}