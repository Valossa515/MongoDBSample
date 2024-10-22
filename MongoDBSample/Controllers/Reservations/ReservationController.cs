using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Application.Reservations.Commands;
using MongoDBSample.Application.Reservations.Data;
using MongoDBSample.Application.Reservations.Queries;

namespace MongoDBSample.API.Controllers.Reservations
{
    [Authorize(Roles = "USER")]
    [ApiController]
    [Route("reservations")]
    public class ReservationController
        : ControllerBase
    {
        private readonly IMediator mediator;

        public ReservationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(ListarReservaResponse), (int)ResponseStatus.Ok)]
        public async Task<IActionResult> Listar(
            [FromQuery] string? id,
            [FromQuery] string? userName)
        {
            ListarReservaPorIdQuery query = new()
            {
                Id = id,
                UserName = userName
            };

            Response<ListarReservaResponse> result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("cadastro")]
        [ProducesResponseType(typeof(Response), (int)ResponseStatus.Ok)]
        [ProducesResponseType(typeof(Response), (int)ResponseStatus.BadRequest)]
        public async Task<IActionResult> CreateReservation(
            [FromBody] CadastrarReservationCommand request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpPut("devolucao/{id}")]
        [ProducesResponseType(typeof(CadastrarReservationResponse), (int)ResponseStatus.Ok)]
        [ProducesResponseType(typeof(CadastrarReservationResponse), (int)ResponseStatus.BadRequest)]
        public async Task<IActionResult> Devolver(
            [FromRoute] string id,
            [FromBody] AtualizarReservationCommand request)
        {
            request.Id = id;
            return Ok(await mediator.Send(request));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<ListarReservaResponse>), (int)ResponseStatus.Ok)]
        public async Task<IActionResult> ListarTodos(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            ListarReservaQuery query = new()
            {
                Page = page,
                PageSize = pageSize
            };

            Response<PaginatedResponse<ListarReservaResponse>> result = await mediator.Send(query);

            PaginatedResponse<ListarReservaResponse> paginatedResponse = result.Result;

            return Ok(paginatedResponse);
        }
    }
}