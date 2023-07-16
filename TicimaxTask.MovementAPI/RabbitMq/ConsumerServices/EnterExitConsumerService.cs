using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using TicimaxTask.Entities.Entities.Models;
using TicimaxTask.MovementAPI.RabbitMq.GeneralServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using TicimaxTask.Shared.Dtos;
using TicimaxTask.BLL.RepositoryPattern.IServices;

namespace TicimaxTask.MovementAPI.RabbitMq.ConsumerServices
{
    public class EnterExitConsumerService:BackgroundService 
    {

        private readonly RabbitMqClientService _rabbitMqClientService;
        private readonly ILogger<EnterExitConsumerService> _logger;
        private IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public static string ExchangeName = "EnterExitExchange";
        public static string Routing = "EnterExitRoute";
        public static string QueueName = "EnterExitQueue";

        public EnterExitConsumerService(RabbitMqClientService rabbitMqClientService, ILogger<EnterExitConsumerService> logger, IServiceScopeFactory serviceScopeFactory)
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
            _channel.BasicConsume(QueueName,false, consumer);

            consumer.Received += Consumer_Received;

            return Task.CompletedTask;

        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            using( var scope = _serviceScopeFactory.CreateScope()) 
            {
               

                using (MemoryStream memoryStream = new MemoryStream(@event.Body.ToArray()))
                {
                    using (StreamReader reader = new StreamReader(memoryStream))
                    {
                      
                        try
                        {
                            IBaseService<AppUser> _appUserService = scope.ServiceProvider.GetRequiredService<IBaseService<AppUser>>();
                            ICheckInOutService _checkInOutService = scope.ServiceProvider.GetRequiredService<ICheckInOutService>();
                            string json = reader.ReadToEnd();
                            CheckInOut checkInOut = JsonConvert.DeserializeObject<CheckInOut>(json);
                            Response<AppUser?> checkOwnerUser = await _appUserService.GetByIdAsync(checkInOut.AppUserID);
                           
                            if (checkInOut.CheckType == Entities.Entities.Enums.CheckStatus.CheckIn)
                            {
                                checkOwnerUser.Data.CheckStatus = Entities.Entities.Enums.CheckStatus.CheckIn;
                                _appUserService.Update(checkOwnerUser.Data);
                                checkInOut.AppUser = checkOwnerUser.Data;
                                await _checkInOutService.EnterAsync(checkInOut);
                            }
                            else
                            {
                                checkOwnerUser.Data.CheckStatus = Entities.Entities.Enums.CheckStatus.CheckOut;
                                _appUserService.Update(checkOwnerUser.Data);
                                checkInOut.AppUser = checkOwnerUser.Data;
                                await _checkInOutService.ExitAsync(checkInOut);
                            }
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

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
