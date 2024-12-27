using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShopLite.Services.EmailAPI.Models.Dto;
using ShopLite.Services.EmailAPI.Services;
using System.Text;

namespace ShopLite.Services.EmailAPI.Messaging
{
    public class RabbitMQCartConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQCartConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"),
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // Lexo përmbajtjen e mesazhit
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(content);

                // Thirr metodën për trajtimin e mesazhit
                HandleMessage(cartDto).GetAwaiter().GetResult();

                // Konfirmo që mesazhi është konsumuar
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            // Fillimi i konsumimit të mesazheve
            _channel.BasicConsume(
                queue: _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"),
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        private async Task HandleMessage(CartDto cartDto)
        {
            // Thirrja e metodës për regjistrimin e email-it dhe logimin
            _emailService.EmailCartAndLog(cartDto).GetAwaiter().GetResult();
        }
    }
}
