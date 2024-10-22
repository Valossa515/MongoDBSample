using Microsoft.AspNetCore.Identity;
using MongoDBSample.API.Controllers.Reservations.enums;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Reservations.Commands;
using MongoDBSample.Application.Reservations.Data;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Domain.Model.Reservations;
using MongoDBSample.Domain.Model.UnitOfWork;
using MongoDBSample.Domain.Model.Users;

namespace MongoDBSample.Domain.Services.Reservations
{
    public class CadastrarReservationService(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
                : CommandHandler<CadastrarReservationCommand, CadastrarReservationResponse>
    {
        public async Task<Response<CadastrarReservationResponse>> PublicExecute(
            CadastrarReservationCommand request,
            CancellationToken cancellationToken)
        {
            return await Execute(request, cancellationToken);
        }

        protected async override Task<Response<CadastrarReservationResponse>> Execute(
            CadastrarReservationCommand request, CancellationToken cancellationToken)
        {
            foreach (string bookId in request.BookIds)
            {
                Book? book = await unitOfWork.FindByIdAsync<Book>(bookId);
                if (book == null)
                {
                    return MapearResponse(false, $"Book com ID {bookId} não encontrado");
                }
            }

            ApplicationUser? user = await userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return MapearResponse(false, "Usuário não encontrado");
            }

            DateTime returnDate = request.ReservationDate.AddDays(7);



            Reservation reservation = new()
            {
                BookIds = request.BookIds,
                UserId = user.Id.ToString(),
                UserName = user.UserName,
                ReservationDate = request.ReservationDate,
                ReturnDate = returnDate,
                Status = ReservationStatus.Active,
            };

            await unitOfWork.AddAsync(reservation);

            StatusCommit statusCommit = await unitOfWork.Commit(cancellationToken);

            return statusCommit switch
            {
                StatusCommit.Sucesso => MapearResponse(true, "Reserva cadastrada com sucesso", reservation.Id),
                _ => MapearResponse(false, "Falha ao cadastrar reserva")
            };
        }

        private Response<CadastrarReservationResponse> MapearResponse(
        bool sucesso,
        string mensagem,
        string? id = null)
        {
            CadastrarReservationResponse response = new()
            {
                Id = id,
                Sucesso = sucesso,
                Mensagem = mensagem
            };

            return Ok(response);
        }
    }
}