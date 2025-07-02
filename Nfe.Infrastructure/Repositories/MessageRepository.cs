using Nfe.Domain.Messages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Nfe.Application.Services;

public class MessageRepository : IMessageRepository
{
    private const string QueueName = "nfe-authorization-queue";

    public async Task SendMessageAsync(NfeAutorizacaoMessage message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "rabbitmq",
            Password = "rabbitmq",
            Port = 5672
        };

        try
        {
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var messageJson = JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var body = Encoding.UTF8.GetBytes(messageJson);

            var properties = new BasicProperties()
            {
                Persistent = true, //mensagem persistente
                MessageId = message.NfeId.ToString(),
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            // publicar a mensagem
            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: QueueName,
                mandatory: false,
                basicProperties: properties,
                body: body);

            Console.WriteLine($" Enviada NFe {message.NumeroNota} para processamento (ID: {message.NfeId})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            throw;
        }
    }
}
