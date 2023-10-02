using VaderSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Hope.Controllers;

[ApiController]
[Route("[controller]")]
public class HopeController : ControllerBase
{
    private readonly ILogger<HopeController> _logger;
    private readonly DataContext _dataContext;

    public HopeController(ILogger<HopeController> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;
    }

    [HttpGet(Name = "GetHope")]
    public async Task<double> Get()
    {
        var analyzer = new SentimentIntensityAnalyzer();
        var data = await _dataContext.Headlines.Where(h => h.Timestamp > DateTime.Now.AddDays(-1)).Select(h => analyzer.PolarityScores(h.Content)).ToListAsync();

        var sum = data.Select(score => score.Positive - score.Negative).Sum();

        return sum;
    }
}
