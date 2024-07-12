using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Auth.Database.Contexts;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UserAuthContext>
{
    public UserAuthContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:UserAuth"] = "Host=localhost;Database=auth;Username=postgres;Password=postgres;Port=5432",
            }!)
            .AddEnvironmentVariables("App_")
            .Build();
        
        var connectionString = configuration.GetConnectionString("UserAuth");
        var builder = new DbContextOptionsBuilder<UserAuthContext>();
        builder.UseNpgsql(connectionString);

        return new UserAuthContext(builder.Options);
    }
}