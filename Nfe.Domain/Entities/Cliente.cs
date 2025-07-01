using Nfe.Domain.ValueObjects;

namespace Nfe.Domain.Entities;

public class Cliente
{
    public Guid Id { get; private set; }
    public string RazaoSocial { get; private set; }
    public string NomeFantasia { get; private set; }
    public Cnpj Cnpj { get; private set; }
    public string? InscricaoEstadual { get; private set; }
    public Endereco Endereco { get; private set; }
    public string Email { get; private set; }
    public string Telefone { get; private set; }
    public DateTime DataCriacao { get; private set; }

    private Cliente() { }

    public Cliente(string razaoSocial, string nomeFantasia, Cnpj cnpj, string? inscricaoEstadual, Endereco endereco, string email, string telefone)
    {
        Id = Guid.NewGuid();
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        Cnpj = cnpj;
        InscricaoEstadual = inscricaoEstadual;
        Endereco = endereco;
        Email = email;
        Telefone = telefone;
        DataCriacao = DateTime.UtcNow;
    }

    public Cliente(Guid id, string razaoSocial, string nomeFantasia, Cnpj cnpj, string? inscricaoEstadual,
        Endereco endereco, string email, string telefone, DateTime dataCriacao)
    {
        Id = id;
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        Cnpj = cnpj;
        InscricaoEstadual = inscricaoEstadual;
        Endereco = endereco;
        Email = email;
        Telefone = telefone;
        DataCriacao = dataCriacao;
    }

    public void AtualizarRazaoSocial(string razaoSocial)
    {
        if (string.IsNullOrWhiteSpace(razaoSocial))
            throw new ArgumentException("Razão social não pode ser vazia");

        RazaoSocial = razaoSocial.Trim();
    }

    public void AtualizarNomeFantasia(string nomeFantasia)
    {
        if (string.IsNullOrWhiteSpace(nomeFantasia))
            throw new ArgumentException("Nome fantasia não pode ser vazio");

        NomeFantasia = nomeFantasia.Trim();
    }

    public void AtualizarCnpj(Cnpj cnpj)
    {
        Cnpj = cnpj ?? throw new ArgumentNullException(nameof(cnpj));
    }

    public void AtualizarInscricaoEstadual(string? inscricaoEstadual)
    {
        InscricaoEstadual = inscricaoEstadual?.Trim();
    }

    public void AtualizarEndereco(Endereco endereco)
    {
        Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco));
    }

    public void AtualizarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email não pode ser vazio");

        if (!email.Contains("@"))
            throw new ArgumentException("Email deve ter formato válido");

        Email = email.Trim().ToLower();
    }

    public void AtualizarTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            throw new ArgumentException("Telefone não pode ser vazio");

        Telefone = telefone.Trim();
    }

    public void AtualizarDadosCompletos(string razaoSocial, string nomeFantasia, Cnpj cnpj,
        string? inscricaoEstadual, Endereco endereco, string email, string telefone)
    {
        AtualizarRazaoSocial(razaoSocial);
        AtualizarNomeFantasia(nomeFantasia);
        AtualizarCnpj(cnpj);
        AtualizarInscricaoEstadual(inscricaoEstadual);
        AtualizarEndereco(endereco);
        AtualizarEmail(email);
        AtualizarTelefone(telefone);
    }
}