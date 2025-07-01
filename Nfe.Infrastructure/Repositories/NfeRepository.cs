using Microsoft.EntityFrameworkCore;
using Nfe.Domain.Contracts.Repositories;
using Nfe.Domain.Entities;
using Nfe.Infrastructure.Data;

namespace Nfe.Infrastructure.Repositories;

public class NfeRepository : BaseRepository<NotaFiscal>, INfeRepository
{
    public NfeRepository(NfeDbContext context) : base(context) { }

    public async Task<NotaFiscal?> GetById(Guid id)
    {
        return await _context.Set<NotaFiscal>()
            .Include(n => n.Emitente)
            .Include(n => n.Destinatario)
            .Include(n => n.Itens)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<NotaFiscal?> GetByChaveAcesso(string chaveAcesso)
    {
        return await _context.Set<NotaFiscal>()
            .Include(n => n.Emitente)
            .Include(n => n.Destinatario)
            .Include(n => n.Itens)
            .FirstOrDefaultAsync(n => n.ChaveAcesso == chaveAcesso);
    }

    public async Task<NotaFiscal> Add(NotaFiscal entity)
    {
        await _context.Set<NotaFiscal>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
} 