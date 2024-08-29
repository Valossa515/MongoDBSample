using MongoDB.Driver;
using MongoDBSample.Domain.Model.UnitOfWork;
using Newtonsoft.Json;
using System.Reflection;

namespace MongoDBSample.Infrastructure.Respostories.Context
{
    public class MongoDBContext : IUnitOfWork
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(
            string connectionString,
            string databaseName)
        {
            MongoClient client = new(connectionString);
            _database = client.GetDatabase(databaseName);

            // Register class maps here
            RegisterClassMaps();
        }

        public IMongoCollection<T> GetCollection<T>(string name) =>
          _database.GetCollection<T>(name);
        public async Task AddAsync<T>(T entity)
        {
            IMongoCollection<T> collection = GetCollection<T>(typeof(T).Name);
            await collection.InsertOneAsync(entity);
        }

        public async Task<StatusCommit> Commit(CancellationToken cancellationToken)
        {
            try
            {
                // Implementar commit de transações se necessário.
                await Task.CompletedTask;
                return StatusCommit.Sucesso;
            }
            catch (Exception)
            {
                return StatusCommit.Falha;
            }
        }

        public void Remove<T>(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            dynamic dynamicEntity = entity as dynamic;
            if (dynamicEntity == null || dynamicEntity.Id == null)
            {
                throw new InvalidOperationException("Entity must have an Id property.");
            }

            dynamic id = dynamicEntity.Id;
            IMongoCollection<T> collection = GetCollection<T>(typeof(T).Name);
            collection.DeleteOne(Builders<T>.Filter.Eq("Id", id));
        }

        public void RemoveRange<T>(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                Remove(entity);
            }
        }

        private static void RegisterClassMaps()
        {
            IEnumerable<Type> classMapTypes = Assembly.GetExecutingAssembly().GetTypes()
             .Where(t => typeof(IEntityClassMap).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (Type type in classMapTypes)
            {
                IEntityClassMap? classMapInstance = Activator.CreateInstance(type) as IEntityClassMap;
                classMapInstance?.RegisterClassMap();
            }
        }

        private static string SerializeObject(Exception exception) =>
        JsonConvert.SerializeObject(exception);

        public async Task UpdateAsync<T>(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            dynamic dynamicEntity = entity as dynamic;
            if (dynamicEntity == null || dynamicEntity.Id == null)
            {
                throw new InvalidOperationException("Entity must have an Id property.");
            }

            dynamic id = dynamicEntity.Id;
            IMongoCollection<T> collection = GetRepository<T>();
            await collection.ReplaceOneAsync(Builders<T>.Filter.Eq("Id", id), entity);
        }

        public IMongoCollection<T> GetRepository<T>()
        {
            return GetCollection<T>(typeof(T).Name);
        }

        public async Task<T?> FindByIdAsync<T>(string id)
        {
            IMongoCollection<T> collection = GetRepository<T>();
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("Id", id);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}