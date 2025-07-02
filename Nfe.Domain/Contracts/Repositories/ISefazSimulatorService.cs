using Nfe.Domain.Messages;

namespace Nfe.Application.Services;

public interface ISefazSimulatorService
{
    Task<NfeAutorizacaoResultMessage> ProcessarAutorizacao(NfeAutorizacaoMessage message);
}
