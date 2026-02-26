    using EventFlow.API.Domain;
    using EventFlow.API.DTOs;
    using EventFlow.API.Helpers;
    using EventFlow.API.Infrastructure.DataAcess;
    using EventFlow.API.Infrastructure.Security;
    using EventFlow.API.Infrastructure.Settings;
    using EventFlow.API.Interfaces.Repositories;
    using EventFlow.API.Interfaces.Services;
    using EventFlow.API.Repositories;
    using EventFlow.API.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using System.Text;


    var builder = WebApplication.CreateBuilder(args);

    // ----------------- SERVICE INJECTION -----------------
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
            };
        });
    builder.Services.AddAuthorization();
    builder.Services.AddSingleton<IJwtService, JwtService>();
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
    builder.Services.AddDbContext<IEventFlowDbContext, EventFlowDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // i wanna see concurrency issues.
    builder.Services.AddScoped<IUserService,UserService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<HashPasssword, HashPasssword>();



    // Add swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter your JWT token. Example: eyJhbGci..."
        });

        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });


    var app = builder.Build();

    // ----------------- AUTO MIGRATION -----------------
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<EventFlowDbContext>();
        db.Database.Migrate();
    }

    // ----------------- SWAGGER -----------------
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // ----------------- MIDDLEWARE -----------------
    app.UseAuthentication();
    app.UseAuthorization();

    // ----------------- ROUTES -----------------
    app.MapGet("/", () => "Hello World!"); // minimal API route

    app.MapPost("/api/auth/register",async (RegisterUserRequest register, IUserService _userSerive) =>
    {
        try
        {
            if (string.IsNullOrEmpty(register.FullName) ||
                string.IsNullOrEmpty(register.Email) ||
                string.IsNullOrEmpty(register.Password))
            {
                return Results.BadRequest("please fill in all the fields");
            }

            var newuser =await _userSerive.createUser(register);

            var response = new IResponseModel<User>
            {
                Data = newuser,
                Message = "user created successfully",
                Error = false
            };

            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occured while registering new user",ex);
        }
    }).WithName("Register")
    .WithTags("Authentication")
    .Produces(200)
    .Produces(400)
    .Produces(500);

    app.MapPost("/api/auth/login", async (LoginDTO login, IJwtService jwtService ,IUserService userService , IUserRepository _userRepository) =>
    {
        if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
            return Results.BadRequest("Email and password are required");

        var user = await _userRepository.GetUserByEmail(login.Email);
        if (user == null)
            return Results.NotFound("User not found");

        try
        {
            var tokenObject = await userService.loginUser(login);

            var response = new IResponseModel<LoginResponseModel>
            {
                Data = tokenObject,
                Message = "User successfully logged in",
                Error = false
            };
            return Results.Ok(response);
        }
        catch (ArgumentException ex) when (ex.Message.Contains("Passwords dont match"))
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occured during login",ex);
        }
    })
    .WithName("Login")
    .WithTags("Authentication") 
    .Produces(200)
    .Produces(400)
    .Produces(500);

    app.MapPut("/api/users/", async (UpdateUserDTO updateUser, IUserService _userService) =>
    {
        try
        {
            var updated = await _userService.UpdateUser(updateUser);
            var response = new IResponseModel<User>
            {
                Data = updated,
                Message = "User updated successfully",
                Error = false
            };
            return Results.Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    })
    .WithName("UpdateUser")
    .WithTags("Users")
    .RequireAuthorization()
    .Produces(200)
    .Produces(400)
    .Produces(500);

app.MapDelete("/api/users/{id}", async (Guid id,IUserService _userService) =>
    {
        try
        {
            var deleted = await _userService.DeleteUser(id);

            var response = new IResponseModel<bool>
            {
                Data = deleted,
                Message = deleted ? "User deleted successfully" : "User could not be deleted",
                Error = false
            };

            return Results.Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch(Exception ex)
        {
        return Results.Problem(ex.Message);
        }
    })
    .WithName("DeleteUser")
    .WithTags("User")
    .RequireAuthorization()
    .Produces(200)
    .Produces(400)
    .Produces(500);

    //app.MapPost("/api/auth/login1", async (LoginDTO login, IJwtService jwtService, IUserRepository _userRepository,HashPasssword _hash) =>
    //{
    //    var user = await _userRepository.GetUserByEmail(login.Email);
    //    if (user == null)
    //        return Results.NotFound("User not found");

    //    if(!_hash.Verify(login.Password,user.Password))
    //        return Results.BadRequest("passwords do not match");

    //    var token = jwtService.generateToken(user);

    //    var result = new LoginResponseModel
    //    {
    //        Token = token,
    //        ExpiresAt = DateTime.UtcNow.AddMinutes(15)
    //    };

    //    var response = new IResponseModel<LoginResponseModel>
    //    {
    //        Data = result,
    //        Message = "",
    //        Error = false
    //    };
    //    return Results.Ok(response);
    //})
    //.WithName("Login1")
    //.WithTags("Authentication")
    //.Produces(200)
    //.Produces(400)
    //.Produces(500);

    app.Run();
