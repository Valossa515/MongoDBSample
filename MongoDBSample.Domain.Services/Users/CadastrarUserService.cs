﻿using Microsoft.AspNetCore.Identity;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Users.Commands;
using MongoDBSample.Application.Users.Data;
using MongoDBSample.Domain.Model.UnitOfWork;
using MongoDBSample.Domain.Model.Users;

namespace MongoDBSample.Domain.Services.Users
{
    public class CadastrarUserService(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
                : CommandHandler<CadastrarUserCommand, CadastrarUserResponse>
    {
        protected override async Task<Response<CadastrarUserResponse>> Execute(
            CadastrarUserCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Password))
            {
                return MapearResponse(false, "Password cannot be null or empty");
            }

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
                    string errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                    return MapearResponse(false, $"Falha ao cadastrar o usuário: {errorMessages}");
                }

                List<string> rolesParaAtribuir = DeterminarRolesParaUsuario(user);

                IdentityResult addUserToRolesResult = await userManager.AddToRolesAsync(user, rolesParaAtribuir);

                if (!addUserToRolesResult.Succeeded)
                {
                    string errorMessages = string.Join(", ", addUserToRolesResult.Errors.Select(e => e.Description));
                    return MapearResponse(false, $"Falha ao adicionar o usuário aos grupos: {errorMessages}");
                }

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
                return MapearResponse(false, $"Erro inesperado: {ex.Message}");
            }
        }

        private static List<string> DeterminarRolesParaUsuario(ApplicationUser user)
        {
            List<string> roles = [];

            if (user.Email?.EndsWith("@empresa.com") == true)
            {
                roles.Add("ADMIN");
            }

            roles.Add("USER");

            return roles;
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