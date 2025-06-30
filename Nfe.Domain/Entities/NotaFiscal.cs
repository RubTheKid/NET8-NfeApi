using Nfe.Domain.ValueObjects;
using System.Xml;

namespace Nfe.Domain.Entities;

public class NotaFiscal
{
    public Guid Id { get; private set; }
    public string NumeroNota { get; private set; }
    public string Serie { get; private set; }
    public string ChaveAcesso { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public DateTime? DataAutorizacao { get; private set; }
    public StatusNFe Status { get; private set; }
    public string? ProtocoloAutorizacao { get; private set; }
    public string? XmlNFe { get; private set; }
    public string? MotivoRejeicao { get; private set; }
    public DateTime DataCriacao { get; private set; }

    public Guid EmitenteId { get; private set; }
    public virtual Cliente Emitente { get; private set; } = null!;
    public Guid DestinatarioId { get; private set; }
    public virtual Cliente Destinatario { get; private set; } = null!;
    public virtual ICollection<ItemNfe> Itens { get; private set; } = new List<ItemNfe>();

    public decimal ValorTotal { get; private set; }
    public decimal ValorIcms { get; private set; }
    public decimal ValorIpi { get; private set; }
    public decimal ValorPis { get; private set; }
    public decimal ValorCofins { get; private set; }

    private NotaFiscal() { }

    public NotaFiscal(string numeroNota, string serie, Guid emitenteId, Guid destinatarioId)
    {
        Id = Guid.NewGuid();
        NumeroNota = numeroNota;
        Serie = serie;
        DataEmissao = DateTime.UtcNow;
        Status = StatusNFe.EmProcessamento;
        DataCriacao = DateTime.UtcNow;
        EmitenteId = emitenteId;
        DestinatarioId = destinatarioId;
        GerarChaveAcesso();
    }

    public void AdicionarItem(ItemNfe item)
    {
        ((List<ItemNfe>)Itens).Add(item);
        CalcularTotais();
    }

    public void Autorizar(string protocoloAutorizacao, string xmlNFe)
    {
        if (Status != StatusNFe.EmProcessamento)
            throw new InvalidOperationException("Apenas notas em processamento podem ser autorizadas");

        Status = StatusNFe.Autorizada;
        ProtocoloAutorizacao = protocoloAutorizacao;
        XmlNFe = xmlNFe;
        DataAutorizacao = DateTime.UtcNow;
    }

    public void Rejeitar(string motivo)
    {
        if (Status != StatusNFe.EmProcessamento)
            throw new InvalidOperationException("Apenas notas em processamento podem ser rejeitadas");

        Status = StatusNFe.Rejeitada;
        MotivoRejeicao = motivo;
    }

    private void GerarChaveAcesso()
    {
        // Lógica simplificada para gerar chave de acesso (44 dígitos)
        var uf = "35"; // SP
        var aamm = DateTime.Now.ToString("yyMM");
        var cnpjEmitente = "12345678000123"; // seria obtido do emitente
        var modelo = "55"; // NF-e
        var random = new Random();
        var dv = random.Next(0, 9);

        ChaveAcesso = $"{uf}{aamm}{cnpjEmitente}{modelo}{Serie.PadLeft(3, '0')}{NumeroNota.PadLeft(9, '0')}{random.Next(10000000, 99999999)}{dv}";
    }

    private void CalcularTotais()
    {
        ValorTotal = Itens.Sum(i => i.ValorTotal);
        ValorIcms = Itens.Sum(i => i.ValorIcms);
        ValorIpi = Itens.Sum(i => i.ValorIpi);
        ValorPis = Itens.Sum(i => i.ValorPis);
        ValorCofins = Itens.Sum(i => i.ValorCofins);
    }
}
