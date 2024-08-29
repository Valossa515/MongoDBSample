using MongoDB.Driver;

namespace MongoDBSample.Domain.Model.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task AddAsync<T>(T entity);
        void Remove<T>(T entity);
        Task UpdateAsync<T>(T entity);
        IMongoCollection<T> GetRepository<T>();
        void RemoveRange<T>(IEnumerable<T> entities);
        Task<T?> FindByIdAsync<T>(string id);
        Task<StatusCommit> Commit(CancellationToken cancellationToken);
    }
}