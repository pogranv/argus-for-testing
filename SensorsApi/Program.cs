using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using SensorsApi.External.Impl;
using SensorsApi.External.Interfaces;
using SensorsApi.External.Mocks;
using SensorsApi.Models;
using SensorsApi.src.Repositories;
using System.Reflection;

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
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

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

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
