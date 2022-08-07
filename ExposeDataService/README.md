# ExposeDataService

---

## This project use to consumer data from MQ and store data

```
  "MQSettings": {
    "RabbitMqRootUri": "rabbitmq://192.168.1.65",
    "RabbitMqQueueName": "machineStream",
    "RabbitMqUri": "rabbitmq://192.168.1.65/machineStream",
    "UserName": "guest",
    "Password": "guest",
    "NotificationServiceQueue": "notification.service"
  },
  "MachineStreamDatabase": {
    "ConnectionString": "mongodb://admin:123456@192.168.1.65:27017",
    "DatabaseName": "MachineStream",
    "StreamCollectionName": "Stream"
  }
```

