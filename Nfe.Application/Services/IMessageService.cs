namespace Nfe.Application.Services;

public interface IMessageService
{
    Task SendNfeToProcessingAsync(NfeAutorizacaoMessage message);
}