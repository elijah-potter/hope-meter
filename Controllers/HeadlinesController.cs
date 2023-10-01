using Hope.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hope.Controllers;

[ApiController]
[Route("[controller]")]
public class HeadlineController : ControllerBase
{
    private readonly ILogger<HeadlineController> _logger;

    public HeadlineController(ILogger<HeadlineController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetHeadlines")]
    public IEnumerable<Headline> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Headline { })
        .ToArray();
    }
}
