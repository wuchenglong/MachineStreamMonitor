namespace DataService.Publisher
{
    public class MQSettings
    {
        public const string RabbitMqRootUri = "rabbitmq://192.168.1.65"; //localhost
        public const string RabbitMqQueueName = "machineStream";
        public const string RabbitMqUri = $"{RabbitMqRootUri}/{RabbitMqQueueName}";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string NotificationServiceQueue = "notification.service";
    }
}
