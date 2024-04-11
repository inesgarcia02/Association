namespace Gateway;
using System.Text;
using System.Text.Json;
using Application.DTO;
using Domain.Model;
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

    public void Publish(Association association)
    {
        AssociationDTO associationDTO = AssociationDTO.ToDTO(association);
        var jsonMessage = JsonSerializer.Serialize(associationDTO); 
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        _channel.BasicPublish(exchange: "logs",
                              routingKey: string.Empty,
                              basicProperties: null,
                              body: body);
    }


}
