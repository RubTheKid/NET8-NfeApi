using Microsoft.EntityFrameworkCore;
using Nfe.Domain.Contracts.Repositories;
using Nfe.Domain.Entities;
using Nfe.Infrastructure.Data;

namespace Nfe.Infrastructure.Repositories;

public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(NfeDbContext context) : base(context) { }

    public async Task<IEnumerable<Cliente>> GetAllClientes()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Cliente?> GetByCnpj(string cnpj)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Cnpj.Numero == cnpj);
    }

    public async Task<Cliente> CreateCliente(Cliente cliente)
    {
        if (await CnpjExists(cliente.Cnpj.Numero))
        {
            throw new InvalidOperationException($"Já existe um cliente com o CNPJ {cliente.Cnpj}");
        }

        await _dbSet.AddAsync(cliente);
        await SaveChangesAsync();
        return cliente;
    }

    public async Task<Cliente> UpdateCliente(Cliente cliente)
    {
        var clienteExistente = await GetById(cliente.Id);
        if (clienteExistente == null)
        {
            throw new InvalidOperationException($"Cliente com ID {cliente.Id} não encontrado");
        }

        if (await CnpjExistsForOtherCliente(cliente.Cnpj.Numero, cliente.Id))
        {
            throw new InvalidOperationException($"CNPJ {cliente.Cnpj} já está sendo usado por outro cliente");
        }

        _dbSet.Update(cliente);
        await SaveChangesAsync();
        return cliente;
    }

    public async Task DeleteCliente(Guid id)
    {
        if (!await CanDeleteCliente(id))
        {
            throw new InvalidOperationException("Cliente não pode ser excluído pois possui notas fiscais associadas");
        }

        await Delete(id);
    }

    public async Task<bool> CanDeleteCliente(Guid id)
    {
        // Verificar se cliente tem notas fiscais associadas
        var temNotasComoEmitente = await _context.NotasFiscais
            .AnyAsync(n => n.EmitenteId == id);

        var temNotasComoDestinatario = await _context.NotasFiscais
            .AnyAsync(n => n.DestinatarioId == id);

        return !temNotasComoEmitente && !temNotasComoDestinatario;
    }
    public async Task<bool> CnpjExists(string cnpj)
    {
        return await _dbSet
            .AnyAsync(c => c.Cnpj.Numero == cnpj);
    }

    public async Task<bool> CnpjExistsForOtherCliente(string cnpj, Guid clienteId)
    {
        return await _dbSet
            .AnyAsync(c => c.Cnpj.Numero == cnpj && c.Id != clienteId);
    }



}
