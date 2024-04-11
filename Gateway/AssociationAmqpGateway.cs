namespace Gateway;
using System.Text;
using RabbitMQ.Client;


public class AssociationAmqpGateway
{
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    public AssociationAmqpGateway()
    {
        _factory = new ConnectionFactory { HostName = "localhost" };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
    }

    public void Publish()
    {
        var message = "hello world";
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "logs",
                             routingKey: string.Empty,
                             basicProperties: null,
                             body: body);
    }
}
