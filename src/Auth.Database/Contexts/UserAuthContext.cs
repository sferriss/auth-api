using Auth.Database.Abstractions;
using Auth.Database.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Auth.Database.Contexts;

public class UserAuthContext : DbContext, IUnitOfWork
{
    public UserAuthContext(DbContextOptions<UserAuthContext> options)
        : base(options)
    {
    }
    
    public Task<int> CommitAsync()
    {
        return SaveChangesAsync();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
        base.OnConfiguring(builder);
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        const string defaultSchema = "auth";

        modelBuilder.HasPostgresExtension("uuid-ossp");
        
        modelBuilder.ApplyConfiguration(new UserMapping(defaultSchema));
        modelBuilder.ApplyConfiguration(new ContactMapping(defaultSchema));
    }
}