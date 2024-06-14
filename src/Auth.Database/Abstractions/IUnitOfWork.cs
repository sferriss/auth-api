namespace Auth.Database.Abstractions;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}