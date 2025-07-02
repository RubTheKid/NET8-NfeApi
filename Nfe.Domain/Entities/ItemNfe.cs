namespace Nfe.Domain.Entities;

public class ItemNfe
{
    public Guid Id { get; private set; }
    public Guid NotaFiscalId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public virtual NotaFiscal NotaFiscal { get; private set; } = null!;
    public virtual Produto Produto { get; private set; } = null!;

    public int Sequencia { get; private set; }
    public decimal Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }
    public decimal ValorTotal { get; private set; }
    public decimal ValorIcms { get; private set; }
    public decimal ValorIpi { get; private set; }
    public decimal ValorPis { get; private set; }
    public decimal ValorCofins { get; private set; }
    public string Cfop { get; private set; }
    public string Cst { get; private set; }

    private ItemNfe() { }

    public ItemNfe(Guid notaFiscalId, Guid produtoId, int sequencia, decimal quantidade,
                   decimal valorUnitario, string cfop, string cst)
    {
        Id = Guid.NewGuid();
        NotaFiscalId = notaFiscalId;
        ProdutoId = produtoId;
        Sequencia = sequencia;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
        Cfop = cfop;
        Cst = cst;

        CalcularValorTotal();
        CalcularImpostos();
    }

    private void CalcularValorTotal()
    {
        ValorTotal = Quantidade * ValorUnitario;
    }

    private void CalcularImpostos()
    {
        // simulacao do calculo do imposto
        ValorIcms = ValorTotal * 0.18m;
        ValorIpi = ValorTotal * 0.05m;
        ValorPis = ValorTotal * 0.0165m;
        ValorCofins = ValorTotal * 0.076m;
    }
}
