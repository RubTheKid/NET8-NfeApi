using Nfe.Domain.Entities;

namespace Nfe.Application.Services;

public interface INfeXmlService
{
    Task<string> ConvertToXmlAsync(NotaFiscal notaFiscal);
}