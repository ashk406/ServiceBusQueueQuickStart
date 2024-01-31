using Azure.Messaging.ServiceBus;
using Azure.Identity;

ServiceBusClient client;
ServiceBusSender sender;
const int numOfMessages = 3;

var clinentOptions = new ServiceBusClientOptions
{
    TransportType = ServiceBusTransportType.AmqpWebSockets
};

client = new ServiceBusClient("Endpoint=sb://ashoknamespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=r06TAy7XaHW6T8pvIjUJCKdeyOt1juU69+ASbK381+Y=", clinentOptions);
sender = client.CreateSender("ashokqueue");

//create a Batch
using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
for (int i = 1; i <= numOfMessages; i++)
{
    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
    {
        throw new Exception($"This message {i} is too loarge to fit in the batch");
    }
}

try
{
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
}
catch (Exception)
{
    throw;
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}
Console.WriteLine("Press any key to end the application");
Console.ReadKey();