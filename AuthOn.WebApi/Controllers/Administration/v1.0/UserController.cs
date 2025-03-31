using AuthOn.Application.Users.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthOn.WebApi.Controllers.Administration.v1._0
{
    [Route("api/[controller]")]
    public class UserController : ApiController
    {
        private readonly ISender _mediator;

        public UserController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var createUserResult = await _mediator.Send(command);

            return createUserResult.Match(
                user => Ok(),
                errors => Problem(errors)
            );
        }
    }
}