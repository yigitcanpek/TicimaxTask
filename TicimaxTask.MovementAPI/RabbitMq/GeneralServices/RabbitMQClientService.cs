using RabbitMQ.Client;

namespace TicimaxTask.MovementAPI.RabbitMq.GeneralServices
{
    public class RabbitMqClientService:IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger<RabbitMqClientService> _logger;

        public RabbitMqClientService(ConnectionFactory connectionFactory,ILogger<RabbitMqClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public IModel Connect(string ExchangeName, string Routing , string QueueName)
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channel is { IsOpen:true})
            {
                return _channel;
            }

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, type: "direct", true,false);
            _channel.QueueDeclare(QueueName, true, false, false, null);

            _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: Routing);
            _logger.LogInformation("Rabbitmq connection has begin");
            return _channel;

        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();



            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("Rabbitmq connection has begin");
        }
    }
}
