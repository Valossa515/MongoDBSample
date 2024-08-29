namespace MongoDBSample.Domain.Model.UnitOfWork
{
    public enum StatusCommit
    {
        Sucesso = 1,
        Concorrencia = 2,
        Falha = 3,
    }
}