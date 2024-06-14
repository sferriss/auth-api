using Auth.Database.Contexts;
using Auth.Domain.Abstractions;

namespace Auth.Database.Abstractions;

public class RepositoryBase<T> : IRepository<T> where T : class, IEntity
{
    private readonly UserAuthContext _authContext;

    public RepositoryBase(UserAuthContext authContext)
    {
        _authContext = authContext;
    }
    
    public T Add(T entity)
    {
        _authContext.Add(entity);
        return entity;
    }

    public void Remove(T entity)
    {
        _authContext.Remove(entity);
    }

    public T Update(T entity)
    {
        _authContext.Update(entity);
        return entity;
    }

    protected IQueryable<T> GetQuery()
    {
        return _authContext.Set<T>();
    }
        
}