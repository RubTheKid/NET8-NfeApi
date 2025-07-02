using MediatR;
using Nfe.Domain.Contracts.Repositories;

namespace Nfe.Application.Features.NotaFiscal.Query.GetNfeById;

public class GetNfeByIdHandler : IRequestHandler<GetNfeByIdRequest, GetNfeByIdResponse?>
{
    private readonly INfeRepository _nfeRepository;

    public GetNfeByIdHandler(INfeRepository nfeRepository)
    {
        _nfeRepository = nfeRepository;
    }

    public async Task<GetNfeByIdResponse?> Handle(GetNfeByIdRequest request, CancellationToken cancellationToken)
    {
        var nfe = await _nfeRepository.GetById(request.id);

        if (nfe == null)
            return null;

        return new GetNfeByIdResponse
        {
            Id = nfe.Id,
            NumeroNota = nfe.NumeroNota,
            Serie = nfe.Serie,
            ChaveAcesso = nfe.ChaveAcesso,
            Status = nfe.Status.ToString(),
            DataEmissao = nfe.DataEmissao,
            DataAutorizacao = nfe.DataAutorizacao,
            ProtocoloAutorizacao = nfe.ProtocoloAutorizacao,
            MotivoRejeicao = nfe.MotivoRejeicao,
            ValorTotal = nfe.ValorTotal,
            ValorIcms = nfe.ValorIcms,
            ValorIpi = nfe.ValorIpi,
            ValorPis = nfe.ValorPis,
            ValorCofins = nfe.ValorCofins,
            Emitente = new Emitente
            {
                Id = nfe.Emitente.Id,
                RazaoSocial = nfe.Emitente.RazaoSocial,
                Cnpj = nfe.Emitente.Cnpj.Numero
            },
            Destinatario = new Destinatario
            {
                Id = nfe.Destinatario.Id,
                RazaoSocial = nfe.Destinatario.RazaoSocial,
                Cnpj = nfe.Destinatario.Cnpj.Numero
            }
        };
    }
}