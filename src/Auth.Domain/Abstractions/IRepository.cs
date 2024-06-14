namespace Auth.Domain.Abstractions;

public interface IRepository<T> where T : IEntity
{
    T Add(T entity);
    
    void Remove(T entity);
    
    T Update(T entity);
}