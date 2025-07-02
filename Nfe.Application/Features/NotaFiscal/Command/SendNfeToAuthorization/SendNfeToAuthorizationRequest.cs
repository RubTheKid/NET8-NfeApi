using MediatR;

namespace Nfe.Application.Features.NotaFiscal.Command.SendNfeToAuthorization;

public record SendNfeToAuthorizationRequest : IRequest<SendNfeToAuthorizationResponse>
{
    public string NumeroNota { get; init; } = string.Empty;
    public string Serie { get; init; } = string.Empty;
    public Guid EmitenteId { get; init; }
    public Guid DestinatarioId { get; init; }
    public List<ItemNfeDto> Itens { get; init; } = new();
}

public record ItemNfeDto
{
    public Guid ProdutoId { get; init; }
    public int Sequencia { get; init; }
    public decimal Quantidade { get; init; }
    public decimal ValorUnitario { get; init; }
    public string Cfop { get; init; } = string.Empty;
    public string Cst { get; init; } = string.Empty;
}