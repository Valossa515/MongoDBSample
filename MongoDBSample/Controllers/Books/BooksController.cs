using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Books.Commands;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Application.Books.Queries;

namespace MongoDBSample.API.Controllers.Books
{
    [ApiController]
    [Route("books")]
    public class BooksController(
        IMediator mediator)
                : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        /// <summary>
        /// Listar books
        /// </summary>
        /// <param name="id">ID do book</param>
        /// <returns>Lista de books</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookResponse), (int)ResponseStatus.Ok)]
        public async Task<IActionResult> ListarBooks(
            [FromRoute] string? id)
        {
            ListarBooksPorIdQuery query = new()
            {
                Id = id
            };

            Response<BookResponse> result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Listar todos os books
        /// </summary>
        /// <returns>Lista de todos os books</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookResponse>), (int)ResponseStatus.Ok)]
        public async Task<IActionResult> ListarTodosBooks()
        {
            ListarBooksQuery query = new();
            Response<IEnumerable<BookResponse>> result = await mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Cadastrar um book
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns></returns>
        [HttpPost("cadastro")]
        [ProducesResponseType(typeof(Response), (int)ResponseStatus.Ok)]
        [ProducesResponseType(typeof(Response), (int)ResponseStatus.BadRequest)]
        public async Task<IActionResult> CadastrarBook(
            [FromBody] CadastrarBookCommand request)
        {
            return Ok(await mediator.Send(request));
        }

        /// <summary>
        /// Atualizar um book
        /// </summary>
        /// <param name="request">Request de atualização</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Response<CadastrarBookResponse>), (int)ResponseStatus.Ok)]
        [ProducesResponseType(typeof(Response<CadastrarBookResponse>), (int)ResponseStatus.BadRequest)]
        public async Task<IActionResult> AtualizarBook(
            [FromRoute] string id,
            [FromBody] AtualizarBookCommand request)
        {
            request.Id = id;
            return Ok(await mediator.Send(request));
        }

        /// <summary>
        /// Remover um livro
        /// </summary>
        /// <param name="id">ID do livro</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response<CadastrarBookResponse>), (int)ResponseStatus.Ok)]
        [ProducesResponseType(typeof(Response<CadastrarBookResponse>), (int)ResponseStatus.BadRequest)]
        public async Task<IActionResult> RemoverBook(
            [FromRoute] string id)
        {
            RemoverBookCommand command = new()
            {
                Id = id
            };

            return Ok(await mediator.Send(command));
        }
    }
}