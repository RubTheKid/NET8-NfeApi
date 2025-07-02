using MediatR;
using Nfe.Application.Services;
using Nfe.Domain.Contracts.Repositories;
using DomainEntity = Nfe.Domain.Entities;

namespace Nfe.Application.Features.NotaFiscal.Command.SendNfeToAuthorization;

public class SendNfeToAuthorizationHandler : IRequestHandler<SendNfeToAuthorizationRequest, SendNfeToAuthorizationResponse>
{
    private readonly INfeRepository _nfeRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly INfeXmlService _xmlService;
    private readonly IMessageService _messageService;

    public SendNfeToAuthorizationHandler(
        INfeRepository nfeRepository,
        IClienteRepository clienteRepository,
        INfeXmlService xmlService,
        IMessageService messageService)
    {
        _nfeRepository = nfeRepository;
        _clienteRepository = clienteRepository;
        _xmlService = xmlService;
        _messageService = messageService;
    }

    public async Task<SendNfeToAuthorizationResponse> Handle(SendNfeToAuthorizationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(request.NumeroNota))
            {
                return new SendNfeToAuthorizationResponse
                {
                    Success = false,
                    Message = "Número da nota é obrigatório"
                };
            }

            if (string.IsNullOrWhiteSpace(request.Serie))
            {
                return new SendNfeToAuthorizationResponse
                {
                    Success = false,
                    Message = "Série da nota é obrigatória"
                };
            }

            if (!request.Itens.Any())
            {
                return new SendNfeToAuthorizationResponse
                {
                    Success = false,
                    Message = "A nota fiscal deve conter pelo menos um item"
                };
            }

            // Validar se emitente e destinatário existem
            var emitente = await _clienteRepository.GetById(request.EmitenteId);
            if (emitente == null)
            {
                return new SendNfeToAuthorizationResponse
                {
                    Success = false,
                    Message = "Emitente não encontrado"
                };
            }

            var destinatario = await _clienteRepository.GetById(request.DestinatarioId);
            if (destinatario == null)
            {
                return new SendNfeToAuthorizationResponse
                {
                    Success = false,
                    Message = "Destinatário não encontrado"
                };
            }

            // Criar a nota fiscal
            var notaFiscal = new DomainEntity.NotaFiscal(request.NumeroNota, request.Serie, request.EmitenteId, request.DestinatarioId);

            // Adicionar itens
            foreach (var itemDto in request.Itens)
            {
                var item = new DomainEntity.ItemNfe(
                    notaFiscal.Id,
                    itemDto.ProdutoId,
                    itemDto.Sequencia,
                    itemDto.Quantidade,
                    itemDto.ValorUnitario,
                    itemDto.Cfop,
                    itemDto.Cst
                );
                notaFiscal.AdicionarItem(item);
            }

            // Salvar a nota fiscal
            await _nfeRepository.Add(notaFiscal);

            // Converter para XML
            var xmlNfe = await _xmlService.ConvertToXmlAsync(notaFiscal);

            // Criar mensagem para RabbitMQ
            var message = new NfeAutorizacaoMessage
            {
                NfeId = notaFiscal.Id,
                NumeroNota = notaFiscal.NumeroNota,
                Serie = notaFiscal.Serie,
                ChaveAcesso = notaFiscal.ChaveAcesso,
                EmitenteId = notaFiscal.EmitenteId,
                DestinatarioId = notaFiscal.DestinatarioId,
                ValorTotal = notaFiscal.ValorTotal,
                DataEnvio = DateTime.UtcNow,
                XmlNfe = xmlNfe
            };

            // Enviar para fila de processamento
            await _messageService.SendNfeToProcessingAsync(message);

            return new SendNfeToAuthorizationResponse
            {
                Success = true,
                NfeId = notaFiscal.Id,
                NumeroNota = notaFiscal.NumeroNota,
                Serie = notaFiscal.Serie,
                ChaveAcesso = notaFiscal.ChaveAcesso,
                DataEmissao = notaFiscal.DataEmissao,
                Status = notaFiscal.Status.ToString(),
                ValorTotal = notaFiscal.ValorTotal,
                Message = "NFe criada e enviada para processamento com sucesso"
            };
        }
        catch (Exception ex)
        {
            return new SendNfeToAuthorizationResponse
            {
                Success = false,
                Message = $"Erro ao processar NFe: {ex.Message}"
            };
        }
    }
}