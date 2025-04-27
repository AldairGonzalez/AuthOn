using AuthOn.Application.Common.Behaviors;
using AuthOn.Application.Configurations;
using AuthOn.Application.Services;
using AuthOn.Application.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AuthOn.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
            });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));

            services.AddScoped<IEmailTemplateService, EmailTemplateService>();

            services.AddScoped<IEmailSenderService, EmailSenderService>();

            services.AddSingleton<ApiEndPointsConstants>();

            services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();

            return services;
        }
    }
}