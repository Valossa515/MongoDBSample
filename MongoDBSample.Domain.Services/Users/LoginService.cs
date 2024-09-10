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

public class LoginService
    : CommandHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public LoginService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    protected override async Task<Response<LoginResponse>> Execute(
    LoginCommand request,
    CancellationToken cancellationToken)
    {
        try
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return MapearResponse(false, "Usuário não encontrado");
            }

            bool passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                return MapearResponse(false, "Senha incorreta");
            }

            List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            IList<string> roles = await _userManager.GetRolesAsync(user);
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
            return MapearResponse(false, $"Erro de argumento fora do intervalo: {ex.Message}");
        }
        catch (Exception ex)
        {
            return MapearResponse(false, $"Erro inesperado: {ex.Message}");
        }
    }

    private Response<LoginResponse> MapearResponseWitToken(
        bool sucesso,
        string mensagem,
        Guid? id = null,
        string token = null)
    {
        LoginResponse response = new()
        {
            Id = id,
            Message = mensagem,
            Success = sucesso,
            Token = token
        };

        return sucesso ? Ok(response) : BadRequest("Erro na requisição");
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

        return sucesso ? Ok(response) : BadRequest("Erro na requisição");
    }
}
