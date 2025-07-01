using MediatR;

namespace Nfe.Application.Features.Clientes.Query.GetAllClientes;

public sealed record GetAllClientesRequest() : IRequest<GetAllClientesResponse> { };