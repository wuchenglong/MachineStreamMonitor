using DataService.Models;
using DataService.Services;
using MassTransit;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;

namespace DataService.Consumers
{
    public class StoreData : IConsumer<MachineStream>
    {
        private readonly StoreService _storeService;
        private readonly TelemetryClient _telemetry;

        public StoreData(StoreService storeService, TelemetryClient telemetry)
        {
            _storeService = storeService;
            _telemetry = telemetry;
        }
        public async Task Consume(ConsumeContext<MachineStream> context)
        {
            var data = context.Message;
            _telemetry.TrackTrace($"Consume data from MQ then storing to MongoDB, id: {data.Id}.");

            var item = await _storeService.GetAsync(data.Id);
            if (item == null)
            {
                await _storeService.CreateAsync(data);
            }
            else
            {
                await _storeService.UpdateAsync(data.Id, data);
            }
        }
    }
}
