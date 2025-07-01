using MediatR;
using Nfe.Domain.Contracts.Repositories;
using Nfe.Domain.ValueObjects;

namespace Nfe.Application.Features.Clientes.Query.GetClienteById;

public class GetClienteByIdHandler : IRequestHandler<GetClienteByIdRequest, GetClienteByIdResponse?>
{
    private readonly IClienteRepository _clienteRepository;

    public GetClienteByIdHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<GetClienteByIdResponse?> Handle(GetClienteByIdRequest request, CancellationToken cancellationToken)
    {
        var cliente = await _clienteRepository.GetById(request.id);

        if (cliente == null)
            return null;

        return new GetClienteByIdResponse
        {
            Id = cliente.Id,
            RazaoSocial = cliente.RazaoSocial,
            NomeFantasia = cliente.NomeFantasia,
            Cnpj = cliente.Cnpj.Numero,
            InscricaoEstadual = cliente.InscricaoEstadual,
            Email = cliente.Email,
            Telefone = cliente.Telefone,
            DataCriacao = cliente.DataCriacao,
            Endereco = new Endereco(
                cliente.Endereco.Logradouro,
                cliente.Endereco.Numero,
                cliente.Endereco.Complemento,
                cliente.Endereco.Bairro,
                cliente.Endereco.Cidade,
                cliente.Endereco.Estado,
                cliente.Endereco.Cep,
                cliente.Endereco.Pais
            )
        };
    }
}