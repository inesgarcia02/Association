namespace Gateway;
using System.Text;
using RabbitMQ.Client;


public class AssociationAmqpGateway
{
    public AssociationAmqpGateway()
    {
    }

    public void Publish()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

        var message = "hello world";
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "logs",
                             routingKey: string.Empty,
                             basicProperties: null,
                             body: body);

    }
}
