using Nfe.Domain.Entities;

namespace Nfe.Domain.Contracts.Repositories;

public interface INfeRepository : IBaseRepository<NotaFiscal>
{
    Task<NotaFiscal?> GetById(Guid id);
    Task<NotaFiscal?> GetByChaveAcesso(string chaveAcesso);
    Task<NotaFiscal> Add(NotaFiscal entity);
} 