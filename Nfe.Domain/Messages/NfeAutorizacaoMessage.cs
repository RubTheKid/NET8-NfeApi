namespace Nfe.Domain.Messages;

public sealed record NfeAutorizacaoMessage
{
    public Guid NfeId { get; init; }
    public string NumeroNota { get; init; } = string.Empty;
    public string Serie { get; init; } = string.Empty;
    public string ChaveAcesso { get; init; } = string.Empty;
    public Guid EmitenteId { get; init; }
    public Guid DestinatarioId { get; init; }
    public decimal ValorTotal { get; init; }
    public DateTime DataEnvio { get; init; }
    public string XmlNfe { get; init; } = string.Empty;
}