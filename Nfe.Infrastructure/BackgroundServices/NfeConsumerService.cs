using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nfe.Domain.Contracts.Repositories;
using Nfe.Domain.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Nfe.Application.Services;

public class NfeConsumerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NfeConsumerService> _logger;
    private IConnection _connection;
    private IChannel _channel;
    private const string QueueName = "nfe-authorization-queue";

    public NfeConsumerService(IServiceProvider serviceProvider, ILogger<NfeConsumerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "rabbitmq",
            Password = "rabbitmq",
            Port = 5672
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null).GetAwaiter().GetResult();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                _logger.LogInformation("📥 Mensagem recebida: NFe para processamento");

                var nfeMessage = JsonSerializer.Deserialize<NfeAutorizacaoMessage>(message, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await ProcessarNfeAsync(nfeMessage);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
                _logger.LogInformation("✅ NFe {NumeroNota} processada com sucesso", nfeMessage.NumeroNota);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro ao processar mensagem");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("🚀 NFe Consumer iniciado. Aguardando mensagens...");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ProcessarNfeAsync(NfeAutorizacaoMessage message)
    {
        using var scope = _serviceProvider.CreateScope();
        var nfeRepository = scope.ServiceProvider.GetRequiredService<INfeRepository>();

        var nfe = await nfeRepository.GetById(message.NfeId);
        if (nfe == null)
        {
            _logger.LogWarning("⚠️ NFe não encontrada: {NfeId}", message.NfeId);
            return;
        }

        // Simular processamento da SEFAZ
        _logger.LogInformation($"Processando NFe {message.NumeroNota}...");
        var delay = Random.Shared.Next(2000, 5000);
        await Task.Delay(delay);

        // simulando chance de rejeicao (5%)
        var autorizada = Random.Shared.NextDouble() > 0.05;

        if (autorizada)
        {
            var protocoloAutorizacao = $"135{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
            nfe.Autorizar(protocoloAutorizacao, message.XmlNfe);

            _logger.LogInformation(" NFe {NumeroNota} AUTORIZADA - Protocolo: {Protocolo}",
                message.NumeroNota, protocoloAutorizacao);
        }
        else
        {
            var motivosRejeicao = new[]
            {
                "CPF/CNPJ do destinatário inválido",
                "Código da NCM inexistente",
                "CFOP não permitido na operação",
                "Valor do ICMS inconsistente"
            };

            var motivo = motivosRejeicao[Random.Shared.Next(motivosRejeicao.Length)];
            nfe.Rejeitar($"Rejeição 999: {motivo}");

            _logger.LogWarning("❌ NFe {NumeroNota} REJEITADA - Motivo: {Motivo}",
                message.NumeroNota, motivo);
        }

        await nfeRepository.Update(nfe);
    }

    public override void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        base.Dispose();
    }
}