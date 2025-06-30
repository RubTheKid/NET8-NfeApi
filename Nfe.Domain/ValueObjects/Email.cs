using System.Text.RegularExpressions;

namespace Nfe.Domain.ValueObjects;

public record Email
{
    public string Endereco { get; }

    public Email(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco))
            throw new ArgumentException("Email não pode ser vazio");

        if (!ValidarEmail(endereco))
            throw new ArgumentException("Email inválido");

        Endereco = endereco.Trim().ToLowerInvariant();
    }

    private static bool ValidarEmail(string email)
    {
        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    public override string ToString() => Endereco;
}
