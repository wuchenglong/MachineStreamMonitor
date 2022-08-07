using MassTransit;
using MassTransit.RabbitMqTransport;

namespace DataService.Publisher
{
    public static class BusConfigurator
    {
        public static IBusControl ConfigureBus(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost>? registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq();
        }
    }
}
