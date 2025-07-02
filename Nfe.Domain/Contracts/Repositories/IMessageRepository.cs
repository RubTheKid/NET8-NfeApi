using Nfe.Domain.Messages;

namespace Nfe.Application.Services;

public interface IMessageRepository
{
    Task SendMessageAsync(NfeAutorizacaoMessage message);
}
