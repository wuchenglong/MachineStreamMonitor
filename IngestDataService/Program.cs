using DataService.Publisher;
using MassTransit;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

builder.Services.AddMassTransit(x =>
{
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.Host(new Uri(configuration["MQSettings:RabbitMqRootUri"]), h =>
        {
            h.Username(configuration["MQSettings:UserName"]);
            h.Password(configuration["MQSettings:Password"]);
        });
    }));
});

#pragma warning disable CS0618 // Type or member is obsolete
builder.Services.AddMassTransitHostedService();
#pragma warning restore CS0618 // Type or member is obsolete

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(60)
};
       
app.UseWebSockets(webSocketOptions);

app.Run();
