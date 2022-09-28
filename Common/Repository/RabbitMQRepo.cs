using RabbitMQ.Client;
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
    }
}