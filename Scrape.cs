using System.ServiceModel.Syndication;
using Hope.Models;
using System.Xml;

namespace Hope;

public static class Scrape
{
    static private string[] DefaultSources = {
      "http://feeds.foxnews.com/foxnews/scitech",
      "http://rss.cnn.com/rss/cnn_topstories.rss",
      "http://feeds.feedburner.com/time/topstories",
      "http://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml",
      "https://www.yahoo.com/news/rss/topstories",
      "http://feeds.washingtonpost.com/rss/world",
      "http://feeds.bbci.co.uk/news/world/rss.xml",
      "http://rss.cnn.com/rss/cnn_world.rss",
      "http://abcnews.go.com/abcnews/topstories",
      "https://www.salon.com/feed/",
      "http://www.newyorker.com/services/rss/feeds/everything.xml",
      "https://nypost.com/feed/",
  };

    public static Task<List<Headline>> ScrapeDefaultSources()
    {
        return FetchAllRSS(DefaultSources.Select(source => new Uri(source)));
    }

    private static async Task<List<Headline>> FetchAllRSS(IEnumerable<Uri> sources)
    {
        var allTasks = sources.Select(FailableFetchRSS);

        var allRes = new List<Headline>();

        foreach (var task in allTasks)
        {
            var headlines = await task;

            foreach (var headline in headlines)
            {
                allRes.Add(headline);
            }
        }

        return allRes;
    }

    private static async Task<IEnumerable<Headline>> FailableFetchRSS(Uri source)
    {
        try
        {
            return await FetchRSS(source);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return new Headline[0];
        }
    }

    private static Task<IEnumerable<Headline>> FetchRSS(Uri source)
    {
        var client = new HttpClient();
        return FetchRSS(client, source);
    }

    private static async Task<IEnumerable<Headline>> FetchRSS(HttpClient client, Uri source)
    {
        var readerSettings = new XmlReaderSettings();
        readerSettings.Async = true;
        var reader = XmlReader.Create(source.ToString(), readerSettings);
        await reader.ReadAsync();
        var feed = SyndicationFeed.Load(reader);

        return feed.Items.Select(item => new Headline(source, item.Title.Text, DateTime.UtcNow));
    }
}
