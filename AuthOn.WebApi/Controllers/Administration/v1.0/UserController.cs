using AuthOn.Application.Users.Commands.Create;
using AuthOn.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuthOn.Application.Users.Commands.Update.ActivateUser;

namespace AuthOn.WebApi.Controllers.Administration.v1._0
{
    [Route("api/[controller]")]
    public class UserController(ISender mediator, ITokenManager tokenManager) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly ITokenManager _tokenManager = tokenManager;

        #region Methods

        [HttpGet("Activate")]
        public async Task<IActionResult> Activate([FromQuery] string token)
        {
            var tokenValidationResult = _tokenManager.ValidateActivationToken(token);

            if (tokenValidationResult.IsError)
            {
                return Problem(
                    detail: string.Join("; ", tokenValidationResult.Errors.Select(e => e.Description)),
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            var tokenData = tokenValidationResult.Value;

            if (tokenData.HasErrors)
            {
                return Problem(
                    detail: string.Join("; ", tokenData.GetErrors().Select(e => e.Description)),
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            var userId = tokenData.UserId.Value!.Value;
            var emailId = tokenData.EmailId.Value!.Value;

            var activateUserCommand = new ActivateUserCommand(userId, emailId);

            var result = await _mediator.Send(activateUserCommand);

            return result.Match(
                _ => Ok(),
                errors => Problem(errors)
            );
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var createUserResult = await _mediator.Send(command);

            return createUserResult.Match(
                _ => Ok(),
                errors => Problem(errors)
            );
        }

        #endregion

    }
}