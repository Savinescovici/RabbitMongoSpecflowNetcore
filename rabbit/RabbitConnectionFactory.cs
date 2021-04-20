using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace rabbit
{
    public interface IRabbitConnectionFactory
    {
        IConnection CreateConnection(RabbitSettings rabbitSettings);
    }
    public class RabbitConnectionFactory : IRabbitConnectionFactory
    {
        public IConnection CreateConnection(RabbitSettings rabbitSettings)
        {
            var factory = new ConnectionFactory()
            {
                HostName = rabbitSettings.HostName,
                Port = rabbitSettings.Port,
                UserName = rabbitSettings.UserName,
                Password = rabbitSettings.Password
            };

            var conn = factory.CreateConnection();
            return conn;
        }
    }
}
