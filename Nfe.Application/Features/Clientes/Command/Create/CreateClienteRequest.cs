using MediatR;
using Nfe.Application.Features.Clientes.Command.Create.Response;
using Nfe.Domain.ValueObjects;

namespace Nfe.Application.Features.Clientes.Command.Create.Request;

public sealed record CreateClienteRequest : IRequest<CreateClienteResponse>
{
    public string RazaoSocial { get; set; }
    public string NomeFantasia { get; set; }
    public string Cnpj { get; set; }
    public string? InscricaoEstadual { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public Endereco Endereco { get; set; }
}
