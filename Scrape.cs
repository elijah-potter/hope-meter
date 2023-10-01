using Hope.Models;
using System.Threading.Channels;

namespace Hope;

public class Scrape
{
    static private string[] DefaultSources = {
      "http://feeds.foxnews.com/foxnews/scitech",
      "http://rss.cnn.com/rss/cnn_topstories.rss",
      "http://feeds.feedburner.com/time/topstories",
      "http://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml",
      "https://www.yahoo.com/news/rss/topstories",
      "http://feeds.washingtonpost.com/rss/world",
      "http://feeds.bbci.co.uk/news/world/rss.xml",
      "https://www.huffingtonpost.com/section/front-page/feed",
      "http://rss.cnn.com/rss/cnn_world.rss",
      "http://abcnews.go.com/abcnews/topstories",
      "http://www.marketwatch.com/rss/topstories/",
      "https://www.salon.com/feed/",
      "http://www.newyorker.com/services/rss/feeds/everything.xml",
      "https://nypost.com/feed/",
  };

    private Channel<Uri> Queue;
    private Channel<string> Results;
    private Task? ScrapeTask;

    public Scrape()
    {
        Queue = Channel.CreateUnbounded<Uri>();
        Results = Channel.CreateUnbounded<string>();
    }

    public async void ScrapeDefaultSources(){
      foreach (var source in DefaultSources){
        await Queue.Writer.WriteAsync(new Uri(source));
      }
    }

    /// <summary>
    /// Starts a scrape task in the background if there isn't one already running.
    //// </summary>
    public void StartScrape(){
      if (ScrapeTask == null)
        ScrapeTask = Run(Queue.Reader);
    }

    private static async Task Run(ChannelReader<Uri> reader)
    {
        while (await reader.WaitToReadAsync())
        {
            while (reader.TryRead(out Uri source))
            {
                Console.WriteLine(source);
            }
        }
    }

    private static async ValueTask<Headline> FetchRSS(Uri source){
      var client = new HttpClient();

      var res = await client.GetAsync(source);
      res.EnsureSuccessStatusCode();
      
      return new Headline{};
    }
}
