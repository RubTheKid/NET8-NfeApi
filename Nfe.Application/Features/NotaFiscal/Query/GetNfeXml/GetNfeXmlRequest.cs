using MediatR;

namespace Nfe.Application.Features.NotaFiscal.Query.GetNfeXml;

public sealed record GetNfeXmlRequest(Guid id) : IRequest<GetNfeXmlResponse> { }; 