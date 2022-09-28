using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace TopicConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = Helper.Connection();
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("topicexchange", type: "topic");
                string queueName = channel.QueueDeclare().QueueName;
                string routingKey = "";
                routingKey = args[0] switch
                {
                    "1" => $"*.*.Tegmen",
                    "2" => $"*.#.Yuzbasi",
                    "3" => $"#.Binbasi.#",
                    "4" => $"Asker.Subay.Tegmen",
                };
                channel.QueueBind(queue: queueName, exchange: "topicexchange", routingKey: routingKey);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queueName, false, consumer);
                consumer.Received += (sender, e) =>
                {
                    Console.WriteLine($"{routingKey} {Encoding.UTF8.GetString(e.Body.ToArray())} görevi aldı.");
                    channel.BasicAck(e.DeliveryTag, false);
                };
                Console.Read();
            }
        }
    }
}