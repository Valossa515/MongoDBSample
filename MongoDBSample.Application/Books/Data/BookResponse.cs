﻿namespace MongoDBSample.Application.Books.Data
{
    public class BookResponse
    {
        public string? Id { get; set; }
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}
