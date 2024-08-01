using Auth.Database.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Auth.Application.IntegrationTest;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("auth")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<UserAuthContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<UserAuthContext>(options =>
            {
                options
                    .UseNpgsql(_dbContainer.GetConnectionString());
            });
        });
        
        var script = File.ReadAllText("../../../Initial.sql");
        _dbContainer.ExecScriptAsync(script);
    }

    public Task InitializeAsync() => 
        _dbContainer.StartAsync();
    
    public new Task DisposeAsync() => 
        _dbContainer.StopAsync();
}