using Shared;

namespace EventBusClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Write("Enter client name: ");
            var name = Console.ReadLine();

            var client = new EventBusClient("127.0.0.1", 9000, name);
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


