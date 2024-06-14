using System.Globalization;
using System.Text;
using Auth.Api.ExceptionHandlers;
using Auth.Api.ExceptionHandlers.Factories;
using Auth.Api.Mappers;
using Auth.Application.Exceptions;
using Auth.Application.Extensions;
using Auth.Application.Settings;
using Auth.Domain.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Auth.Api.Startup;

public static class DependencyInjection
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
        builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        
        builder.AddAuthentication();
        
        builder.Services.AddExceptionHandlers()
            .AddHandler<BusinessValidationException, BusinessValidationExceptionHandler>()
            .AddHandler<NotFoundException, NotFoundExceptionHandler>()
            .AddHandler<UnauthorizedException, UnauthorizedExceptionHandler>();

        builder.Services.AddApplicationDependencies(builder.Configuration.GetConnectionString("UserAuth")!);
        builder.Services.RegisterExceptionHandlers();
        builder.Services.AddMappers();
        builder.RegisterServicesSingletonDependencies();
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth Api", Version = "v1" });
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT token into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
    
    private static void RegisterServicesSingletonDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
        builder.Services.AddSingleton(ctx => ctx.GetRequiredService<IOptions<ApplicationSettings>>().Value);
    }

    private static void AddAuthentication(this WebApplicationBuilder builder)
    {
        var jwtSecret = builder.Configuration["ApplicationSettings:JwtSecret"]; 

        var key = Encoding.ASCII.GetBytes(jwtSecret!);
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        
        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
    }
    
    private static void AddMappers(this IServiceCollection services)
    {
        services.AddTransient<UserMapper>();
    }
    
    private static ExceptionHandlerFactory AddExceptionHandlers(this IServiceCollection services)
    {
        var factory = new ExceptionHandlerFactory();
    
        services.AddSingleton(factory);
    
        return factory;
    }
    
    private static void RegisterExceptionHandlers(this IServiceCollection services)
    {
        services.AddTransient<BusinessValidationExceptionHandler>();
        services.AddTransient<NotFoundExceptionHandler>();
        services.AddTransient<UnauthorizedExceptionHandler>();
    }
}