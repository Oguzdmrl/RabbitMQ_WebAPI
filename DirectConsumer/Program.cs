using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace DirectConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = Helper.Connection();
            using (var channel = connection.CreateModel()) // dotnet run admin
            {
                channel.ExchangeDeclare("directexchange", type: "direct");
                string queueName = channel.QueueDeclare().QueueName;
                if (args[0] == "admin")
                    channel.QueueBind(queue: queueName, exchange: "directexchange", routingKey: "admin");
                else
                    channel.QueueBind(queue: queueName, exchange: "directexchange", routingKey: "user");
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queueName, false, consumer);
                consumer.Received += (sender, e) =>
                {
                    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()) + " sayısı alındı.");
                    channel.BasicAck(e.DeliveryTag, false);
                };
                Console.Read();
            }
        }
    }
}