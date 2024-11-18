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
builder.Logging.AddFile("Logs/log.txt");
builder.Services.AddHostedJobs();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MongoDBSample API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer"
    });

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

app.UseCustomMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.Run();