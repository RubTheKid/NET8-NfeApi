using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nfe.Domain.ValueObjects;

public record Cnpj
{
    public string Numero { get; }

    public Cnpj(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new ArgumentException("CNPJ não pode ser vazio");

        var cnpjNumerico = ExtrairNumeros(numero);

        if (!ValidarCnpj(cnpjNumerico))
            throw new ArgumentException("CNPJ inválido");

        Numero = cnpjNumerico;
    }

    private static string ExtrairNumeros(string cnpj)
    {
        return new string(cnpj.Where(char.IsDigit).ToArray());
    }

    private static bool ValidarCnpj(string cnpj)
    {
        if (cnpj.Length != 14)
            return false;

        // validacao simplificada do cnpj
        return !cnpj.All(c => c == cnpj[0]);
    }

    public string FormatarCnpj()
    {
        return $"{Numero.Substring(0, 2)}.{Numero.Substring(2, 3)}.{Numero.Substring(5, 3)}/{Numero.Substring(8, 4)}-{Numero.Substring(12, 2)}";
    }

    public override string ToString() => FormatarCnpj();
}