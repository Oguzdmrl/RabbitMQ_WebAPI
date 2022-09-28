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
        public void PublishDirect(string yetki, string message)
        {
            var connection = Helper.Connection();
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("directexchange", type: "direct");
                var byteMsg = Encoding.UTF8.GetBytes(message);
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                if (yetki == "admin")
                    channel.BasicPublish(exchange: "directexchange", routingKey: "admin", basicProperties: properties, body: byteMsg);
                else
                    channel.BasicPublish(exchange: "directexchange", routingKey: "user", basicProperties: properties, body: byteMsg);
            }
        }
        public void PublishTopic()
        {
            var connection = Helper.Connection();
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("topicexchange", type: "topic");
                for (int i = 1; i <= 100; i++)
                {
                    byte[] bytemessage = Encoding.UTF8.GetBytes($"{i}. görev verildi.");
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.BasicPublish(exchange: "topicexchange", routingKey: $"Asker.Subay.{(i % 2 == 0 ? "Yuzbasi" : (i % 11 == 0 ? "Binbasi" : "Tegmen"))}", basicProperties: properties, body: bytemessage);
                }
            }
        }
    }
}