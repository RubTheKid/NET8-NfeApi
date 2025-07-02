using Nfe.Domain.ValueObjects;

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
        // Gerar chave de acesso da NFe (44 dígitos)
        var uf = "35"; // SP (2 dígitos)
        var aamm = DateTime.Now.ToString("yyMM"); // Ano e Mês (4 dígitos)
        var cnpjEmitente = "12345678000123"; // CNPJ do emitente (14 dígitos)
        var modelo = "55"; // Modelo NFe (2 dígitos)
        var serie = Serie.PadLeft(3, '0'); // Série (3 dígitos)
        var numeroNota = NumeroNota.PadLeft(9, '0'); // Número da nota (9 dígitos)
        var tpEmis = "1"; // Forma de emissão - Normal (1 dígito)
        var random = new Random();
        var codigoNumerico = random.Next(10000000, 99999999).ToString(); // Código numérico (8 dígitos)

        // Chave sem o dígito verificador (43 dígitos)
        var chaveSemDv = $"{uf}{aamm}{cnpjEmitente}{modelo}{serie}{numeroNota}{tpEmis}{codigoNumerico}";

        // Calcular dígito verificador
        var dv = CalcularDigitoVerificador(chaveSemDv);

        // Chave completa (44 dígitos)
        ChaveAcesso = $"{chaveSemDv}{dv}";
    }

    private static int CalcularDigitoVerificador(string chave)
    {
        // Algoritmo simplificado para calcular o dígito verificador
        // Em produção, deve-se usar o algoritmo oficial do módulo 11
        var soma = 0;
        var multiplicador = 2;

        for (int i = chave.Length - 1; i >= 0; i--)
        {
            soma += int.Parse(chave[i].ToString()) * multiplicador;
            multiplicador++;
            if (multiplicador > 9)
                multiplicador = 2;
        }

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
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
