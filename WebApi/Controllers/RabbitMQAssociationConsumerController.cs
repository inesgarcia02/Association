using Application.DTO;
using Application.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace WebApi.Controllers
{
    public class RabbitMQAssociationConsumerController : IRabbitMQAssociationConsumerController
    {
        private List<string> _errorMessages = new List<string>();
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQAssociationConsumerController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _factory = new ConnectionFactory { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "associationLogs", type: ExchangeType.Fanout);

            _queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: _queueName,
                  exchange: "associationLogs",
                  routingKey: string.Empty);

            Console.WriteLine(" [*] Waiting for messages.");
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received +=  async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                AssociationDTO associationDTO = AssociationAmqpDTO.Deserialize(message);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var associationService = scope.ServiceProvider.GetRequiredService<AssociationService>();

                    //ProjectDTO projectResultDTO = projectService.Add(deserializedObject, _errorMessages).Result;
                    await associationService.Add(associationDTO, _errorMessages);
                }

                Console.WriteLine($" [x] Received {message}");
            };
            _channel.BasicConsume(queue: _queueName,
                                autoAck: true,
                                consumer: consumer);
        }
    }
}