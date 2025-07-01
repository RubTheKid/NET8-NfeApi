using Microsoft.EntityFrameworkCore;
using Nfe.Domain.Entities;

namespace Nfe.Infrastructure.Data.Seed;

public static class DataSeed
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        SeedClientes(modelBuilder);
        SeedProdutos(modelBuilder);
    }

    private static void SeedClientes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>().HasData(
            new
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                RazaoSocial = "Empresa Emitente LTDA",
                NomeFantasia = "Emitente Tech",
                InscricaoEstadual = "123456789",
                Email = "emitente@empresa.com",
                Telefone = "11987654321",
                DataCriacao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                RazaoSocial = "Cliente Destinatário LTDA",
                NomeFantasia = "Destinatário Corp",
                InscricaoEstadual = "987654321",
                Email = "destinatario@cliente.com",
                Telefone = "11912345678",
                DataCriacao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                RazaoSocial = "Distribuidora ABC S/A",
                NomeFantasia = "ABC Distribuição",
                InscricaoEstadual = "555666777",
                Email = "contato@abcdist.com",
                Telefone = "11999888777",
                DataCriacao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }
        );

        modelBuilder.Entity<Cliente>().OwnsOne(c => c.Cnpj).HasData(
            new { ClienteId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Numero = "12345678000123" },
            new { ClienteId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Numero = "98765432000198" },
            new { ClienteId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Numero = "11222333000144" }
        );

        modelBuilder.Entity<Cliente>().OwnsOne(c => c.Endereco).HasData(
            new
            {
                ClienteId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Logradouro = "Rua das Empresas",
                Numero = "123",
                Complemento = "Sala 101",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "01234567",
                Pais = "Brasil"
            },
            new
            {
                ClienteId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Logradouro = "Avenida dos Clientes",
                Numero = "456",
                Complemento = (string?)null,
                Bairro = "Vila Nova",
                Cidade = "Rio de Janeiro",
                Estado = "RJ",
                Cep = "87654321",
                Pais = "Brasil"
            },
            new
            {
                ClienteId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Logradouro = "Rua do Comércio",
                Numero = "789",
                Complemento = "Galpão A",
                Bairro = "Industrial",
                Cidade = "Guarulhos",
                Estado = "SP",
                Cep = "12345678",
                Pais = "Brasil"
            }
        );
    }

    private static void SeedProdutos(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>().HasData(
            new
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Nome = "Notebook Dell Inspiron",
                Descricao = "Notebook Dell Inspiron 15 3000, Intel Core i5, 8GB RAM, 256GB SSD",
                CodigoBarras = "7891234567890",
                Ncm = "84713012",
                UnidadeMedida = "UN",
                Preco = 2500.00m,
                Peso = 2.1m,
                DataCriacao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Ativo = true
            },
            new
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Nome = "Mouse Logitech MX",
                Descricao = "Mouse Logitech MX Master 3, Wireless, Bluetooth",
                CodigoBarras = "7891234567891",
                Ncm = "84716060",
                UnidadeMedida = "UN",
                Preco = 350.00m,
                Peso = 0.15m,
                DataCriacao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Ativo = true
            },
            new
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Nome = "Teclado Mecânico RGB",
                Descricao = "Teclado Mecânico RGB, Switch Blue, Layout ABNT2",
                CodigoBarras = "7891234567892",
                Ncm = "84716060",
                UnidadeMedida = "UN",
                Preco = 450.00m,
                Peso = 1.2m,
                DataCriacao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Ativo = true
            },
            new
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Nome = "Monitor LED 24\"",
                Descricao = "Monitor LED 24 polegadas, Full HD, HDMI, VGA",
                CodigoBarras = "7891234567893",
                Ncm = "85285210",
                UnidadeMedida = "UN",
                Preco = 800.00m,
                Peso = 4.5m,
                DataCriacao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Ativo = true
            },
            new
            {
                Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Nome = "Cabo HDMI 2m",
                Descricao = "Cabo HDMI 2.0, 2 metros, 4K, Alta Velocidade",
                CodigoBarras = "7891234567894",
                Ncm = "85444200",
                UnidadeMedida = "UN",
                Preco = 35.00m,
                Peso = 0.3m,
                DataCriacao = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Ativo = true
            }
        );
    }
}