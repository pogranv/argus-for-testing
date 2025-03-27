using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using SensorsApi.External.Impl;
using SensorsApi.External.Interfaces;
using SensorsApi.External.Mocks;
using SensorsApi.Models;
using SensorsApi.src.Repositories;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Development,
    Args = args
});

builder.WebHost.UseUrls("http://*:5003");

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// var useMocks = true;
// if (useMocks)
// {
//     // Регистрируем мок
//     builder.Services.AddSingleton<IProcessesService, ProcessesServiceMock>();
// }
// else
// {
//     // Регистрируем реальный сервис с HttpClient
//     builder.Services.AddHttpClient<IProcessesService, ProcessesService>(client => 
//     {
//         client.BaseAddress = new Uri("http://processes_api:5002/");
//         client.DefaultRequestHeaders.Accept.Add(
//             new MediaTypeWithQualityHeaderValue("application/json"));
//     });
// }
builder.Services.AddHttpClient<IProcessesService, ProcessesService>(client => 
    {
        client.BaseAddress = new Uri("http://processes_api:5002/");
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    });

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Строка подключения не установлена!");
}
Console.WriteLine(connectionString);
builder.Services.AddDbContext<SensorsDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
        npgsqlOptions.MapEnum<Priority>("ticket_priority_type")));

var app = builder.Build();

// Остальная конфигурация middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
