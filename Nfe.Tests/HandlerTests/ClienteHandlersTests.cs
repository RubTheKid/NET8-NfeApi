using Moq;
using Nfe.Application.Features.Clientes.Command.Create;
using Nfe.Application.Features.Clientes.Command.Create.Request;
using Nfe.Application.Features.Clientes.Command.Update;
using Nfe.Application.Features.Clientes.Query.GetClienteById;
using Nfe.Domain.Contracts.Repositories;
using Nfe.Domain.Entities;
using Nfe.Domain.ValueObjects;

namespace Nfe.Tests.HandlerTests;

[TestFixture]
public class ClienteHandlersTests
{
    [Test]
    public async Task Handle_ExistingId_ShouldReturnCliente()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cliente = new Cliente("Empresa", "Fantasia", new Cnpj("12345678000111"), "IE",
            new Endereco("Rua", "1", null, "Bairro", "Cidade", "SP", "01010101", "Brasil"), "a@a.com", "11999999999");
        typeof(Cliente).GetProperty("Id")?.SetValue(cliente, clienteId);

        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.GetById(clienteId)).ReturnsAsync(cliente);

        var handler = new GetClienteByIdHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetClienteByIdRequest(clienteId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(clienteId);
        result.RazaoSocial.Should().Be("Empresa");
    }

    [Test]
    public async Task Handle_NonExistingId_ShouldReturnNull()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Cliente)null);

        var handler = new GetClienteByIdHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetClienteByIdRequest(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task Handle_InvalidId_ShouldReturnNull()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.GetById(Guid.Empty)).ReturnsAsync((Cliente)null);

        var handler = new GetClienteByIdHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetClienteByIdRequest(Guid.Empty), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task Handle_ValidRequest_ShouldCreateCliente()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.CnpjExists(It.IsAny<string>())).ReturnsAsync(false);
        repoMock.Setup(r => r.CreateCliente(It.IsAny<Cliente>()))
            .ReturnsAsync((Cliente c) =>
            {
                typeof(Cliente).GetProperty("Id")?.SetValue(c, Guid.NewGuid());
                return c;
            });

        var handler = new CreateClienteHandler(repoMock.Object);
        var request = new CreateClienteRequest
        {
            RazaoSocial = "Empresa Nova",
            NomeFantasia = "Fantasia Nova",
            Cnpj = "12345678000199",
            InscricaoEstadual = "IE",
            Email = "nova@empresa.com",
            Telefone = "11999999999",
            Endereco = new Endereco("Rua Nova", "10", null, "Bairro", "Cidade", "SP", "01010101", "Brasil")
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.RazaoSocial.Should().Be("Empresa Nova");
    }

    [Test]
    public async Task Handle_DuplicatedCnpj_ShouldReturnError()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.CnpjExists(It.IsAny<string>())).ReturnsAsync(true);

        var handler = new CreateClienteHandler(repoMock.Object);
        var request = new CreateClienteRequest
        {
            RazaoSocial = "Empresa Nova",
            NomeFantasia = "Fantasia Nova",
            Cnpj = "12345678000199",
            InscricaoEstadual = "IE",
            Email = "nova@empresa.com",
            Telefone = "11999999999",
            Endereco = new Endereco("Rua Nova", "10", null, "Bairro", "Cidade", "SP", "01010101", "Brasil")
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Id.Should().Be(Guid.Empty);
        result.Message.Should().Be("CNPJ já está cadastrado no sistema.");
    }

    [Test]
    public async Task Handle_ValidRequest_ShouldUpdateCliente()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cliente = new Cliente("Empresa", "Fantasia", new Cnpj("12345678000111"), "IE",
                new Endereco("Rua", "1", null, "Bairro", "Cidade", "SP", "01010101", "Brasil"), "a@a.com", "11999999999");
        typeof(Cliente).GetProperty("Id")?.SetValue(cliente, clienteId);

        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.GetById(clienteId)).ReturnsAsync(cliente);
        repoMock.Setup(r => r.CnpjExistsForOtherCliente(It.IsAny<string>(), clienteId)).ReturnsAsync(false);
        repoMock.Setup(r => r.UpdateCliente(It.IsAny<Cliente>())).ReturnsAsync(cliente);

        var handler = new UpdateClienteHandler(repoMock.Object);
        var request = new UpdateClienteRequest
        {
            Id = clienteId,
            RazaoSocial = "Empresa Atualizada",
            NomeFantasia = "Fantasia Atualizada",
            Cnpj = "12345678000111",
            InscricaoEstadual = "IE",
            Email = "atualizada@empresa.com",
            Telefone = "11999999999",
            Endereco = new Endereco("Rua Atualizada", "2", null, "Bairro", "Cidade", "SP", "01010101", "Brasil")
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(clienteId);
        result.RazaoSocial.Should().Be("Empresa Atualizada");
    }

    [Test]
    public void Handle_ClienteNotFound_ShouldThrow()
    {
        // Arrange
        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Cliente)null);

        var handler = new UpdateClienteHandler(repoMock.Object);
        var request = new UpdateClienteRequest
        {
            Id = Guid.NewGuid(),
            RazaoSocial = "Empresa Atualizada",
            NomeFantasia = "Fantasia Atualizada",
            Cnpj = "12345678000111",
            InscricaoEstadual = "IE",
            Email = "atualizada@empresa.com",
            Telefone = "11999999999",
            Endereco = new Endereco("Rua Atualizada", "2", null, "Bairro", "Cidade", "SP", "01010101", "Brasil")
        };

        // Act
        Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<ArgumentException>().WithMessage("Cliente não encontrado.");
    }

    [Test]
    public void Handle_CnpjExistsForOtherCliente_ShouldThrow()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cliente = new Cliente("Empresa", "Fantasia", new Cnpj("12345678000111"), "IE",
                new Endereco("Rua", "1", null, "Bairro", "Cidade", "SP", "01010101", "Brasil"), "a@a.com", "11999999999");
        typeof(Cliente).GetProperty("Id")?.SetValue(cliente, clienteId);

        var repoMock = new Mock<IClienteRepository>();
        repoMock.Setup(r => r.GetById(clienteId)).ReturnsAsync(cliente);
        repoMock.Setup(r => r.CnpjExistsForOtherCliente(It.IsAny<string>(), clienteId)).ReturnsAsync(true);

        var handler = new UpdateClienteHandler(repoMock.Object);
        var request = new UpdateClienteRequest
        {
            Id = clienteId,
            RazaoSocial = "Empresa Atualizada",
            NomeFantasia = "Fantasia Atualizada",
            Cnpj = "12345678000111",
            InscricaoEstadual = "IE",
            Email = "atualizada@empresa.com",
            Telefone = "11999999999",
            Endereco = new Endereco("Rua Atualizada", "2", null, "Bairro", "Cidade", "SP", "01010101", "Brasil")
        };

        // Act
        Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<ArgumentException>().WithMessage("CNPJ já está cadastrado para outro cliente.");
    }
}

