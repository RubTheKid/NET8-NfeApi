using Microsoft.EntityFrameworkCore;
using Nfe.Infrastructure.Data;
using Nfe.Domain.Contracts.Repositories;
using Nfe.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NfeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<INfeRepository, NfeRepository>();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Nfe.Application.Features.Clientes.Command.Create.CreateClienteHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(Nfe.Application.Features.NotaFiscal.Query.GetNfeById.GetNfeByIdHandler).Assembly);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
