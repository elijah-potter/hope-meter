namespace Hope.Models;

public class Headline
{
    public ulong? Id { get; set; }
    public Uri Source { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }

    public Headline(Uri source, string content, DateTime timestamp)
    {
        Source = source;
        Content = content;
        Timestamp = timestamp;
    }
}
