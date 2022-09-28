using RabbitMQ.Client;

namespace Common
{
    public static class Helper
    {
        public static IConnection Connection()
        {
            var factory = new ConnectionFactory();
            factory.UserName = "end";
            factory.Password = "end@2222";
            factory.HostName = "localhost";
            var connection = factory.CreateConnection();
            return connection;
        }
    }
}