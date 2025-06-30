using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nfe.Domain.ValueObjects;

public record Endereco(
    string Logradouro,
    string Numero,
    string? Complemento,
    string Bairro,
    string Cidade,
    string Estado,
    string Cep,
    string Pais = "Brasil")
{
    public string EnderecoCompleto =>
        $"{Logradouro}, {Numero}" +
        (string.IsNullOrEmpty(Complemento) ? "" : $", {Complemento}") +
        $", {Bairro}, {Cidade} - {Estado}, {Cep}, {Pais}";
}
