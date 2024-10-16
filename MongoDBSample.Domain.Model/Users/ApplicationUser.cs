using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace MongoDBSample.Domain.Model.Users
{
    [CollectionName("Users")]
    public class ApplicationUser
        : MongoIdentityUser<Guid>
    {

    }
}