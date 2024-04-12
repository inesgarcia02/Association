using Application.DTO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Application.Services;
using DataModel.Repository;
using Domain.Model;

namespace WebApi.Controllers
{
    public class RabbitMQConsumerController : IRabbitMQConsumerController 
    {
        private readonly AssociationService _associationService;

        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQConsumerController(AssociationService associationService)
        {
            _associationService = associationService;
            // _contextFactory = contextFactory;

            _factory = new ConnectionFactory { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            _queueName = _channel.QueueDeclare().QueueName;
            
            _channel.QueueBind(queue: _queueName,
                  exchange: "logs",
                  routingKey: string.Empty);

            Console.WriteLine(" [*] Waiting for messages.");
        }

        public void StartConsuming()
        {         
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                AssociationDTO associationDTO = AssociationAmqpDTO.Deserialize(message);
                Console.WriteLine($" [x] Received {message}");

                // await _associationService.HandleMessage(associationDTO);
            };
            _channel.BasicConsume(queue: _queueName,
                                autoAck: true,
                                consumer: consumer);
        }
    }
}