using AuthOn.WebApi.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuthOn.WebApi.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected ObjectResult Problem(List<Error> errors)
        {
            if (errors.Count is 0)
            {
                return Problem();
            }

            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            HttpContext.Items[HttpContextItemKeys.Errors] = errors;

            return Problem(errors[0]);
        }

        private ObjectResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, title: error.Description);
        }

        private ObjectResult ValidationProblem(List<Error> erros)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in erros)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }

            return (ObjectResult)ValidationProblem(modelStateDictionary);
        }
    }
}