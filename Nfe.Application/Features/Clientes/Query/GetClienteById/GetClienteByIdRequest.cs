using MediatR;

namespace Nfe.Application.Features.Clientes.Query.GetClienteById;

public sealed record GetClienteByIdRequest(Guid id) : IRequest<GetClienteByIdResponse> { };