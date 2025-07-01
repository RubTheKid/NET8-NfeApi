using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nfe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialDataFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "DataCriacao", "Email", "InscricaoEstadual", "NomeFantasia", "RazaoSocial", "Telefone", "Cnpj", "Bairro", "Cep", "Cidade", "Complemento", "Estado", "Logradouro", "NumeroEndereco", "Pais" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "emitente@empresa.com", "123456789", "Emitente Tech", "Empresa Emitente LTDA", "11987654321", "12345678000123", "Centro", "01234567", "São Paulo", "Sala 101", "SP", "Rua das Empresas", "123", "Brasil" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "destinatario@cliente.com", "987654321", "Destinatário Corp", "Cliente Destinatário LTDA", "11912345678", "98765432000198", "Vila Nova", "87654321", "Rio de Janeiro", null, "RJ", "Avenida dos Clientes", "456", "Brasil" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "contato@abcdist.com", "555666777", "ABC Distribuição", "Distribuidora ABC S/A", "11999888777", "11222333000144", "Industrial", "12345678", "Guarulhos", "Galpão A", "SP", "Rua do Comércio", "789", "Brasil" }
                });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Ativo", "CodigoBarras", "DataCriacao", "Descricao", "Ncm", "Nome", "Peso", "Preco", "UnidadeMedida" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), true, "7891234567890", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Notebook Dell Inspiron 15 3000, Intel Core i5, 8GB RAM, 256GB SSD", "84713012", "Notebook Dell Inspiron", 2.1m, 2500.00m, "UN" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), true, "7891234567891", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mouse Logitech MX Master 3, Wireless, Bluetooth", "84716060", "Mouse Logitech MX", 0.15m, 350.00m, "UN" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), true, "7891234567892", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Teclado Mecânico RGB, Switch Blue, Layout ABNT2", "84716060", "Teclado Mecânico RGB", 1.2m, 450.00m, "UN" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), true, "7891234567893", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Monitor LED 24 polegadas, Full HD, HDMI, VGA", "85285210", "Monitor LED 24\"", 4.5m, 800.00m, "UN" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), true, "7891234567894", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cabo HDMI 2.0, 2 metros, 4K, Alta Velocidade", "85444200", "Cabo HDMI 2m", 0.3m, 35.00m, "UN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));
        }
    }
}
