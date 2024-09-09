using MongoDBSample.API;
using MongoDBSample.Application;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationValidators();
builder.Services.AddAuthenticationJwt(builder.Configuration);
builder.Services.AddMongoDBContext(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddMediatRServices();
builder.Services.AddCustomControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCustomMiddleware();

app.Run();