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
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
            services.Configure<GmailConfiguration>(configuration.GetSection("GmailSettings"));
            services.Configure<ApiInformationConfiguration>(configuration.GetSection("ApiInformation"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<ApiInformationConfiguration>>().Value);
            services.Configure<EndPointsConfiguration>(configuration.GetSection("EndPoints"));
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.Configure<Dictionary<string, TokenConfiguration>>(configuration.GetSection("TokenSettings"));

            return services;
        }
    }
}