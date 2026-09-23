// Distro.Application

using System.Text;
using Anexcs.Distro.Application.Abstractions.Persistence.Central;
using Anexcs.Distro.Application.Abstractions.Persistence.Tenant;
using Anexcs.Distro.Application.Abstractions.Tenancy;
using Anexcs.Distro.Application.Common.Interfaces;
using Anexcs.Distro.Infrastructure.Authentication;
using Anexcs.Distro.Infrastructure.Identity;

// Distro.Infrastructure
using Anexcs.Distro.Infrastructure.Persistence.Central;
using Anexcs.Distro.Infrastructure.Persistence.Central.Repositories;
using Anexcs.Distro.Infrastructure.Persistence.Tenant;
using Anexcs.Distro.Infrastructure.Tenancy;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Anexcs.Distro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CentralDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("CentralDatabase"),
                npgsql =>
                {
                    npgsql.MigrationsAssembly(
                        typeof(CentralDbContext).Assembly);
                });
        });
        
        services.AddDbContext<TenantDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("TenantDatabase"),
                npgsql =>
                {
                    npgsql.MigrationsAssembly(
                        typeof(TenantDbContext).Assembly.FullName);
                });
        });
        
        // 1. JWT Configuration & Token Generator Registration
        var jwtSettings = new JwtSettings();
        configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // 2. JWT Bearer Authentication Setup
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        Action<IdentityOptions> configureIdentityOptions = options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequiredLength = 12;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        };

        services
            .AddIdentityCore<CentralIdentityUser>(configureIdentityOptions)
            .AddEntityFrameworkStores<CentralDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddIdentityCore<TenantIdentityUser>(configureIdentityOptions)
            .AddEntityFrameworkStores<TenantDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped<
            ITenantDatabaseProvisioner,
            TenantDatabaseProvisioner>();
        
        services.AddScoped<ICentralUnitOfWork>(sp =>
            sp.GetRequiredService<CentralDbContext>());
        services.AddScoped<ITenantUnitOfWork>(sp =>
            sp.GetRequiredService<TenantDbContext>());
        
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ITenantDomainRepository, TenantDomainRepository>();
        
        return services;
    }
}