namespace Shared
{
    public class EventMessage
    {
        public string Type { get; set; } = "event";   
        public string Data { get; set; } = "";        
        public string? Sender { get; set; }           
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
