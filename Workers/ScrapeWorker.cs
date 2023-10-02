using Microsoft.EntityFrameworkCore;

namespace Hope.Workers;

public class ScrapeWorker : BackgroundService
{
    private readonly ILogger<ScrapeWorker> _logger;
    private readonly DataContext _dataContext;

    public ScrapeWorker(ILogger<ScrapeWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _dataContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("ScrapeWorker running at: {time}", DateTimeOffset.Now);

            var fetched = await Scrape.ScrapeDefaultSources();

            _logger.LogInformation("Scraped {0} headlines.", fetched.Count);

            foreach (var headline in fetched)
            {
                _logger.LogInformation("{0} matching entries found", await _dataContext.Headlines.Where(h => h.Content == headline.Content).CountAsync());

                if (await _dataContext.Headlines.Where(h => h.Content == headline.Content).CountAsync() == 0)
                {
                    await _dataContext.Headlines.AddAsync(headline);
                    _logger.LogInformation("Added headline \"{0}\" to the database.", headline.Content);
                }
            }

            await _dataContext.SaveChangesAsync();

            _logger.LogInformation("ScrapeWorker completed at: {time}", DateTimeOffset.Now);

            await Task.Delay(120000, stoppingToken);
        }
    }
}
