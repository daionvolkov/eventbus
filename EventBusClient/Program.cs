using Shared;
using System.Net.Sockets;

namespace EventBusClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var clientName = Environment.GetEnvironmentVariable("CLIENT_NAME") ?? "client";
            var serverHost = Environment.GetEnvironmentVariable("SERVER_HOST") ?? "server";
            var serverPort = int.Parse(Environment.GetEnvironmentVariable("SERVER_PORT") ?? "9000");

            var client = new EventBusClient(serverHost, serverPort, clientName);
            await client.ConnectAsync(); 

            await SendMessageAsync(client);
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


