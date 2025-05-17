
namespace EventBusServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting EventBus server...");
            await StartServerAsync();
        }
        private static async Task StartServerAsync()
        {
            var port = 9000;
            var server = new EventBusServer(port);

            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) => {
                e.Cancel = true;
                cts.Cancel();
            };

            await server.StartAsync(cts.Token);

        }
    }
}
