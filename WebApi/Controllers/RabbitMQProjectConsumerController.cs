using Application.DTO;
using Application.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace WebApi.Controllers
{
    public class RabbitMQProjectConsumerController : IRabbitMQProjectConsumerController
    {
        private List<string> _errorMessages = new List<string>();
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQProjectConsumerController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _factory = new ConnectionFactory { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "project", type: ExchangeType.Fanout);

            _queueName = _channel.QueueDeclare().QueueName;


            _channel.QueueBind(queue: _queueName,
            exchange: "project",
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

                ProjectDTO projectDTO = ProjectAmqpDTO.ToDTO(message);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var projectService = scope.ServiceProvider.GetRequiredService<ProjectService>();

                    projectService.Add(projectDTO.Id);
                }

                Console.WriteLine($" [x] Received {message}");
            };
            _channel.BasicConsume(queue: _queueName,
                                autoAck: true,
                                consumer: consumer);
        }
    }
}