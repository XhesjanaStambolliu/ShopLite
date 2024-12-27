using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShopLite.Services.RewardAPI.Message;
using ShopLite.Services.RewardAPI.Services;
using System.Text;

namespace ShopLite.Services.RewardAPI.Messaging
{
    public class RabbitMQOrderConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        //private readonly EmailService _emailService;
        private readonly RewardService _rewardService;
        private IConnection _connection;
        private IModel _channel;
        private const string OrderCreated_RewardsUpdateQueue = "RewardsUpdateQueue";
        private string ExchangeName = "";

        public RabbitMQOrderConsumer(IConfiguration configuration, RewardService rewardService)
        {
            _configuration = configuration;
            _rewardService = rewardService;
            ExchangeName = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");


            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare( ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(OrderCreated_RewardsUpdateQueue, false, false, false, null);
            _channel.QueueBind(OrderCreated_RewardsUpdateQueue,ExchangeName, "RewardsUpdate");

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // Lexo përmbajtjen e mesazhit
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                RewardsMessage rewardsMessage = JsonConvert.DeserializeObject<RewardsMessage>(content);

                // Thirr metodën për trajtimin e mesazhit
                HandleMessage(rewardsMessage).GetAwaiter().GetResult();

                // Konfirmo që mesazhi është konsumuar
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            // Fillimi i konsumimit të mesazheve
            _channel.BasicConsume(OrderCreated_RewardsUpdateQueue, false, consumer);
            //_channel.BasicConsume(_configuration.GetValue<string>(queueName), false,consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(RewardsMessage rewardsMessage)
        {
            // Thirrja e metodës për regjistrimin e email-it dhe logimin
            _rewardService.UpdateRewards(rewardsMessage).GetAwaiter().GetResult();
        }
    }
}
