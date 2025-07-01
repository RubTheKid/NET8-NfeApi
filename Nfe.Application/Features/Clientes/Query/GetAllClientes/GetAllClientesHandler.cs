using MediatR;
using Nfe.Domain.Contracts.Repositories;

namespace Nfe.Application.Features.Clientes.Query.GetAllClientes;

public class GetAllClientesHandler : IRequestHandler<GetAllClientesRequest, GetAllClientesResponse>
{
    private readonly IClienteRepository _clienteRepository;

    public GetAllClientesHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<GetAllClientesResponse> Handle(GetAllClientesRequest request, CancellationToken cancellationToken)
    {
        var clientes = await _clienteRepository.GetAllClientes();

        var clientesList = clientes.Select(cliente => new Clientes
        {
            Id = cliente.Id,
            RazaoSocial = cliente.RazaoSocial,
            NomeFantasia = cliente.NomeFantasia,
            Cnpj = cliente.Cnpj.Numero,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            DataCriacao = cliente.DataCriacao,
            Endereco = cliente.Endereco
        }).ToList();


        return new GetAllClientesResponse
        {
            Clientes = clientesList,
            Total = clientesList.Count
        };
    }
}