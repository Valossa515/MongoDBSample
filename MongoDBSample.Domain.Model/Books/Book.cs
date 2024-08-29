using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBSample.Domain.Model.Books
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string? BookName { get; set; } = null!;

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        public string? Category { get; set; } = null!;

        public string? Author { get; set; } = null!;

        public DateTime Date { get; set; }

        public void AtualizarDados(
            string? bookName,
            decimal price,
            string? category,
            string? author,
            DateTime date)
        {
            BookName = bookName;
            Price = price;
            Category = category;
            Author = author;
            Date = date;
        }
    }
}