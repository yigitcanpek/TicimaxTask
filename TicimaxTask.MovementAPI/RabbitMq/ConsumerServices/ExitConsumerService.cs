using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TicimaxTask.BLL.RepositoryPattern.IServices;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.MovementAPI.RabbitMq.GeneralServices;
using TicimaxTask.Shared.Dtos;

namespace TicimaxTask.MovementAPI.RabbitMq.ConsumerServices
{
    public class ExitConsumerService:BackgroundService
    {
        private readonly RabbitMqClientService _rabbitMqClientService;
        private readonly ILogger<EnterConsumerService> _logger;
        private IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public static string ExchangeName = "ExitExchange";
        public static string Routing = "EnterRoute";
        public static string QueueName = "EnterQueue";

        public ExitConsumerService(RabbitMqClientService rabbitMqClientService, ILogger<EnterConsumerService> logger, IServiceScopeFactory serviceScopeFactory)
        {


            _rabbitMqClientService = rabbitMqClientService;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {

            _channel = _rabbitMqClientService.Connect(ExchangeName, Routing, QueueName);
            _channel.BasicQos(0, 1, false); //Size,Much



            return base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            _channel.BasicConsume(QueueName, false, consumer);

            consumer.Received += Consumer_Received;

            return Task.CompletedTask;

        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {


                using (MemoryStream memoryStream = new MemoryStream(@event.Body.ToArray()))
                {
                    using (StreamReader reader = new StreamReader(memoryStream))
                    {

                        try
                        {
                            IBaseService<AppUser> _appUserService = scope.ServiceProvider.GetRequiredService<IBaseService<AppUser>>();
                            ICheckInOutService _checkInOut = scope.ServiceProvider.GetRequiredService<ICheckInOutService>();
                            string json = reader.ReadToEnd();
                            CheckInOut checkOut = JsonConvert.DeserializeObject<CheckInOut>(json);
                            Response<AppUser?> checkOwnerUser = await _appUserService.GetByIdAsync(checkOut.AppUserID);
                            checkOwnerUser.Data.CheckStatus = Entities.Entities.Enums.CheckStatus.CheckOut;
                            _appUserService.Update(checkOwnerUser.Data);
                            checkOut.AppUser = checkOwnerUser.Data;
                            checkOut.CheckType = Entities.Entities.Enums.CheckStatus.CheckOut;
                            Response<bool> response = await _checkInOut.ExitAsync(checkOut);

                            _channel.BasicAck(@event.DeliveryTag, false);

                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }
                }

            }





        }
    }
}
