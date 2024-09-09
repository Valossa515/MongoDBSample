using Microsoft.AspNetCore.Identity;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Users.Commands;
using MongoDBSample.Application.Users.Data;
using MongoDBSample.Domain.Model.UnitOfWork;
using MongoDBSample.Domain.Model.Users;

namespace MongoDBSample.Domain.Services.Users
{
    public class CadastrarUserService
        : CommandHandler<CadastrarUserCommand, CadastrarUserResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public CadastrarUserService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        protected override async Task<Response<CadastrarUserResponse>> Execute(
            CadastrarUserCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                ApplicationUser user = new()
                {
                    UserName = request.Name,
                    Email = request.Email,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                };

                IdentityResult result = await userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    // Capture os erros de identidade e retorne uma resposta adequada
                    string errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                    return MapearResponse(false, $"Falha ao cadastrar o usuário: {errorMessages}");
                }

                IdentityResult addUserToRoleResult = await userManager.AddToRoleAsync(user, "USER");

                if (!addUserToRoleResult.Succeeded)
                {
                    // Capture os erros de identidade e retorne uma resposta adequada
                    string errorMessages = string.Join(", ", addUserToRoleResult.Errors.Select(e => e.Description));
                    return MapearResponse(false, $"Falha ao adicionar o usuário ao grupo: {errorMessages}");
                }

                // Commit após criar o usuário
                await unitOfWork.AddAsync(result);
                StatusCommit commitStatus = await unitOfWork.Commit(cancellationToken);

                if (commitStatus == StatusCommit.Sucesso)
                {
                    return MapearResponse(true, "Usuário cadastrado com sucesso", user.Id);
                }
                else
                {
                    return MapearResponse(false, "Falha ao cadastrar o usuário no banco de dados");
                }
            }
            catch (Exception ex)
            {
                // Capture qualquer exceção inesperada
                return MapearResponse(false, $"Erro inesperado: {ex.Message}");
            }
        }

        private Response<CadastrarUserResponse> MapearResponse(
           bool sucesso,
           string mensagem,
           Guid? id = null)
        {
            CadastrarUserResponse response = new()
            {
                Id = id,
                Sucesso = sucesso,
                Mensagem = mensagem
            };

            return Ok(response);
        }
    }
}