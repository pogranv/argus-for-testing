using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using StatusesApi.External.Impl;
using StatusesApi.External.Interfaces;
using StatusesApi.External.Mocks;
using StatusesApi.Repositories;
using StatusesApi.Services;


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Development,
    Args = args
});

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.WebHost.UseUrls("http://*:5001");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

bool useMocks = true;

if (useMocks)
{
    // Регистрируем мок
    builder.Services.AddSingleton<IUserService, UserServiceMock>();
    builder.Services.AddSingleton<IDutyService, DutyServiceMock>();
}
else
{
    // Регистрируем реальный сервис с HttpClient
    builder.Services.AddHttpClient<IUserService, UserService>(client => 
    {
        client.BaseAddress = new Uri("TODO");
    });
    builder.Services.AddHttpClient<IDutyService, DutyService>(client => 
    {
        client.BaseAddress = new Uri("TODO");
    }); 
}


string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Строка подключения не установлена!");
}
Console.WriteLine(connectionString);
builder.Services.AddDbContext<StatusDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
        npgsqlOptions.MapEnum<NotificationType>("status_type")));

builder.Services.AddScoped<IStatusService, StatusService>();

// Регистрация сервисов
// builder.Services.AddScoped<IStatusService, StatusService>();
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddHttpClient<IDutyServiceClient, DutyServiceClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();