using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace HeaderConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = Helper.Connection(); // dotnet run 1
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("headerexchange", type: "headers");
                channel.QueueDeclare($"kuyruk-{args[0]}", false, false, false, null);
                channel.QueueBind(queue: $"kuyruk-{args[0]}", exchange: "headerexchange", routingKey: string.Empty, new Dictionary<string, object>
                {
                    ["x-match"] = "all",
                    ["keyNo"] = args[0] == "1" ? "666" : "2345",
                });
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume($"kuyruk-{args[0]}", false, consumer);
                consumer.Received += (sender, e) =>
                {
                    Console.WriteLine($"{Encoding.UTF8.GetString(e.Body.ToArray())}. mesaj");
                    channel.BasicAck(e.DeliveryTag, false);
                };
                Console.Read();
            }
        }
    }
}