using Shared;
using System.Net.Sockets;

namespace EventBusClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var serverHost = Environment.GetEnvironmentVariable("SERVER_HOST") ?? "localhost";
            var serverPort = int.Parse(Environment.GetEnvironmentVariable("SERVER_PORT") ?? "9000");
            var clientName = Environment.GetEnvironmentVariable("CLIENT_NAME") ?? "Anonymous";
            var client = new EventBusClient(serverHost, serverPort, clientName);

            await client.ConnectAsync();
            
            for (int i = 0; i < 100; i++)
            {
                var message = new EventMessage
                {
                    Type = "chat",
                    Data = $"Hello from {clientName}! #{i + 1}",
                    Sender = clientName
                };
                await client.SendEventAsync(message);
                await Task.Delay(3000); 
            }
            await Task.Delay(-1); 
        }


        private static async Task SendMessageAsync(EventBusClient client)
        {
            Console.WriteLine("Enter messages (or leave blank to exit):");
            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    break;

                var message = new EventMessage
                {
                    Type = "chat",
                    Data = input,
                    Sender = client.Name
                };
                await client.SendEventAsync(message);
            }
        }
    }
}


