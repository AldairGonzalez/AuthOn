using AuthOn.WebApi.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace AuthOn.WebApi.Middlewares
{
    public class GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger, ProblemDetailsFactory problemDetailsFactory) : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;
        private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                const string logMessageTemplate = "An unexpected error occurred: {ErrorMessage}";
                _logger.LogError(ex, logMessageTemplate, ex.Message);

                context.Items[HttpContextItemKeys.Errors] = new List<Error>
                    {
                        Error.Unexpected(description: ex.Message)
                    };

                var problemDetails = _problemDetailsFactory.CreateProblemDetails(
                    context,
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    title: "Internal Server Error",
                    detail: "An unexpected error occurred. Please try again later."
                );

                problemDetails.Extensions["traceId"] = context.TraceIdentifier;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}