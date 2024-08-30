using MediatR;
using Microsoft.Extensions.Options;
using MongoDBSample.Application.Abstractions.Data;
using MongoDBSample.Application.Abstractions.Handlers;
using MongoDBSample.Application.Books.Commands;
using MongoDBSample.Application.Books.Data;
using MongoDBSample.Application.Books.Queries;
using MongoDBSample.Application.Books.Validator;
using MongoDBSample.Domain.Model.Books;
using MongoDBSample.Domain.Model.UnitOfWork;
using MongoDBSample.Domain.Services.Books;
using MongoDBSample.Infrastructure.Respositories.Books;
using MongoDBSample.Infrastructure.Respostories.Books;
using MongoDBSample.Infrastructure.Respostories.Context;
using System.Reflection;

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
                cfg.RegisterServicesFromAssembly(typeof(CadastrarBookValidator).Assembly);
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            //Services
            services.AddScoped<IRequestHandler<CadastrarBookCommand, Response<CadastrarBookResponse>>, CadastrarBookService>();
            services.AddScoped<IRequestHandler<AtualizarBookCommand, Response<CadastrarBookResponse>>, AtualizarBookService>();
            services.AddScoped<IRequestHandler<RemoverBookCommand, Response<CadastrarBookResponse>>, RemoverBookService>();

            //Repositories
            services.AddScoped<IRequestHandler<ListarBooksPorIdQuery, Response<BookResponse>>, ListarBooksPorIdRepository>();
            services.AddScoped<IRequestHandler<ListarBooksQuery, Response<IEnumerable<BookResponse>>>, ListarBooksRepository>();

            return services;
        }

        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            return services;
        }

        public static WebApplication UseCustomMiddleware(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }
    }
}
