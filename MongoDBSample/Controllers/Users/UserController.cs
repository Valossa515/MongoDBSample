using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Users.Commands;
using MongoDBSample.Application.Users.Data;

namespace MongoDBSample.API.Controllers.Users
{
    [ApiController]
    [Route("users")]
    public class UserController
        : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(Response), (int)ResponseStatus.Ok)]
        [ProducesResponseType(typeof(Response), (int)ResponseStatus.BadRequest)]
        public async Task<IActionResult> CadastrarUser(
            [FromBody] CadastrarUserRequest command)
        {
            CadastrarUserCommand cadastrarUserCommand = new()
            {
                Name = command.Name,
                Email = command.Email,
                Password = command.Password
            };

            Response<CadastrarUserResponse> result = await mediator.Send(cadastrarUserCommand);
            return Ok(result);
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(LoginResponse), (int)ResponseStatus.Ok)]
        [ProducesResponseType(typeof(LoginResponse), (int)ResponseStatus.BadRequest)]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            LoginCommand loginCommand = new()
            {
                Email = request.Email,
                Password = request.Password
            };

            Response<LoginResponse> result = await mediator.Send(loginCommand);
            return Ok(result);
        }

    }
}