namespace Nfe.Application.Features.NotaFiscal.Command.SendNfeToAuthorization;

public record SendNfeToAuthorizationResponse
{
    public Guid NfeId { get; init; }
    public string NumeroNota { get; init; } = string.Empty;
    public string Serie { get; init; } = string.Empty;
    public string ChaveAcesso { get; init; } = string.Empty;
    public DateTime DataEmissao { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal ValorTotal { get; init; }
    public string Message { get; init; } = string.Empty;
    public bool Success { get; init; }
}