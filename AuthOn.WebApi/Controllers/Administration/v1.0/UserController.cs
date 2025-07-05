using AuthOn.Application.Users.Commands.Create;
using AuthOn.Application.Users.Commands.Update.ActivateUser;
using AuthOn.Application.Users.Commands.Update.Login;
using AuthOn.Application.Users.Commands.Update.RefreshToken;
using AuthOn.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthOn.WebApi.Controllers.Administration.v1._0
{
    [Route("api/[controller]")]
    public class UserController(ISender mediator, ITokenManager tokenManager) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly ITokenManager _tokenManager = tokenManager;

        #region Methods

        [HttpGet("Activate")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

            var activateUserCommand = new ActivateUserCommand(userId, emailId, token);

            var result = await _mediator.Send(activateUserCommand);

            return result.Match<IActionResult>(
                _ => Ok(),
                errors => Problem(errors)
            );
        }

        [HttpPost("Create")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var createUserResult = await _mediator.Send(command);

            return createUserResult.Match<IActionResult>(
                _ => StatusCode(StatusCodes.Status201Created),
                errors => Problem(errors)
            );
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var loginResult = await _mediator.Send(command);

            return loginResult.Match(
                _ => Ok(loginResult.Value),
                errors => Problem(errors)
            );
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenUserCommand command)
        {
            var refreshTokenResult = await _mediator.Send(command);

            return refreshTokenResult.Match(
                _ => Ok(refreshTokenResult.Value),
                errors => Problem(errors)
            );
        }

        #endregion

    }
}