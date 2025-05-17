using Shared;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace EventBusServer
{
    public class EventBusServer
    {
        private readonly int _port;
        private TcpListener? _listener;
        private readonly List<TcpClient> _clients = new();


        public EventBusServer(int port)
        {
            _port = port;
        }


        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            Console.WriteLine($"EventBus server started on port {_port}");

            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _clients.Add(client);
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            var endpoint = client.Client.RemoteEndPoint;
            Console.WriteLine($"New client connected: {endpoint}");

            using var stream = client.GetStream();
            var buffer = new byte[4096];

            try
            {
                while (true)
                {
                    var byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (byteCount == 0) break; 

                    var json = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    EventMessage? msg = null;
                    try
                    {
                        msg = JsonSerializer.Deserialize<EventMessage>(json);
                    }
                    catch
                    {
                        Console.WriteLine($"Invalid JSON from {endpoint}: {json.Trim()}");
                        continue;
                    }
                    Console.WriteLine($"[{msg?.Timestamp:HH:mm:ss}] {msg?.Sender ?? "unknown"} ({msg?.Type}): {msg?.Data}");

                    
                    var responseData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(msg));
                    foreach (var otherClient in _clients.Where(c => c != client && c.Connected))
                    {
                        try
                        {
                            await otherClient.GetStream().WriteAsync(responseData, 0, responseData.Length);
                        }
                        catch {  }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"Client disconnected: {endpoint}");
                client.Close();
                _clients.Remove(client);
            }
        }

    }
}
