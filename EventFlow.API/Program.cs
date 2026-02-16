using EventFlow.API.Domain;
using EventFlow.API.DTOs;
using EventFlow.API.Infrastructure.DataAcess;
using EventFlow.API.Infrastructure.Security;
using EventFlow.API.Infrastructure.Settings;
using EventFlow.API.Interfaces.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// ----------------- SERVICE INJECTION -----------------
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddDbContext<IEventFlowDbContext, EventFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// ----------------- AUTO MIGRATION -----------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EventFlowDbContext>();
    db.Database.Migrate(); // applies pending migrations + seeds
}

// ----------------- SWAGGER -----------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ----------------- ROUTES -----------------
app.MapGet("/", () => "Hello World!"); // minimal API route

app.MapPost("/api/auth/login", (LoginDTO login, IJwtService jwtService) =>
{
    // Validation
    if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
        return Results.BadRequest(new { error = "Email and password are required" });

    try
    {
        // TODO: Validate credentials against database here
        // For now, just generating token
        var token = jwtService.generateToken(new User
        {
            Email = login.Email,
            Name = login.Email,
            Role = "User"
        });

        return Results.Ok(new { token, expiresIn = 3600 });
    }
    catch (Exception ex)
    {
        // Log the exception here
        return Results.Problem("An error occurred during login", statusCode: 500);
    }
})
.WithName("Login")
.WithTags("Authentication") //for swagger grouping
.Produces(200)
.Produces(400)
.Produces(500);

app.Run();
