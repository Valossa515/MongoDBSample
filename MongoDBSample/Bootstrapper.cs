using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDBSample.API.Behaviors;
using MongoDBSample.API.Exceptions;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Books.Commands;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Application.Books.Queries;
using MongoDBSample.Application.Users.Commands;
using MongoDBSample.Application.Users.Data;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Domain.Model.UnitOfWork;
using MongoDBSample.Domain.Model.Users;
using MongoDBSample.Domain.Services.Books;
using MongoDBSample.Domain.Services.Users;
using MongoDBSample.Infrastructure.Respositories.Books;
using MongoDBSample.Infrastructure.Respostories.Books;
using MongoDBSample.Infrastructure.Respostories.Context;
using System.Reflection;
using System.Text;

namespace MongoDBSample.API
{
    public static partial class Bootstrapper
    {
        public static IServiceCollection AddMongoDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BookStoreDatabaseSettings>(configuration.GetSection("BookStoreDatabase"));

            services.AddSingleton<MongoDBContext>(provider =>
            {
                BookStoreDatabaseSettings settings = provider.GetRequiredService<IOptions<BookStoreDatabaseSettings>>().Value;
                return new MongoDBContext(settings.ConnectionString, settings.DatabaseName);
            });

            // Register IMongoCollection<Book>
            services.AddSingleton(provider =>
            {
                MongoDBContext context = provider.GetRequiredService<MongoDBContext>();
                return context.GetCollection<Book>("BooksCollectionName");
            });

            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<MongoDBContext>());

            MongoDbConfig? identitySettings = configuration.GetSection("MongoDbConfig").Get<MongoDbConfig>();
            services.AddIdentity<ApplicationUser, ApplicationRole>()
               .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
               (
                   identitySettings?.ConnectionString, identitySettings?.DatabaseName
               )
               .AddDefaultTokenProviders();

            return services;
        }
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CommandHandler<CadastrarBookCommand, CadastrarBookResponse>).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(QueryHandler<,>).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ListarBooksPorIdRepository).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ListarBooksRepository).Assembly);
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //Services
            services.AddScoped<IRequestHandler<CadastrarBookCommand, Response<CadastrarBookResponse>>, CadastrarBookService>();
            services.AddScoped<IRequestHandler<AtualizarBookCommand, Response<CadastrarBookResponse>>, AtualizarBookService>();
            services.AddScoped<IRequestHandler<RemoverBookCommand, Response<CadastrarBookResponse>>, RemoverBookService>();
            services.AddScoped<IRequestHandler<CadastrarUserCommand, Response<CadastrarUserResponse>>, CadastrarUserService>();
            services.AddScoped<IRequestHandler<LoginCommand, Response<LoginResponse>>, LoginService>();

            //Repositories
            services.AddScoped<IRequestHandler<ListarBooksPorIdQuery, Response<BookResponse>>, ListarBooksPorIdRepository>();
            services.AddScoped<IRequestHandler<ListarBooksQuery, Response<PaginatedResponse<BookResponse>>>, ListarBooksRepository>();

            return services;
        }

        public static IServiceCollection AddAuthenticationJwt(this IServiceCollection services, IConfiguration configuration)
        {
            // Adiciona o serviço de autenticação JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            // Add CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });


            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            return services;
        }

        public static WebApplication UseCustomMiddleware(this WebApplication app)
        {
            app.UseCors("AllowAll");
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}