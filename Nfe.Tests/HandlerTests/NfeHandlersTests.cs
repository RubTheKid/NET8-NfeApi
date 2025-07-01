using Moq;
using Nfe.Application.Features.NotaFiscal.Query.GetNfeById;
using Nfe.Application.Features.NotaFiscal.Query.GetNfeXml;
using Nfe.Domain.Contracts.Repositories;
using Nfe.Domain.Entities;
using Nfe.Domain.ValueObjects;

namespace Nfe.Tests.HandlerTests;

[TestFixture]
public class NfeHandlersTests
{
    #region GetNfeById Tests

    [Test]
    public async Task GetNfeById_ExistingId_ShouldReturnNfe()
    {
        // Arrange
        var nfeId = Guid.NewGuid();
        var emitenteId = Guid.NewGuid();
        var destinatarioId = Guid.NewGuid();

        var emitente = CreateClienteMock(emitenteId, "Empresa Emitente LTDA", "12345678000123");
        var destinatario = CreateClienteMock(destinatarioId, "Cliente Destinatário LTDA", "98765432000198");

        var nfe = new NotaFiscal("000000001", "1", emitenteId, destinatarioId);
        SetPrivateProperty(nfe, "Id", nfeId);
        SetPrivateProperty(nfe, "Emitente", emitente);
        SetPrivateProperty(nfe, "Destinatario", destinatario);
        SetPrivateProperty(nfe, "Status", StatusNFe.Autorizada);
        SetPrivateProperty(nfe, "ValorTotal", 1000.00m);
        SetPrivateProperty(nfe, "ValorIcms", 180.00m);
        SetPrivateProperty(nfe, "DataAutorizacao", DateTime.UtcNow);
        SetPrivateProperty(nfe, "ProtocoloAutorizacao", "123456789");

        var repoMock = new Mock<INfeRepository>();
        repoMock.Setup(r => r.GetById(nfeId)).ReturnsAsync(nfe);

        var handler = new GetNfeByIdHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetNfeByIdRequest(nfeId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(nfeId);
        result.NumeroNota.Should().Be("000000001");
        result.Serie.Should().Be("1");
        result.Status.Should().Be("Autorizada");
        result.ValorTotal.Should().Be(1000.00m);
        result.ValorIcms.Should().Be(180.00m);
        result.Emitente.Should().NotBeNull();
        result.Emitente.RazaoSocial.Should().Be("Empresa Emitente LTDA");
        result.Destinatario.Should().NotBeNull();
        result.Destinatario.RazaoSocial.Should().Be("Cliente Destinatário LTDA");
    }

    [Test]
    public async Task GetNfeById_NonExistingId_ShouldReturnNull()
    {
        // Arrange
        var repoMock = new Mock<INfeRepository>();
        repoMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((NotaFiscal)null);

        var handler = new GetNfeByIdHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetNfeByIdRequest(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetNfeById_EmptyId_ShouldReturnNull()
    {
        // Arrange
        var repoMock = new Mock<INfeRepository>();
        repoMock.Setup(r => r.GetById(Guid.Empty)).ReturnsAsync((NotaFiscal)null);

        var handler = new GetNfeByIdHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetNfeByIdRequest(Guid.Empty), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetNfeXml Tests

    [Test]
    public async Task GetNfeXml_AuthorizedNfe_ShouldReturnXml()
    {
        // Arrange
        var nfeId = Guid.NewGuid();
        var xmlContent = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><nfeProc>...</nfeProc>";
        
        var nfe = new NotaFiscal("000000001", "1", Guid.NewGuid(), Guid.NewGuid());
        SetPrivateProperty(nfe, "Id", nfeId);
        SetPrivateProperty(nfe, "Status", StatusNFe.Autorizada);
        SetPrivateProperty(nfe, "XmlNFe", xmlContent);

        var repoMock = new Mock<INfeRepository>();
        repoMock.Setup(r => r.GetById(nfeId)).ReturnsAsync(nfe);

        var handler = new GetNfeXmlHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetNfeXmlRequest(nfeId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be("Autorizada");
        result.XmlContent.Should().Be(xmlContent);
        result.Message.Should().BeNull();
    }

    [Test]
    public async Task GetNfeXml_NotAuthorizedNfe_ShouldReturnMessageWithoutXml()
    {
        // Arrange
        var nfeId = Guid.NewGuid();
        
        var nfe = new NotaFiscal("000000001", "1", Guid.NewGuid(), Guid.NewGuid());
        SetPrivateProperty(nfe, "Id", nfeId);
        SetPrivateProperty(nfe, "Status", StatusNFe.EmProcessamento);

        var repoMock = new Mock<INfeRepository>();
        repoMock.Setup(r => r.GetById(nfeId)).ReturnsAsync(nfe);

        var handler = new GetNfeXmlHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetNfeXmlRequest(nfeId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be("EmProcessamento");
        result.XmlContent.Should().BeNull();
        result.Message.Should().Be("XML só está disponível para notas autorizadas");
    }

    [Test]
    public async Task GetNfeXml_RejectedNfe_ShouldReturnMessageWithoutXml()
    {
        // Arrange
        var nfeId = Guid.NewGuid();
        
        var nfe = new NotaFiscal("000000001", "1", Guid.NewGuid(), Guid.NewGuid());
        SetPrivateProperty(nfe, "Id", nfeId);
        SetPrivateProperty(nfe, "Status", StatusNFe.Rejeitada);
        SetPrivateProperty(nfe, "MotivoRejeicao", "Erro na validação dos dados");

        var repoMock = new Mock<INfeRepository>();
        repoMock.Setup(r => r.GetById(nfeId)).ReturnsAsync(nfe);

        var handler = new GetNfeXmlHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetNfeXmlRequest(nfeId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be("Rejeitada");
        result.XmlContent.Should().BeNull();
        result.Message.Should().Be("XML só está disponível para notas autorizadas");
    }

    [Test]
    public async Task GetNfeXml_NonExistingId_ShouldReturnNull()
    {
        // Arrange
        var repoMock = new Mock<INfeRepository>();
        repoMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((NotaFiscal)null);

        var handler = new GetNfeXmlHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetNfeXmlRequest(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Helper Methods

    private static Cliente CreateClienteMock(Guid id, string razaoSocial, string cnpj)
    {
        var endereco = new Endereco("Rua Teste", "123", null, "Centro", "São Paulo", "SP", "01010101", "Brasil");
        var cliente = new Cliente(razaoSocial, "Nome Fantasia", new Cnpj(cnpj), "123456789", 
            endereco, "teste@teste.com", "11999999999");
        
        SetPrivateProperty(cliente, "Id", id);
        return cliente;
    }

    private static void SetPrivateProperty(object obj, string propertyName, object value)
    {
        var property = obj.GetType().GetProperty(propertyName, 
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        property?.SetValue(obj, value);
    }

    #endregion
} 