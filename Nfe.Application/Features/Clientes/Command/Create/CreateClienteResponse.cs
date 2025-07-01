namespace Nfe.Application.Features.Clientes.Command.Create.Response;

public sealed record CreateClienteResponse
{
    public Guid Id { get; init; }
    public string RazaoSocial { get; init; }
    public string NomeFantasia { get; init; }
    public string Cnpj { get; init; }
    public string Message { get; init; }
}
