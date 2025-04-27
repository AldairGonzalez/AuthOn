using AuthOn.Application.Configurations;
using AuthOn.WebApi.Middlewares;

namespace AuthOn.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();
            services.AddTransient<GlobalExceptionHandlingMiddleware>();
            return services;
        }
    }
}