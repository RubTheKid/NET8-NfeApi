using Nfe.Domain.Entities;

namespace Nfe.Domain.Contracts.Repositories;

public interface IClienteRepository : IBaseRepository<Cliente>
{
    Task<IEnumerable<Cliente>> GetAllClientes();
    Task<Cliente?> GetByCnpj(string cnpj);

    Task<Cliente> CreateCliente(Cliente cliente);
    Task<Cliente> UpdateCliente(Cliente cliente);
    Task DeleteCliente(Guid id);
    Task<bool> CanDeleteCliente(Guid id);

    Task<bool> CnpjExists(string cnpj);
    Task<bool> CnpjExistsForOtherCliente(string cnpj, Guid clienteId);

}
