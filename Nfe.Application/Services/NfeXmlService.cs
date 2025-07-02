using Nfe.Domain.Entities;
using System.Xml.Linq;

namespace Nfe.Application.Services;

public class NfeXmlService : INfeXmlService
{
    private static readonly XNamespace NfeNamespace = "http://www.portalfiscal.inf.br/nfe";

    public async Task<string> ConvertToXmlAsync(NotaFiscal notaFiscal)
    {
        return await Task.Run(() =>
        {
            var xml = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement(NfeNamespace + "nfeProc",
                    new XAttribute("versao", "4.00"),
                    new XElement(NfeNamespace + "NFe",
                        new XElement(NfeNamespace + "infNFe",
                            new XAttribute("Id", $"NFe{notaFiscal.ChaveAcesso}"),
                            CriarIde(notaFiscal),
                            CriarEmitente(),
                            CriarDestinatario(),
                            CriarItens(notaFiscal),
                            CriarTotais(notaFiscal),
                            CriarTransporte(),
                            CriarInformacoesAdicionais()
                        )
                    )
                )
            );

            return xml.ToString();
        });
    }

    private static XElement CriarIde(NotaFiscal notaFiscal)
    {
        return new XElement(NfeNamespace + "ide",
            new XElement(NfeNamespace + "cUF", "35"),
            new XElement(NfeNamespace + "cNF", notaFiscal.ChaveAcesso.Substring(35, 8)),
            new XElement(NfeNamespace + "natOp", "Venda"),
            new XElement(NfeNamespace + "mod", "55"),
            new XElement(NfeNamespace + "serie", notaFiscal.Serie),
            new XElement(NfeNamespace + "nNF", notaFiscal.NumeroNota),
            new XElement(NfeNamespace + "dhEmi", notaFiscal.DataEmissao.ToString("yyyy-MM-ddTHH:mm:sszzz")),
            new XElement(NfeNamespace + "tpNF", "1"),
            new XElement(NfeNamespace + "idDest", "1"),
            new XElement(NfeNamespace + "cMunFG", "3550308"),
            new XElement(NfeNamespace + "tpImp", "1"),
            new XElement(NfeNamespace + "tpEmis", "1"),
            new XElement(NfeNamespace + "cDV", notaFiscal.ChaveAcesso.Substring(43, 1)),
            new XElement(NfeNamespace + "tpAmb", "2"),
            new XElement(NfeNamespace + "finNFe", "1"),
            new XElement(NfeNamespace + "indFinal", "0"),
            new XElement(NfeNamespace + "indPres", "1")
        );
    }

    private static XElement CriarEmitente()
    {
        return new XElement(NfeNamespace + "emit",
            new XElement(NfeNamespace + "CNPJ", "12345678000123"),
            new XElement(NfeNamespace + "xNome", "Empresa Emitente LTDA")
        );
    }

    private static XElement CriarDestinatario()
    {
        return new XElement(NfeNamespace + "dest",
            new XElement(NfeNamespace + "CNPJ", "98765432000198"),
            new XElement(NfeNamespace + "xNome", "Cliente Destinatario LTDA")
        );
    }

    private static XElement[] CriarItens(NotaFiscal notaFiscal)
    {
        return notaFiscal.Itens.Select((item, index) =>
            new XElement(NfeNamespace + "det",
                new XAttribute("nItem", (index + 1).ToString()),
                new XElement(NfeNamespace + "prod",
                    new XElement(NfeNamespace + "cProd", item.ProdutoId.ToString()),
                    new XElement(NfeNamespace + "cEAN", ""),
                    new XElement(NfeNamespace + "xProd", $"Produto {item.ProdutoId}"),
                    new XElement(NfeNamespace + "NCM", "12345678"),
                    new XElement(NfeNamespace + "CFOP", item.Cfop),
                    new XElement(NfeNamespace + "uCom", "UN"),
                    new XElement(NfeNamespace + "qCom", item.Quantidade.ToString("F4")),
                    new XElement(NfeNamespace + "vUnCom", item.ValorUnitario.ToString("F2")),
                    new XElement(NfeNamespace + "vProd", item.ValorTotal.ToString("F2")),
                    new XElement(NfeNamespace + "cEANTrib", ""),
                    new XElement(NfeNamespace + "uTrib", "UN"),
                    new XElement(NfeNamespace + "qTrib", item.Quantidade.ToString("F4")),
                    new XElement(NfeNamespace + "vUnTrib", item.ValorUnitario.ToString("F2"))
                ),
                new XElement(NfeNamespace + "imposto",
                    new XElement(NfeNamespace + "ICMS",
                        new XElement(NfeNamespace + "ICMS00",
                            new XElement(NfeNamespace + "orig", "0"),
                            new XElement(NfeNamespace + "CST", "00"),
                            new XElement(NfeNamespace + "modBC", "3"),
                            new XElement(NfeNamespace + "vBC", item.ValorTotal.ToString("F2")),
                            new XElement(NfeNamespace + "pICMS", "18.00"),
                            new XElement(NfeNamespace + "vICMS", item.ValorIcms.ToString("F2"))
                        )
                    )
                )
            )
        ).ToArray();
    }

    private static XElement CriarTotais(NotaFiscal notaFiscal)
    {
        return new XElement(NfeNamespace + "total",
            new XElement(NfeNamespace + "ICMSTot",
                new XElement(NfeNamespace + "vBC", notaFiscal.ValorTotal.ToString("F2")),
                new XElement(NfeNamespace + "vICMS", notaFiscal.ValorIcms.ToString("F2")),
                new XElement(NfeNamespace + "vICMSDeson", "0.00"),
                new XElement(NfeNamespace + "vFCP", "0.00"),
                new XElement(NfeNamespace + "vBCST", "0.00"),
                new XElement(NfeNamespace + "vST", "0.00"),
                new XElement(NfeNamespace + "vFCPST", "0.00"),
                new XElement(NfeNamespace + "vFCPSTRet", "0.00"),
                new XElement(NfeNamespace + "vProd", notaFiscal.ValorTotal.ToString("F2")),
                new XElement(NfeNamespace + "vFrete", "0.00"),
                new XElement(NfeNamespace + "vSeg", "0.00"),
                new XElement(NfeNamespace + "vDesc", "0.00"),
                new XElement(NfeNamespace + "vII", "0.00"),
                new XElement(NfeNamespace + "vIPI", notaFiscal.ValorIpi.ToString("F2")),
                new XElement(NfeNamespace + "vIPIDevol", "0.00"),
                new XElement(NfeNamespace + "vPIS", notaFiscal.ValorPis.ToString("F2")),
                new XElement(NfeNamespace + "vCOFINS", notaFiscal.ValorCofins.ToString("F2")),
                new XElement(NfeNamespace + "vOutro", "0.00"),
                new XElement(NfeNamespace + "vNF", notaFiscal.ValorTotal.ToString("F2"))
            )
        );
    }

    private static XElement CriarTransporte()
    {
        return new XElement(NfeNamespace + "transp",
            new XElement(NfeNamespace + "modFrete", "9")
        );
    }

    private static XElement CriarInformacoesAdicionais()
    {
        return new XElement(NfeNamespace + "infAdic",
            new XElement(NfeNamespace + "infCpl", "Nota Fiscal gerada automaticamente")
        );
    }
}