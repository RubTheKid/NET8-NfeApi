namespace Nfe.Domain.Entities;

public class Produto
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public string CodigoBarras { get; private set; }
    public string Ncm { get; private set; }
    public string UnidadeMedida { get; private set; }
    public decimal Preco { get; private set; }
    public decimal Peso { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public bool Ativo { get; private set; }

    private Produto() { }

    public Produto(string nome, string descricao, string codigoBarras, string ncm,
                   string unidadeMedida, decimal preco, decimal peso)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
        CodigoBarras = codigoBarras;
        Ncm = ncm;
        UnidadeMedida = unidadeMedida;
        Preco = preco;
        Peso = peso;
        DataCriacao = DateTime.UtcNow;
        Ativo = true;
    }

    public void AtualizarPreco(decimal novoPreco)
    {
        if (novoPreco <= 0)
            throw new ArgumentException("Preço deve ser maior que zero");

        Preco = novoPreco;
    }

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
}
