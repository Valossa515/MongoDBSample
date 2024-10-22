using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Reservations.Commands;
using MongoDBSample.Application.Reservations.Data;
using MongoDBSample.Domain.Model.Reservations;
using MongoDBSample.Domain.Model.UnitOfWork;

namespace MongoDBSample.Domain.Services.Reservations
{
    public class AtualizarReservationService(
        IUnitOfWork unitOfWork)
                : CommandHandler<AtualizarReservationCommand, CadastrarReservationResponse>
    {
        public async Task<Response<CadastrarReservationResponse>> PublicExecute(
            AtualizarReservationCommand request,
            CancellationToken cancellationToken)
        {
            return await Execute(request, cancellationToken);
        }

        protected async override Task<Response<CadastrarReservationResponse>> Execute(
            AtualizarReservationCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                return MapearResponse(false, "ID da reserva não pode ser nulo ou vazio");
            }

            Reservation? reservation = await unitOfWork.FindByIdAsync<Reservation>(request.Id);

            if (reservation == null)
            {
                return MapearResponse(false, "Reserva não encontrado", request.Id);
            }

            reservation.AtualizarDados(request.Status);

            await unitOfWork.UpdateAsync(reservation);

            StatusCommit commitStatus = await unitOfWork.Commit(cancellationToken);

            if (commitStatus == StatusCommit.Sucesso)
            {
                return MapearResponse(true, "Reserva atualizado com sucesso", reservation.Id);
            }

            return MapearResponse(false, "Falha ao atualizar a reserva", reservation.Id);
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
