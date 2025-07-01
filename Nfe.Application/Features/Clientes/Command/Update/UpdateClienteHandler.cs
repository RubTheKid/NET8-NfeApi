using MediatR;
using Nfe.Domain.Contracts.Repositories;
using Nfe.Domain.ValueObjects;

namespace Nfe.Application.Features.Clientes.Command.Update;

public class UpdateClienteHandler : IRequestHandler<UpdateClienteRequest, UpdateClienteResponse>
{
    private readonly IClienteRepository _clienteRepository;

    public UpdateClienteHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<UpdateClienteResponse> Handle(UpdateClienteRequest request, CancellationToken cancellationToken)
    {
        var cliente = await _clienteRepository.GetById(request.Id);
        if (cliente == null)
        {
            throw new ArgumentException("Cliente não encontrado.");
        }

        if (await _clienteRepository.CnpjExistsForOtherCliente(request.Cnpj, request.Id))
        {
            throw new ArgumentException("CNPJ já está cadastrado para outro cliente.");
        }

        cliente.AtualizarRazaoSocial(request.RazaoSocial);
        cliente.AtualizarNomeFantasia(request.NomeFantasia);
        cliente.AtualizarCnpj(new Cnpj(request.Cnpj));
        cliente.AtualizarInscricaoEstadual(request.InscricaoEstadual);
        cliente.AtualizarEmail(request.Email);
        cliente.AtualizarTelefone(request.Telefone);
        cliente.AtualizarEndereco(new Endereco(
            request.Endereco.Logradouro,
            request.Endereco.Numero,
            request.Endereco.Complemento,
            request.Endereco.Bairro,
            request.Endereco.Cidade,
            request.Endereco.Estado,
            request.Endereco.Cep,
            request.Endereco.Pais
        ));

        var clienteAtualizado = await _clienteRepository.UpdateCliente(cliente);

        return new UpdateClienteResponse
        {
            Id = clienteAtualizado.Id,
            RazaoSocial = clienteAtualizado.RazaoSocial,
            NomeFantasia = clienteAtualizado.NomeFantasia,
            Cnpj = clienteAtualizado.Cnpj.Numero,
            InscricaoEstadual = clienteAtualizado.InscricaoEstadual,
            Email = clienteAtualizado.Email,
            Telefone = clienteAtualizado.Telefone,
            DataCriacao = clienteAtualizado.DataCriacao,
            Endereco = new Endereco(
                clienteAtualizado.Endereco.Logradouro,
                clienteAtualizado.Endereco.Numero,
                clienteAtualizado.Endereco.Complemento,
                clienteAtualizado.Endereco.Bairro,
                clienteAtualizado.Endereco.Cidade,
                clienteAtualizado.Endereco.Estado,
                clienteAtualizado.Endereco.Cep,
                clienteAtualizado.Endereco.Pais
            )
        };
    }
}