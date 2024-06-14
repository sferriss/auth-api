namespace Auth.Domain.Abstractions;

public interface IEntity
{
    public Guid Id { get; init; }
}