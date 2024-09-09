using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace MongoDBSample.Domain.Model.Users
{
    [CollectionName("Roles")]
    public class ApplicationRole
        : MongoIdentityRole<Guid>
    {
    }
}