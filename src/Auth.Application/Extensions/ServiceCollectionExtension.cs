using Auth.Application.Services;
using Auth.Database.Abstractions;
using Auth.Database.Contexts;
using Auth.Database.Repositories;
using Auth.Domain.Repositories;
using Auth.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplicationDependencies(this IServiceCollection services, string connectionString)
    {
        services.AddDatabase(connectionString);
        services.AddRepositories();
        services.AddServices();
    }
    
    private static void AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<UserAuthContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddTransient<IUnitOfWork>(x => x.GetService<UserAuthContext>()!);
    }
    
    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
    }
    
    private static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<ILoginService, LoginService>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<ICreateAdminUserService, CreateAdminUserService>();
    }
}