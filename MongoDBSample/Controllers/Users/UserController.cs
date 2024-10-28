using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Users.Commands;
using MongoDBSample.Application.Users.Data;
using MongoDBSample.Domain.Model.Users;

namespace MongoDBSample.API.Controllers.Users
{
    [ApiController]
    [Route("users")]
    public class UserController
        : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly RoleManager<ApplicationRole> roleManager;

        public UserController(
            IMediator mediator,
            RoleManager<ApplicationRole> roleManager)
        {
            this.mediator = mediator;
            this.roleManager = roleManager;
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

            return result.IsSuccess ? Ok(result) : BadRequest(result.Message);

        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [Route("roles/add")]
        public async Task<IActionResult> CreateRole([FromBody]
            CreateRoleRequest request)
        {
            ApplicationRole appRole = new() { Name = request.Role };
            IdentityResult createRole = await roleManager.CreateAsync(appRole);

            return Ok(new { message = "role created succesfully" });
        }
    }
}