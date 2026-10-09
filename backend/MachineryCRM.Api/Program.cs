using Asp.Versioning;
using MachineryCRM.Application.Interfaces;
using MachineryCRM.Application.Services;
using MachineryCRM.Domain.Interfaces;
using MachineryCRM.Infrastructure.Data;
using MachineryCRM.Infrastructure.Repositories;
using MachineryCRM.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurações de Banco de Dados
ConfigureDatabase(builder);

// 2. Injeção de Dependências (Services & Repositories)
ConfigureDependencies(builder.Services);

// 3. Autenticação e Autorização (JWT)
ConfigureAuthentication(builder);

// 4. Configurações da API (Controllers, Versioning, CORS, Swagger)
ConfigureApi(builder.Services);

var app = builder.Build();

// 5. Pipeline de Requisições HTTP
ConfigurePipeline(app);

app.Run();

// ============================================================================
// MÉTODOS DE CONFIGURAÇÃO (Local Functions)
// ============================================================================

void ConfigureDatabase(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Database connection string is missing.");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
}

void ConfigureDependencies(IServiceCollection services)
{
    // Unit of Work
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    // Repositories
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<ICustomerRepository, CustomerRepository>();
    services.AddScoped<IMachineRepository, MachineRepository>();
    services.AddScoped<IAppUserRepository, AppUserRepository>();

    // Services
    services.AddScoped<ICustomerService, CustomerService>();
    services.AddScoped<IMachineService, MachineService>();
    services.AddScoped<IAppUserService, AppUserService>();
    services.AddScoped<IMaintenanceService, MaintenanceService>();
    services.AddScoped<ITransferHistoryService, TransferHistoryService>();
    services.AddScoped<ICommunicationService, CommunicationService>();

    // Security
    services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured.");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

    builder.Services.AddAuthorization();
}

void ConfigureApi(IServiceCollection services)
{
    services.AddControllers()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    services.AddEndpointsApiExplorer();

    services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins("http://localhost:5173") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "MachineryCRM API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
    });
}

void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            var descriptions = app.DescribeApiVersions();
            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = $"MachineryCRM API {description.GroupName.ToUpperInvariant()}";
                c.SwaggerEndpoint(url, name);
            }
            c.RoutePrefix = string.Empty; 
        });

        ExecuteDbSeeder(app);
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors("AllowFrontend"); 
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}

void ExecuteDbSeeder(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    try
    {
        DbSeeder.Seed(dbContext, app.Configuration);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}