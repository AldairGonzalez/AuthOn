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

            services.Configure<ApiBehaviorOptions>(options => { });

            services.AddSingleton<ProblemDetailsFactory>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<ApiBehaviorOptions>>().Value;
                return new AuthOnProblemDetailsFactory(options);
            });

            services.AddTransient<GlobalExceptionHandlingMiddleware>();

            return services;
        }
    }
}