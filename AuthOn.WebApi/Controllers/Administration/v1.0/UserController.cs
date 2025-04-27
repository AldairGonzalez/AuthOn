using AuthOn.Application.Users.Commands.Create;
using AuthOn.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthOn.WebApi.Controllers.Administration.v1._0
{
    [Route("api/[controller]")]
    public class UserController(ISender mediator, ITokenManager tokenManager) : ApiController
    {
        private readonly ISender _mediator = mediator;
        private readonly ITokenManager _tokenManager = tokenManager;

        #region Methods

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var createUserResult = await _mediator.Send(command);

            return createUserResult.Match(
                user => Ok(),
                errors => Problem(errors)
            );
        }

        #endregion

    }
}