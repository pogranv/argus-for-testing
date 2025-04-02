using ProcessesApi.External.Impl;
using ProcessesApi.External.Interfaces;
using ProcessesApi.External.Mocks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProxyApi.Middlewares;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.WebHost.UseUrls("http://*:5000");

#region Swagger Configuration
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo { });
    
    // swagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
    //     $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

    swagger.OperationFilter<SwaggerFilter>();
    // To Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
#endregion

// Add this line to register controllers
builder.Services.AddControllers();
builder.Services.AddHttpClient();

bool useMock = true;

if (useMock) {
    builder.Services.AddScoped<IUserService, UserServiceMock>();
} else {
    builder.Services.AddScoped<IUserService, UserService>();
}

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();
app.Run();


// Прокси-обработчик
// async Task ProxyHandler(HttpContext context)
// {
//     // if (!context.User.Identity.IsAuthenticated)
//     // {
//     //     context.Response.StatusCode = 401;
//     //     return;
//     // }

//     var clientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
//     var client = clientFactory.CreateClient();

//     var targetService = GetTargetService(context.Request.Path);
//     if (string.IsNullOrEmpty(targetService))
//     {
//         context.Response.StatusCode = 404;
//         return;
//     }

//     var targetUri = new Uri(new Uri(targetService), context.Request.Path);
    
//     var request = new HttpRequestMessage(
//         new HttpMethod(context.Request.Method),
//         targetUri
//     );

//     foreach (var header in context.Request.Headers)
//     {
//         if (!request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
//         {
//             request.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
//         }
//     }

//     if (context.Request.ContentLength > 0)
//     {
//         request.Content = new StreamContent(context.Request.Body);
//     }

//     try
//     {
//         var response = await client.SendAsync(request);
//         context.Response.StatusCode = (int)response.StatusCode;
        
//         foreach (var header in response.Headers)
//         {
//             context.Response.Headers[header.Key] = header.Value.ToArray();
//         }

//         await response.Content.CopyToAsync(context.Response.Body);
//     }
//     catch
//     {
//         context.Response.StatusCode = 503;
//     }
// }

// string GetTargetService(PathString path)
// {
//     return path.StartsWithSegments("/sensors") ? "http://sensors-api:5003" :
//            path.StartsWithSegments("/processes") ? "http://processes-api:5002" :
//            path.StartsWithSegments("/statuses") ? "http://statuses-api:5001" : null;
// }