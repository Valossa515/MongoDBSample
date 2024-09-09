namespace MongoDBSample.Application.Users.Data
{
    public class CadastrarUserResponse
    {
        public Guid? Id { get; set; }
        public bool Sucesso { get; set; }
        public string? Mensagem { get; set; }
        public string? Email { get; set; }
    }
}