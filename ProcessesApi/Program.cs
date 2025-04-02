using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ProcessesApi.External.Impl;
using ProcessesApi.External.Interfaces;
using ProcessesApi.External.Mocks;
using ProcessesApi.Models;
using ProcessesApi.Repositories;
using ProcessesApi.Services;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Development,
    Args = args
});

builder.WebHost.UseUrls("http://*:5002");

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});
builder.Services.AddControllers();

bool useMocks = true;

if (useMocks)
{
    // Регистрируем мок
    // builder.Services.AddSingleton<IGraphService, GraphServiceMock>();
    builder.Services.AddHttpClient<IGraphService, GraphService>(client => 
    {
        client.BaseAddress = new Uri("http://statuses_api:5001/");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    }); 
    
    builder.Services.AddSingleton<IDutyService, DutyServiceMock>();
    builder.Services.AddSingleton<INotificationService, NotificationServiceMock>();
    builder.Services.AddSingleton<IUserService, UserServiceMock>();
}
else
{
    // Регистрируем реальный сервис с HttpClient
    builder.Services.AddHttpClient<IGraphService, GraphService>(client => 
    {
        client.BaseAddress = new Uri("http://statuses_api:5001/");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    }); 
    builder.Services.AddHttpClient<IDutyService, DutyService>(client => 
    {
        client.BaseAddress = new Uri("TODO");
    });
    builder.Services.AddHttpClient<INotificationService, NotificationService>(client => 
    {
        client.BaseAddress = new Uri("TODO");
    });
    builder.Services.AddHttpClient<IUserService, UserService>(client => 
    {
        client.BaseAddress = new Uri("TODO");
    });
}

builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ITicketService, TicketService>();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Строка подключения не установлена!");
}
Console.WriteLine(connectionString);

builder.Services.AddDbContext<ProcessesDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
        npgsqlOptions.MapEnum<Priority>("ticket_priority_type")));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (context, next) => {
    if (context.Request.Path.StartsWithSegments("/swagger") || 
        context.Request.Path.StartsWithSegments("/swagger-ui"))
    {
        await next();
        return;
    }
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

app.MapControllers();

app.Run();