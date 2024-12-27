﻿namespace ShopLite.Services.OrderAPI.RabbitMQSender
{
    public interface IRabbitMQOrderMessageSender
    {
        void SendMessage(object message, string exchangeName);
    }
}
