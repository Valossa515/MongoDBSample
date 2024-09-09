using Microsoft.OpenApi.Models;
using MongoDBSample.API;
using MongoDBSample.Application;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationValidators();
builder.Services.AddMongoDBContext(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddMediatRServices();
builder.Services.AddCustomControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthenticationExtension(builder.Configuration);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MongoDBSample API", Version = "v1" });

    // Define o esquema de seguran�a
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer"
    });

    // Requer o esquema de seguran�a para todas as opera��es
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCustomMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.Run();