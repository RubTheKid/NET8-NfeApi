using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nfe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedNotaFiscal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clientes_NomeFantasiaNormalizada",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_RazaoSocialNormalizada",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "NomeFantasiaNormalizada",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "RazaoSocialNormalizada",
                table: "Clientes");

            migrationBuilder.InsertData(
                table: "NotasFiscais",
                columns: new[] { "Id", "ChaveAcesso", "DataAutorizacao", "DataCriacao", "DataEmissao", "DestinatarioId", "EmitenteId", "MotivoRejeicao", "NumeroNota", "ProtocoloAutorizacao", "Serie", "Status", "ValorCofins", "ValorIcms", "ValorIpi", "ValorPis", "ValorTotal", "XmlNFe" },
                values: new object[,]
                {
                    { new Guid("f1111111-1111-1111-1111-111111111111"), "35240112345678000123550010000000011234567890", new DateTime(2024, 1, 15, 10, 31, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("11111111-1111-1111-1111-111111111111"), null, "000000001", "135240000012345", "1", 2, 243.20m, 576.00m, 160.00m, 52.80m, 3200.00m, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><nfeProc versao=\"4.00\"><NFe><infNFe Id=\"NFe35240112345678000123550010000000011234567890\"></infNFe></NFe></nfeProc>" },
                    { new Guid("f2222222-2222-2222-2222-222222222222"), "35240112345678000123550010000000021234567890", null, new DateTime(2024, 1, 16, 14, 45, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 16, 14, 45, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("11111111-1111-1111-1111-111111111111"), null, "000000002", null, "1", 1, 63.46m, 150.30m, 41.75m, 13.78m, 835.00m, null }
                });

            migrationBuilder.InsertData(
                table: "ItensNfe",
                columns: new[] { "Id", "Cfop", "Cst", "NotaFiscalId", "ProdutoId", "Quantidade", "Sequencia", "ValorCofins", "ValorIcms", "ValorIpi", "ValorPis", "ValorTotal", "ValorUnitario" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), "5102", "00", new Guid("f1111111-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 1.00m, 1, 190.00m, 450.00m, 125.00m, 41.25m, 2500.00m, 2500.00m },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), "5102", "00", new Guid("f1111111-1111-1111-1111-111111111111"), new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), 1.00m, 2, 53.20m, 126.00m, 35.00m, 11.55m, 700.00m, 700.00m },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), "5102", "00", new Guid("f2222222-2222-2222-2222-222222222222"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 1.00m, 1, 26.60m, 63.00m, 17.50m, 5.78m, 350.00m, 350.00m },
                    { new Guid("d4444444-4444-4444-4444-444444444444"), "5102", "00", new Guid("f2222222-2222-2222-2222-222222222222"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), 1.00m, 2, 34.20m, 81.00m, 22.50m, 7.43m, 450.00m, 450.00m },
                    { new Guid("e5555555-5555-5555-5555-555555555555"), "5102", "00", new Guid("f2222222-2222-2222-2222-222222222222"), new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), 1.00m, 3, 2.66m, 6.30m, 1.75m, 0.58m, 35.00m, 35.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItensNfe",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "ItensNfe",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "ItensNfe",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "ItensNfe",
                keyColumn: "Id",
                keyValue: new Guid("d4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "ItensNfe",
                keyColumn: "Id",
                keyValue: new Guid("e5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "NotasFiscais",
                keyColumn: "Id",
                keyValue: new Guid("f1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "NotasFiscais",
                keyColumn: "Id",
                keyValue: new Guid("f2222222-2222-2222-2222-222222222222"));

            migrationBuilder.AddColumn<string>(
                name: "NomeFantasiaNormalizada",
                table: "Clientes",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RazaoSocialNormalizada",
                table: "Clientes",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "NomeFantasiaNormalizada", "RazaoSocialNormalizada" },
                values: new object[] { "EMITENTE TECH", "EMPRESA EMITENTE LTDA" });

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "NomeFantasiaNormalizada", "RazaoSocialNormalizada" },
                values: new object[] { "DESTINATARIO CORP", "CLIENTE DESTINATARIO LTDA" });

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "NomeFantasiaNormalizada", "RazaoSocialNormalizada" },
                values: new object[] { "ABC DISTRIBUICAO", "DISTRIBUIDORA ABC S/A" });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_NomeFantasiaNormalizada",
                table: "Clientes",
                column: "NomeFantasiaNormalizada");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_RazaoSocialNormalizada",
                table: "Clientes",
                column: "RazaoSocialNormalizada");
        }
    }
}
