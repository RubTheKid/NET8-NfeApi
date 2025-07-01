using Microsoft.EntityFrameworkCore;
using Nfe.Domain.Entities;
using Nfe.Infrastructure.Data.Seed;
using System.Reflection.Emit;

namespace Nfe.Infrastructure.Data;

public class NfeDbContext(DbContextOptions<NfeDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<NotaFiscal> NotasFiscais { get; set; }
    public DbSet<ItemNfe> ItensNfe { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RazaoSocial).HasMaxLength(128).IsRequired();
            entity.Property(e => e.NomeFantasia).HasMaxLength(128);
            entity.Property(e => e.InscricaoEstadual).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(128);
            entity.Property(e => e.Telefone).HasMaxLength(20);

            entity.OwnsOne(e => e.Cnpj, cnpj =>
            {
                cnpj.Property(c => c.Numero).HasColumnName("Cnpj").HasMaxLength(14);
            });
            entity.OwnsOne(e => e.Endereco, endereco =>
            {
                endereco.Property(e => e.Logradouro).HasColumnName("Logradouro").HasMaxLength(128);
                endereco.Property(e => e.Numero).HasColumnName("NumeroEndereco").HasMaxLength(8);
                endereco.Property(e => e.Complemento).HasColumnName("Complemento").HasMaxLength(64);
                endereco.Property(e => e.Bairro).HasColumnName("Bairro").HasMaxLength(64);
                endereco.Property(e => e.Cidade).HasColumnName("Cidade").HasMaxLength(64);
                endereco.Property(e => e.Estado).HasColumnName("Estado").HasMaxLength(2);
                endereco.Property(e => e.Cep).HasColumnName("Cep").HasMaxLength(8);
                endereco.Property(e => e.Pais).HasColumnName("Pais").HasMaxLength(64);
            });
        });

        mb.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Descricao).HasMaxLength(500);
            entity.Property(e => e.CodigoBarras).HasMaxLength(50);
            entity.Property(e => e.Ncm).HasMaxLength(10);
            entity.Property(e => e.UnidadeMedida).HasMaxLength(10);
            entity.Property(e => e.Preco).HasPrecision(18, 2);
            entity.Property(e => e.Peso).HasPrecision(18, 3);
        });

        mb.Entity<NotaFiscal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NumeroNota).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Serie).HasMaxLength(3).IsRequired();
            entity.Property(e => e.ChaveAcesso).HasMaxLength(44).IsRequired();
            entity.Property(e => e.ProtocoloAutorizacao).HasMaxLength(50);
            entity.Property(e => e.XmlNFe).HasColumnType("text");
            entity.Property(e => e.MotivoRejeicao).HasMaxLength(500);
            entity.Property(e => e.Status).HasConversion<int>();

            entity.Property(e => e.ValorTotal).HasPrecision(18, 2);
            entity.Property(e => e.ValorIcms).HasPrecision(18, 2);
            entity.Property(e => e.ValorIpi).HasPrecision(18, 2);
            entity.Property(e => e.ValorPis).HasPrecision(18, 2);
            entity.Property(e => e.ValorCofins).HasPrecision(18, 2);

            entity.HasOne(e => e.Emitente)
                  .WithMany()
                  .HasForeignKey(e => e.EmitenteId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Destinatario)
                  .WithMany()
                  .HasForeignKey(e => e.DestinatarioId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Itens)
                  .WithOne(i => i.NotaFiscal)
                  .HasForeignKey(i => i.NotaFiscalId);
        });

        mb.Entity<ItemNfe>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantidade).HasPrecision(18, 4);
            entity.Property(e => e.ValorUnitario).HasPrecision(18, 2);
            entity.Property(e => e.ValorTotal).HasPrecision(18, 2);
            entity.Property(e => e.ValorIcms).HasPrecision(18, 2);
            entity.Property(e => e.ValorIpi).HasPrecision(18, 2);
            entity.Property(e => e.ValorPis).HasPrecision(18, 2);
            entity.Property(e => e.ValorCofins).HasPrecision(18, 2);
            entity.Property(e => e.Cfop).HasMaxLength(4);
            entity.Property(e => e.Cst).HasMaxLength(3);

            entity.HasOne(e => e.Produto)
                  .WithMany()
                  .HasForeignKey(e => e.ProdutoId);
        });

        DataSeed.SeedData(mb);
        NotaFiscalSeed.SeedData(mb);

        base.OnModelCreating(mb);
    }
}
