namespace Nfe.Application.Services;

public interface IMessageRepository
{
    Task SendMessageAsync(NfeAutorizacaoMessage message);
}
