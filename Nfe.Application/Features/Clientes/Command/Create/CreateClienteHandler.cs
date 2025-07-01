using MediatR;
using Nfe.Application.Features.Clientes.Command.Create.Request;
using Nfe.Application.Features.Clientes.Command.Create.Response;
using Nfe.Domain.Contracts.Repositories;
using Nfe.Domain.Entities;
using Nfe.Domain.ValueObjects;

namespace Nfe.Application.Features.Clientes.Command.Create;

public class CreateClienteHandler : IRequestHandler<CreateClienteRequest, CreateClienteResponse>
{
    private readonly IClienteRepository _clienteRepository;

    public CreateClienteHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<CreateClienteResponse> Handle(CreateClienteRequest request, CancellationToken cancellationToken)
    {
        if (await _clienteRepository.CnpjExists(request.Cnpj))
        {
            return new CreateClienteResponse
            {
                Id = Guid.Empty,
                Message = "CNPJ já está cadastrado no sistema."
            };
        }

        var cnpj = new Cnpj(request.Cnpj);
        var endereco = new Endereco(
            request.Endereco.Logradouro,
            request.Endereco.Numero,
            request.Endereco.Complemento,
            request.Endereco.Bairro,
            request.Endereco.Cidade,
            request.Endereco.Estado,
            request.Endereco.Cep,
            request.Endereco.Pais
        );

        var cliente = new Cliente(
            request.RazaoSocial,
            request.NomeFantasia,
            cnpj,
            request.InscricaoEstadual,
            endereco,
            request.Email,
            request.Telefone
        );

        var clienteCriado = await _clienteRepository.CreateCliente(cliente);

        return new CreateClienteResponse
        {
            Id = clienteCriado.Id,
            RazaoSocial = clienteCriado.RazaoSocial,
            NomeFantasia = clienteCriado.NomeFantasia,
            Cnpj = clienteCriado.Cnpj.Numero,
        };
    }
}