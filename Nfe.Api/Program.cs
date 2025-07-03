using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Nfe.Application.Services;
using Nfe.Auth;
using Nfe.Auth.Models;
using Nfe.Domain.Contracts.Repositories;
using Nfe.Infrastructure.Data;
using Nfe.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// JWT Authentication from Auth library
builder.Services.AddJwtAuthentication(builder.Configuration);

// Swagger Configuration with JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NFe API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<NfeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<INfeRepository, NfeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddScoped<INfeXmlService, NfeXmlService>();
builder.Services.AddScoped<IMessageService, MessageService>();

// Background service
builder.Services.AddHostedService<NfeConsumerService>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Nfe.Application.Features.Clientes.Command.Create.CreateClienteHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(Nfe.Application.Features.NotaFiscal.Query.GetNfeById.GetNfeByIdHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(Nfe.Application.Features.NotaFiscal.Command.SendNfeToAuthorization.SendNfeToAuthorizationHandler).Assembly);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var passwordHashService = scope.ServiceProvider.GetRequiredService<IPasswordHashService>();

    var adminEmail = "admin@nfe.com";
    var adminPassword = "Admin123!";

    var existingUser = await userRepository.GetByEmail(adminEmail);
    if (existingUser == null)
    {
        var hashedPassword = passwordHashService.HashPassword(adminPassword);
        var adminUser = new User(adminEmail, hashedPassword);

        await userRepository.Create(adminUser);
        await userRepository.SaveChangesAsync();
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
