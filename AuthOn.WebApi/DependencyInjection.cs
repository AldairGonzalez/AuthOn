using AuthOn.WebApi.Common.Errors;
using AuthOn.WebApi.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

namespace AuthOn.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();

            // Asegúrate de que ApiBehaviorOptions esté configurado
            services.Configure<ApiBehaviorOptions>(options => { });

            // Registrar tu propio ProblemDetailsFactory
            services.AddSingleton<ProblemDetailsFactory>(sp =>
            {
                // Desenvolver IOptions<ApiBehaviorOptions> a ApiBehaviorOptions
                var options = sp.GetRequiredService<IOptions<ApiBehaviorOptions>>().Value;
                return new AuthOnProblemDetailsFactory(options);
            });

            // Registrar el middleware para el manejo global de excepciones
            services.AddTransient<GlobalExceptionHandlingMiddleware>();

            return services;
        }
    }
}