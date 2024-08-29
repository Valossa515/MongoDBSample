namespace MongoDBSample.Application.Books.Data.Payloads
{
    public class BookPayload
    {
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}