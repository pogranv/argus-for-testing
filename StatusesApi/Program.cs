using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API для создания статусов", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName);
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
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
builder.Services.AddHttpContextAccessor();

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

app.Use(async (context, next) => {
    if (!context.Request.Headers.TryGetValue("UserId", out var userIdHeader))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("User ID header is required");
        return;
    }

    if (!long.TryParse(userIdHeader, out var userId))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Invalid User ID format");
        return;
    }

    if (!context.Request.Headers.TryGetValue("RobotId", out var robotIdHeader))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Robot ID header is required");
        return;
    }

    if (!long.TryParse(robotIdHeader, out var robotId))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Invalid Robot ID format");
        return;
    }

    context.Items["UserId"] = userId;
    context.Items["RobotId"] = robotId;
    await next();
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();