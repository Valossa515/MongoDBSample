using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Users.Commands;
using MongoDBSample.Application.Users.Data;
using MongoDBSample.Domain.Model.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class LoginService(
    UserManager<ApplicationUser> userManager)
        : CommandHandler<LoginCommand, LoginResponse>
{
    protected override async Task<Response<LoginResponse>> Execute(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email não pode ser nulo ou vazio");
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Senha não pode ser nula ou vazia");
            }

            ApplicationUser? user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return BadRequest("Usuário não encontrado");
            }

            bool passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                return BadRequest("Senha incorreta");
            }

            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            IList<string> roles = await userManager.GetRolesAsync(user);
            IEnumerable<Claim> roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
            claims.AddRange(roleClaims);

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("1swek3u4uo2u4a6e1swek3u4uo2u4a6e"));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
            DateTime expires = DateTime.UtcNow.AddMinutes(30);

            JwtSecurityToken token = new(
               issuer: "http://localhost:5124/",
               audience: "http://localhost:3000/",
               claims: claims,
               expires: expires,
               signingCredentials: creds
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return MapearResponseWitToken(true, "Usuário autenticado com sucesso", user.Id, tokenString);

        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest($"Erro de argumento fora do intervalo: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro inesperado: {ex.Message}");
        }
    }

    private Response<LoginResponse> MapearResponseWitToken(
        bool sucesso,
        string mensagem,
        Guid? id = null,
        string? token = null)
    {
        LoginResponse response = new()
        {
            Id = id,
            Message = mensagem,
            Success = sucesso,
            Token = token
        };

        return Ok(response);
    }

    private Response<LoginResponse> MapearResponse(
       bool sucesso,
       string mensagem,
       Guid? id = null)
    {
        LoginResponse response = new()
        {
            Id = id,
            Message = mensagem,
            Success = sucesso
        };

        return Ok(response);
    }
}
