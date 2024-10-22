namespace MongoDBSample.Application.Users.Data
{
    public class LoginResponse
    {
        public Guid? Id { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }
    }
}
