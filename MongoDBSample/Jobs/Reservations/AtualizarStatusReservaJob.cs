using MongoDB.Driver;
using MongoDBSample.API.Controllers.Reservations.enums;
using MongoDBSample.Domain.Model.Reservations;
using MongoDBSample.Domain.Model.UnitOfWork;

namespace MongoDBSample.API.Jobs.Reservations
{
    public class AtualizarStatusReservaJob
        : BackgroundService

    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AtualizarStatusReservaJob> _logger;

        public AtualizarStatusReservaJob(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<AtualizarStatusReservaJob> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Iniciando a execução do job de atualização de status de reserva.");

                try
                {
                    using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                    {
                        IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        List<Reservation> reservations = await unitOfWork
                            .GetRepository<Reservation>()
                            .Find(r => r.Status == ReservationStatus.Active)
                            .ToListAsync();

                        _logger.LogInformation($"{reservations.Count} reservas ativas encontradas para verificação.");

                        foreach (Reservation reservation in reservations)
                        {
                            if ((DateTime.UtcNow - reservation.ReservationDate).TotalDays > 7)
                            {
                                reservation.Status = ReservationStatus.Expired;
                                await unitOfWork.UpdateAsync(reservation);
                                _logger.LogInformation($"Reserva com ID {reservation.Id} atualizada para 'Expired'.");
                            }
                        }

                        await unitOfWork.Commit(CancellationToken.None);
                        _logger.LogInformation("As atualizações de reserva foram salvas com sucesso.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro durante a execução do job de atualização de status de reserva.");
                }

                _logger.LogInformation("Aguardando 60 segundos até a próxima execução.");
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}