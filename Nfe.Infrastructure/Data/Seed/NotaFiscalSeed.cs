using Microsoft.EntityFrameworkCore;
using Nfe.Domain.Entities;
using Nfe.Domain.ValueObjects;

namespace Nfe.Infrastructure.Data.Seed;

public static class NotaFiscalSeed
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        SeedNotasFiscais(modelBuilder);
    }
    private static void SeedNotasFiscais(ModelBuilder modelBuilder)
    {
        var nfe1Id = Guid.Parse("f1111111-1111-1111-1111-111111111111");
        var nfe2Id = Guid.Parse("f2222222-2222-2222-2222-222222222222");

        modelBuilder.Entity<NotaFiscal>().HasData(
            new
            {
                Id = nfe1Id,
                NumeroNota = "000000001",
                Serie = "1",
                ChaveAcesso = "35240112345678000123550010000000011234567890",
                DataEmissao = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc),
                DataAutorizacao = new DateTime(2024, 1, 15, 10, 31, 0, DateTimeKind.Utc),
                Status = StatusNFe.Autorizada,
                ProtocoloAutorizacao = "135240000012345",
                XmlNFe = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><nfeProc versao=\"4.00\"><NFe><infNFe Id=\"NFe35240112345678000123550010000000011234567890\"></infNFe></NFe></nfeProc>",
                MotivoRejeicao = (string?)null,
                DataCriacao = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc),
                EmitenteId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                DestinatarioId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ValorTotal = 3200.00m,
                ValorIcms = 576.00m,
                ValorIpi = 160.00m,
                ValorPis = 52.80m,
                ValorCofins = 243.20m
            },
            new
            {
                Id = nfe2Id,
                NumeroNota = "000000002",
                Serie = "1",
                ChaveAcesso = "35240112345678000123550010000000021234567890",
                DataEmissao = new DateTime(2024, 1, 16, 14, 45, 0, DateTimeKind.Utc),
                DataAutorizacao = (DateTime?)null,
                Status = StatusNFe.EmProcessamento,
                ProtocoloAutorizacao = (string?)null,
                XmlNFe = (string?)null,
                MotivoRejeicao = (string?)null,
                DataCriacao = new DateTime(2024, 1, 16, 14, 45, 0, DateTimeKind.Utc),
                EmitenteId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                DestinatarioId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ValorTotal = 835.00m,
                ValorIcms = 150.30m,
                ValorIpi = 41.75m,
                ValorPis = 13.78m,
                ValorCofins = 63.46m
            }
        );

        modelBuilder.Entity<ItemNfe>().HasData(
            // Itens da NFe 1
            new
            {
                Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                NotaFiscalId = nfe1Id,
                ProdutoId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Sequencia = 1,
                Quantidade = 1.00m,
                ValorUnitario = 2500.00m,
                ValorTotal = 2500.00m,
                ValorIcms = 450.00m,
                ValorIpi = 125.00m,
                ValorPis = 41.25m,
                ValorCofins = 190.00m,
                Cfop = "5102",
                Cst = "00"
            },
            new
            {
                Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"),
                NotaFiscalId = nfe1Id,
                ProdutoId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Sequencia = 2,
                Quantidade = 1.00m,
                ValorUnitario = 700.00m,
                ValorTotal = 700.00m,
                ValorIcms = 126.00m,
                ValorIpi = 35.00m,
                ValorPis = 11.55m,
                ValorCofins = 53.20m,
                Cfop = "5102",
                Cst = "00"
            },
            // Itens da NFe 2
            new
            {
                Id = Guid.Parse("c3333333-3333-3333-3333-333333333333"),
                NotaFiscalId = nfe2Id,
                ProdutoId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Sequencia = 1,
                Quantidade = 1.00m,
                ValorUnitario = 350.00m,
                ValorTotal = 350.00m,
                ValorIcms = 63.00m,
                ValorIpi = 17.50m,
                ValorPis = 5.78m,
                ValorCofins = 26.60m,
                Cfop = "5102",
                Cst = "00"
            },
            new
            {
                Id = Guid.Parse("d4444444-4444-4444-4444-444444444444"),
                NotaFiscalId = nfe2Id,
                ProdutoId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Sequencia = 2,
                Quantidade = 1.00m,
                ValorUnitario = 450.00m,
                ValorTotal = 450.00m,
                ValorIcms = 81.00m,
                ValorIpi = 22.50m,
                ValorPis = 7.43m,
                ValorCofins = 34.20m,
                Cfop = "5102",
                Cst = "00"
            },
            new
            {
                Id = Guid.Parse("e5555555-5555-5555-5555-555555555555"),
                NotaFiscalId = nfe2Id,
                ProdutoId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Sequencia = 3,
                Quantidade = 1.00m,
                ValorUnitario = 35.00m,
                ValorTotal = 35.00m,
                ValorIcms = 6.30m,
                ValorIpi = 1.75m,
                ValorPis = 0.58m,
                ValorCofins = 2.66m,
                Cfop = "5102",
                Cst = "00"
            }
        );
    }
}