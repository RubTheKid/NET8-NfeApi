using Nfe.Domain.ValueObjects;

namespace Nfe.Application.Features.Clientes.Query.GetAllClientes;

public class GetAllClientesResponse
{
    public IEnumerable<Clientes> Clientes { get; set; } = new List<Clientes>();
    public int Total { get; set; }
}

public class Clientes
{
    public Guid Id { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public Endereco Endereco { get; set; } = null!;
}