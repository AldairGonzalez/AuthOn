using AuthOn.Application.Data;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Primitives;
using AuthOn.Infrastructure.Persistence.Repositories;
using AuthOn.Infrastructure.Persistence;
using AuthOn.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthOn.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using AuthOn.Application.Services.Interfaces;
using AuthOn.Application.Configurations;
using AuthOn.Domain.Entities.Emails;
using Microsoft.Extensions.Options;
using AuthOn.Domain.Entities.UserTokens;

namespace AuthOn.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SQLConnection")));

            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IEmailRepository, EmailRepository>();

            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            services.AddScoped<ITokenManager, TokenManager>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserTokenRepository, UserTokenRepository>();

            services.AddSingleton(sp => sp.GetRequiredService<IOptions<ApiInformationConfiguration>>().Value);

            services.Configure<ApiInformationConfiguration>(configuration.GetSection("ApiInformation"));

            services.Configure<EndPointsConfiguration>(configuration.GetSection("EndPoints"));

            services.Configure<GmailConfiguration>(configuration.GetSection("GmailSettings"));

            services.Configure<Dictionary<string, TokenConfiguration>>(configuration.GetSection("TokenConfiguration"));

            return services;
        }
    }
}