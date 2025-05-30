using System.Text;

namespace EventBusClient;

public class EventListener
{
    private readonly EventBusClient _client;

    public EventListener(EventBusClient client)
    {
        _client = client;
    }
    
    public async Task ListenAsync()
    {
        var stream = _client.GetStream();
        var buffer = new byte[4096];
        while (_client.IsConnected)
        {
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead == 0) break;
            var json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            // Обработка сообщения
            Console.WriteLine($"[Listener]: {json}");
        }
    }
}