using Application.DTO;
using Application.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace WebApi.Controllers
{
    public class RabbitMQConsumerController : IRabbitMQConsumerController
    {
        private List<string> _errorMessages = new List<string>();
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQConsumerController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _factory = new ConnectionFactory { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "associationLogs", type: ExchangeType.Fanout);
            _channel.ExchangeDeclare(exchange: "project", type: ExchangeType.Fanout);
            _channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            _queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: _queueName,
                  exchange: "associationLogs",
                  routingKey: string.Empty);

            _channel.QueueBind(queue: _queueName,
            exchange: "project",
            exchange: "project",
            routingKey: string.Empty);

            _channel.QueueBind(queue: _queueName,
            exchange: "logs",
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

                var source = GetSource(message);

                switch (source)
                {
                    case "Project":
                        ProjectDTO projectDTO = ProjectAmqpDTO.ToDTO(message);
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var projectService = scope.ServiceProvider.GetRequiredService<ProjectService>();

                            projectService.Add(projectDTO.Id);
                        }
                        break;
                    case "Association":
                        AssociationDTO associationDTO = AssociationAmqpDTO.Deserialize(message);
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var associationService = scope.ServiceProvider.GetRequiredService<AssociationService>();

                            //ProjectDTO projectResultDTO = projectService.Add(deserializedObject, _errorMessages).Result;
                            await associationService.Add(associationDTO, _errorMessages);
                        }
                        break;

                    case "Colaborator":
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var colaboratorService = scope.ServiceProvider.GetRequiredService<ColaboratorIdService>();
                            await colaboratorService.Add(long.Parse(message));
                        }
                        break;
                }

                Console.WriteLine($" [x] Received {message}");
            };
            _channel.BasicConsume(queue: _queueName,
                                autoAck: true,
                                consumer: consumer);
        }

        public string GetSource(string message)
        {
            if (message.Contains("Identifier\":\"Project"))
            {
                return "Project";
            }
            else /*if (message.Contains("id:"))*/
            {
                return "Colaborator";
            }

            return "Association";
        }
    }
}