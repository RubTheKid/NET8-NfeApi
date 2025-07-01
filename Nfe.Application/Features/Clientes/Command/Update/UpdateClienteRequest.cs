using MediatR;
using Nfe.Application.Features.Clientes.Command.Create;
using Nfe.Domain.ValueObjects;

namespace Nfe.Application.Features.Clientes.Command.Update;

public class UpdateClienteRequest : IRequest<UpdateClienteResponse>
{
    public Guid Id { get; set; }
    public string? RazaoSocial { get; set; }
    public string? NomeFantasia { get; set; }
    public string? Cnpj { get; set; }
    public string? InscricaoEstadual { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; } 
    public Endereco? Endereco { get; set; }
}
