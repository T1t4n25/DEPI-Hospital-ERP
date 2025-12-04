using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using HospitalERP.API.Common.Swagger;
using HospitalERP.API.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Hospital ERP API",
        Version = "v1",
        Description = "API for Hospital ERP System"
    });

    // Define the JWT security scheme
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Description = "Enter your JWT token in the text input below.\n\nExample: \"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\""
    });

    // Add the security requirement - using function overload
    options.AddSecurityRequirement((doc) =>
    {
        var schemeRef = new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", doc);
        return new Microsoft.OpenApi.OpenApiSecurityRequirement
        {
            { schemeRef, new List<string>() }
        };
    });

    // Add operation filter for response examples
    options.OperationFilter<SwaggerExampleFilter>();
    
    // Enable annotations for better Swagger documentation
    options.EnableAnnotations();
});

// Entity Framework Core
// Read connection string from environment variables or configuration
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
var dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "HospitalERP";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "YourStrong!Password123";

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? $"Server={dbServer},{dbPort};Database={dbDatabase};User Id={dbUser};Password={dbPassword};TrustServerCertificate=true;MultipleActiveResultSets=true;";

builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 100;
        options.QueueLimit = 100;
    });
});

// TODO: Add memory cache for endpoints that has frequent repeating requests
// builder.Services.AddMemoryCache();

// MediatR (CQRS)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Keycloak JWT Authentication
var keycloakAuthority = Environment.GetEnvironmentVariable("KEYCLOAK_AUTHORITY") 
    ?? builder.Configuration["Keycloak:Authority"] 
    ?? "http://localhost:8080/realms/hospital-erp";
var keycloakAudience = Environment.GetEnvironmentVariable("KEYCLOAK_AUDIENCE") 
    ?? builder.Configuration["Keycloak:Audience"] 
    ?? "hospital-erp-api";
var requireHttpsMetadata = Environment.GetEnvironmentVariable("KEYCLOAK_REQUIRE_HTTPS") != null 
    ? bool.Parse(Environment.GetEnvironmentVariable("KEYCLOAK_REQUIRE_HTTPS")!) 
    : builder.Configuration.GetValue<bool>("Keycloak:RequireHttpsMetadata", false);

// Log Keycloak configuration for debugging
Console.WriteLine("\n=== Keycloak JWT Configuration ===");
Console.WriteLine($"Authority: {keycloakAuthority}");
Console.WriteLine($"Configured Audience: {keycloakAudience}");
Console.WriteLine($"RequireHttpsMetadata: {requireHttpsMetadata}");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = keycloakAuthority;
    options.RequireHttpsMetadata = requireHttpsMetadata;
    
    // Handle single or multiple audiences
    var audiences = keycloakAudience.Contains(",")
        ? keycloakAudience.Split(',').Select(a => a.Trim()).ToArray()
        : new[] { keycloakAudience };
    
    // Log audience configuration
    Console.WriteLine($"\n=== JWT Bearer Audience Configuration ===");
    Console.WriteLine($"Parsed Audiences: [{string.Join(", ", audiences)}]");
    Console.WriteLine($"Number of audiences: {audiences.Length}");
    
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        // Set ValidAudience for single audience, ValidAudiences for multiple
        ValidAudience = audiences.Length == 1 ? audiences[0] : null,
        ValidAudiences = audiences.Length > 1 ? audiences : null
    };
    
    // Log what's set in TokenValidationParameters
    if (options.TokenValidationParameters.ValidAudience != null)
    {
        Console.WriteLine($"ValidAudience (single): {options.TokenValidationParameters.ValidAudience}");
    }
    if (options.TokenValidationParameters.ValidAudiences != null)
    {
        Console.WriteLine($"ValidAudiences (multiple): [{string.Join(", ", options.TokenValidationParameters.ValidAudiences)}]");
    }
    
    // Also set on options.Audience for consistency
    if (audiences.Length == 1)
    {
        options.Audience = audiences[0];
        Console.WriteLine($"options.Audience: {options.Audience}");
    }
    
    // Add event handlers to log incoming token details for debugging
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(context.Exception, "JWT Authentication failed");
            if (context.Exception is Microsoft.IdentityModel.Tokens.SecurityTokenInvalidAudienceException)
            {
                logger.LogError("Token audience validation failed");
                // Try to extract audience from token if possible
                try
                {
                    var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    if (!string.IsNullOrEmpty(token))
                    {
                        var parts = token.Split('.');
                        if (parts.Length >= 2)
                        {
                            var payload = parts[1];
                            var padding = 4 - payload.Length % 4;
                            if (padding != 4) payload += new string('=', padding);
                            var decoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                            var json = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(decoded);
                            if (json.TryGetProperty("aud", out var audElement))
                            {
                                var tokenAudience = audElement.ValueKind == System.Text.Json.JsonValueKind.String 
                                    ? audElement.GetString() 
                                    : string.Join(", ", audElement.EnumerateArray().Select(a => a.GetString()));
                                logger.LogError("Token audience (from token): {TokenAudience}", tokenAudience);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Could not extract audience from token for debugging");
                }
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            if (context.Principal?.Identity is System.Security.Claims.ClaimsIdentity claimsIdentity)
            {
                // Extract roles from realm_access claim (Keycloak format)
                var realmAccessClaim = claimsIdentity.FindFirst("realm_access");
                if (realmAccessClaim != null)
                {
                    try
                    {
                        var realmAccess = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(realmAccessClaim.Value);
                        if (realmAccess.TryGetProperty("roles", out var roles))
                        {
                            var roleList = new List<string>();
                            foreach (var role in roles.EnumerateArray())
                            {
                                var roleValue = role.GetString();
                                if (!string.IsNullOrEmpty(roleValue))
                                {
                                    // Add role as a claim with the standard Role claim type
                                    claimsIdentity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, roleValue));
                                    roleList.Add(roleValue);
                                }
                            }
                            logger.LogInformation("Extracted {RoleCount} roles from token: {Roles}", roleList.Count, string.Join(", ", roleList));
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Could not extract roles from realm_access claim");
                    }
                }
                
                // Extract roles from resource_access claim if present (client-specific roles)
                var resourceAccessClaim = claimsIdentity.FindFirst("resource_access");
                if (resourceAccessClaim != null)
                {
                    try
                    {
                        var resourceAccess = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(resourceAccessClaim.Value);
                        foreach (var clientProperty in resourceAccess.EnumerateObject())
                        {
                            if (clientProperty.Value.TryGetProperty("roles", out var clientRoles))
                            {
                                foreach (var role in clientRoles.EnumerateArray())
                                {
                                    var roleValue = role.GetString();
                                    if (!string.IsNullOrEmpty(roleValue))
                                    {
                                        // Add client-specific role as claim
                                        claimsIdentity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, roleValue));
                                        logger.LogDebug("Added client role from {Client}: {Role}", clientProperty.Name, roleValue);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Could not extract roles from resource_access claim");
                    }
                }
                
                var tokenAudience = claimsIdentity.FindFirst("aud")?.Value;
                var allRoles = claimsIdentity.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value);
                logger.LogInformation("Token validated successfully. Token audience: {TokenAudience}, Roles: [{Roles}]", 
                    tokenAudience ?? "(null)", 
                    string.Join(", ", allRoles));
            }
            return Task.CompletedTask;
        }
    };
    
    Console.WriteLine("=== End JWT Configuration ===\n");
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Seed database on startup (only in development)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<HospitalDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Connection String: {ConnectionString}", connectionString);
    // Retry connection with exponential backoff
    var maxRetries = 10;
    var delay = 2000; // Start with 2 seconds
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            logger.LogInformation("Attempting to connect to database (attempt {Attempt}/{MaxRetries})...", i + 1, maxRetries);
            await context.Database.CanConnectAsync();
            logger.LogInformation("Database connection successful!");
            break;
        }
        catch (Exception ex) when (i < maxRetries - 1)
        {
            logger.LogWarning(ex, "Database not ready yet. Retrying in {Delay}ms...", delay);
            await Task.Delay(delay);
            delay = Math.Min(delay * 2, 10000); // Exponential backoff, max 10 seconds
        }
    }
    
    try
    {
        await DbInitializer.SeedAsync(context);
        logger.LogInformation("Database seeding completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hospital ERP API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
