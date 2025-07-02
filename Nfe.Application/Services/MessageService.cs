using Nfe.Domain.Messages;

namespace Nfe.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;

    public MessageService(IMessageRepository repository)
    {
        _repository = repository;
    }

    public async Task SendNfeToProcessingAsync(NfeAutorizacaoMessage message)
    {
        await _repository.SendMessageAsync(message);
    }
}
