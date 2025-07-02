namespace Nfe.Application.Services;

public sealed record NfeAutorizacaoResultMessage
{
    public Guid NfeId { get; init; }
    public string ChaveAcesso { get; init; } = string.Empty;
    public bool Autorizada { get; init; }
    public string? ProtocoloAutorizacao { get; init; }
    public string? MotivoRejeicao { get; init; }
    public DateTime DataProcessamento { get; init; }
    public string? XmlRetorno { get; init; }
}