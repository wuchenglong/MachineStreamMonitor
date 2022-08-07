using System.Net.WebSockets;
using System.Text;
using DataService.Models;
using DataService.Publisher;
using MassTransit;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DataService.Controllers
{
    public class WebSocketController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;
        private readonly TelemetryClient _telemetry;
        public WebSocketController(IBus bus, IConfiguration configuration, TelemetryClient telemetry)
        {
            _bus = bus;
            _configuration = configuration;
            _telemetry = telemetry;
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            Console.WriteLine("Connected!");
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await IngestData(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task IngestData(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
            
            while (!receiveResult.CloseStatus.HasValue)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, receiveResult.Count);
                // Console.WriteLine(message);
                try
                {
                    var jsonData = JsonConvert.DeserializeObject<dynamic>(message);                    
                    if (jsonData != null)
                    {
                        MachineStream machineStream = new MachineStream()
                        {
                            Id = (jsonData.payload.id != null) ? jsonData.payload.id : new Guid(),
                            EventName = jsonData["event"],
                            MachineId = jsonData.payload.machine_id,
                            Status = jsonData.payload.status,
                            Timestamp = jsonData.payload.timestamp,
                        };
                        if (machineStream.Status == MachineStreamStatus.errored)
                        {
                            // todo while find the errored machine.
                        }
                        Uri uri = new Uri(_configuration["MQSettings:RabbitMqUri"]);
                        var endPoint = await _bus.GetSendEndpoint(uri);
                        await endPoint.Send(machineStream);                        
                    }
                }
                catch (Exception ex)
                {
                    _telemetry.TrackException(ex);
                    //Console.WriteLine(ex.ToString());
                }

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}
