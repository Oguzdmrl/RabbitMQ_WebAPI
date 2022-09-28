using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;

namespace Common.Repository
{
    public class RabbitMQRepo
    {
        public void PublishFanout(string message)
        {
            var connection = Helper.Connection();
            using (var channel = connection.CreateModel())
            {
                var msg = Encoding.UTF8.GetBytes(message);
                channel.ExchangeDeclare(exchange: "logs", durable: true, type: "fanout");
                var propertis = channel.CreateBasicProperties();
                propertis.Persistent = true;
                channel.BasicPublish("logs", routingKey: "", propertis, body: msg);
            }
        }
        public void PublishHeader(string args, string byteMessage)
        {
            var connection = Helper.Connection();
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("headerexchange", type: ExchangeType.Headers);
                var msg = Encoding.UTF8.GetBytes(byteMessage);
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.Headers = new Dictionary<string, object>()
                {
                    ["keyNo"] = args == "1" ? "666" : "2345"
                };
                channel.BasicPublish(exchange: "headerexchange", routingKey: string.Empty, basicProperties: properties, body: msg);
            }
        }
    }
}