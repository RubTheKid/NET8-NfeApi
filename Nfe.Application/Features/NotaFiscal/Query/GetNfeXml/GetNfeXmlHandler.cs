using MediatR;
using Nfe.Domain.Contracts.Repositories;
using Nfe.Domain.ValueObjects;

namespace Nfe.Application.Features.NotaFiscal.Query.GetNfeXml;

public class GetNfeXmlHandler : IRequestHandler<GetNfeXmlRequest, GetNfeXmlResponse?>
{
    private readonly INfeRepository _nfeRepository;

    public GetNfeXmlHandler(INfeRepository nfeRepository)
    {
        _nfeRepository = nfeRepository;
    }

    public async Task<GetNfeXmlResponse?> Handle(GetNfeXmlRequest request, CancellationToken cancellationToken)
    {
        var nfe = await _nfeRepository.GetById(request.id);

        if (nfe == null)
            return null;

        if (nfe.Status != StatusNFe.Autorizada)
        {
            return new GetNfeXmlResponse
            {
                Status = nfe.Status.ToString(),
                XmlContent = null,
                Message = "XML só está disponível para notas autorizadas"
            };
        }

        return new GetNfeXmlResponse
        {
            Status = nfe.Status.ToString(),
            XmlContent = nfe.XmlNFe,
            Message = null
        };
    }
}