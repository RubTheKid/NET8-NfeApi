using MediatR;

namespace Nfe.Application.Features.NotaFiscal.Query.GetNfeById;

public sealed record GetNfeByIdRequest(Guid id) : IRequest<GetNfeByIdResponse> { };