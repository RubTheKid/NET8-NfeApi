namespace Nfe.Domain.ValueObjects;

public record Telefone
{
    public string Numero { get; }

    public Telefone(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new ArgumentException("Telefone não pode ser vazio");

        var telefoneNumerico = ExtrairNumeros(numero);

        if (telefoneNumerico.Length < 10 || telefoneNumerico.Length > 11)
            throw new ArgumentException("Telefone deve ter 10 ou 11 dígitos");

        Numero = telefoneNumerico;
    }

    private static string ExtrairNumeros(string telefone)
    {
        return new string(telefone.Where(char.IsDigit).ToArray());
    }

    public string FormatarTelefone()
    {
        if (Numero.Length == 10)
            return $"({Numero.Substring(0, 2)}) {Numero.Substring(2, 4)}-{Numero.Substring(6, 4)}";
        else
            return $"({Numero.Substring(0, 2)}) {Numero.Substring(2, 5)}-{Numero.Substring(7, 4)}";
    }

    public override string ToString() => FormatarTelefone();
}
