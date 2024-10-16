namespace MongoDBSample.Infrastructure.Respostories.Context
{
    public class BookStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string BooksCollectionName { get; set; } = null!;
        public string ReservationsCollectionName { get; set; } = null!;
    }

    public class MongoDbConfig
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}