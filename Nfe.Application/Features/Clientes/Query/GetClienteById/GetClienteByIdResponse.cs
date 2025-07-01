using Nfe.Domain.ValueObjects;

namespace Nfe.Application.Features.Clientes.Query.GetClienteById;

public class GetClienteByIdResponse
{
    public Guid Id { get; set; }
    public string RazaoSocial { get; set; }
    public string NomeFantasia { get; set; }
    public string Cnpj { get; set; }
    public string? InscricaoEstadual { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public DateTime DataCriacao { get; set; }
    public Endereco Endereco { get; set; } = null!;
}