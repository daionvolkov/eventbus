using Shared;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace EventBusClient
{
    public class EventBusClient
    {
        private readonly string _host;
        private readonly int _port;
        private TcpClient? _client;
        private NetworkStream? _stream;
        public string? Name { get; set; }

        public EventBusClient(string host, int port, string? name)
        {
            _host = host;
            _port = port;
            Name = name;
        }


        public async Task ConnectAsync()
        {
            _client = new TcpClient();
            await _client.ConnectAsync(_host, _port);
            _stream = _client.GetStream();
            Console.WriteLine($"Connected to EventBus at {_host}:{_port}");


            _ = ListenAsync();
        }

        public async Task SendAsync(string message)
        {
            if (_stream == null) return;
            var data = Encoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(data, 0, data.Length);
        }


        public async Task SendEventAsync(EventMessage message)
        {
            if (_stream == null) return;
            var json = JsonSerializer.Serialize(message);
            var data = Encoding.UTF8.GetBytes(json);
            await _stream.WriteAsync(data, 0, data.Length);
        }



        private async Task ListenAsync()
        {
            var buffer = new byte[4096];
            try
            {
                while (_client is { Connected: true })
                {
                    var bytesRead = await _stream!.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Сервер закрыл соединение
                    var json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    try
                    {
                        var eventMsg = JsonSerializer.Deserialize<EventMessage>(json);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"\n[{eventMsg?.Timestamp:HH:mm:ss}] {eventMsg?.Sender ?? "Server"} ({eventMsg?.Type}): {eventMsg?.Data}");
                        Console.ResetColor();
                    }
                    catch
                    {
                        Console.WriteLine($"\n[From Server][RAW]: {json.Trim()}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection closed. Reason: {ex.Message}");
            }
            finally
            {
                _client?.Close();
            }
        }
    }
}
