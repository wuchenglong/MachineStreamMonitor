using DataService.Consumers;
using DataService.Models;
using DataService.Services;
using MassTransit;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // ignore omitted parameters on models to enable optional params (e.g. User update)
#pragma warning disable SYSLIB0020 // Type or member is obsolete
    x.JsonSerializerOptions.IgnoreNullValues = true;
#pragma warning restore SYSLIB0020 // Type or member is obsolete
}); 


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

builder.Services.AddMassTransit((IBusRegistrationConfigurator x) =>
{
    x.AddConsumer<StoreData>();

    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(rabbitMq =>
    {
        rabbitMq.Host(new Uri(configuration["MQSettings:RabbitMqRootUri"]), h =>
        {
            h.Username(configuration["MQSettings:UserName"]);
            h.Password(configuration["MQSettings:Password"]);
        });
        rabbitMq.ReceiveEndpoint(configuration["MQSettings:RabbitMqQueueName"], ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<StoreData>(provider);
        });
    }));
});


builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("MachineStreamDatabase"));

builder.Services.AddSingleton<StoreService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
