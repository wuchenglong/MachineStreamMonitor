using DataService.Models;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace DataService.Consumers
{
    public static class BusConfigurator
    {
        public static IBusControl ConfigureBus(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost>? registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq();
        }
    }
}
