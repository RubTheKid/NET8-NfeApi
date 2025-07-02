namespace Nfe.Application.Features.NotaFiscal.Query.GetNfeXml;

public class GetNfeXmlResponse
{
    public string Status { get; set; } = null!;
    public string? XmlContent { get; set; }
    public string? Message { get; set; }
}