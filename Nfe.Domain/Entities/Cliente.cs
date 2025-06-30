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
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        Cnpj = cnpj;
        InscricaoEstadual = inscricaoEstadual;
        Endereco = endereco;
        Email = email;
        Telefone = telefone;
    }
}
