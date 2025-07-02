namespace Nfe.Application.Features.NotaFiscal.Query.GetNfeById;

public class GetNfeByIdResponse
{
    public Guid Id { get; set; }
    public string NumeroNota { get; set; } = null!;
    public string Serie { get; set; } = null!;
    public string ChaveAcesso { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime DataEmissao { get; set; }
    public DateTime? DataAutorizacao { get; set; }
    public string? ProtocoloAutorizacao { get; set; }
    public string? MotivoRejeicao { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorIcms { get; set; }
    public decimal ValorIpi { get; set; }
    public decimal ValorPis { get; set; }
    public decimal ValorCofins { get; set; }
    public Emitente Emitente { get; set; } = null!;
    public Destinatario Destinatario { get; set; } = null!;
}

public class Emitente
{
    public Guid Id { get; set; }
    public string RazaoSocial { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
}

public class Destinatario
{
    public Guid Id { get; set; }
    public string RazaoSocial { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
}