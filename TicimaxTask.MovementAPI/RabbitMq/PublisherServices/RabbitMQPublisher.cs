using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.MovementAPI.RabbitMq.GeneralServices;

namespace TicimaxTask.MovementAPI.RabbitMq.PublisherServices
{
    public class RabbitMQPublisher
    {

       

        private readonly RabbitMqClientService _rabbitMQClientService;

        public RabbitMQPublisher(RabbitMqClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }

        public void Publish(CheckInOut checkIn, string ExchangeName,string Routing, string QueueName) 
        {
            IModel channel = _rabbitMQClientService.Connect(ExchangeName, Routing, QueueName);

            string bodyString = JsonSerializer.Serialize(checkIn);

            byte[] bodyByte = Encoding.UTF8.GetBytes(bodyString);

            channel.BasicPublish(exchange: ExchangeName,  routingKey:Routing, body: bodyByte);


        }
    }
}
