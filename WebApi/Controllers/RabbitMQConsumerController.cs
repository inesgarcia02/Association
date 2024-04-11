using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WebApi.Controllers
{
    public class RabbitMQConsumerController : IRabbitMQConsumerController 
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQConsumerController()
        {
            _factory = new ConnectionFactory { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            //_channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            _channel.QueueDeclare(queue: "hello",
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
            
            // _channel.QueueBind(queue: "hello",
            //       exchange: "logs",
            //       routingKey: string.Empty);

            Console.WriteLine(" [*] Waiting for messages.");
        }

        public void StartConsuming()
        {         
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: "hello",
                                autoAck: true,
                                consumer: consumer);
        }
    }
}