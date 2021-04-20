using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rabbit
{
    public class RabbitWorker : IDisposable, IHostedService
    {
        IConnection _conn;
        IModel _channel;
        public RabbitWorker(IOptions<RabbitSettings> rabbitSettings, IRabbitConnectionFactory rabbitConnectionFactory)
        {

            _conn = rabbitConnectionFactory.CreateConnection(rabbitSettings.Value);
            _channel = _conn.CreateModel();
            _channel.ExchangeDeclare(exchange: "users", type: ExchangeType.Fanout);
        }

        public void Consume()
        {
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                              exchange: "users",
                              routingKey: "");

            Console.WriteLine(" [*] Waiting for users.");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

        }

        public void Dispose()
        {
            _channel.Dispose();
            _conn.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Consume();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
