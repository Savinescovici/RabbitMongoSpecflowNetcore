using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rabbit
{
    public interface IRabbitService
    {
        void Enqueue(string message);
    }

    public class RabbitService : IRabbitService, IDisposable
    {
        IConnection _conn;
        IModel _channel;
        public RabbitService(IOptions<RabbitSettings> rabbitSettings, IRabbitConnectionFactory rabbitConnectionFactory)
        {

            _conn = rabbitConnectionFactory.CreateConnection(rabbitSettings.Value);
            _channel = _conn.CreateModel();
            _channel.ExchangeDeclare(exchange: "users", type: ExchangeType.Fanout);

        }

        public void Enqueue(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "users",
                                routingKey: "",
                                basicProperties: null,
                                body: body);
            Console.WriteLine(" [x] Published {0} to users exchange", message);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _conn.Dispose();
        }
    }
}
