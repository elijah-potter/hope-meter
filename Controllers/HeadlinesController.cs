using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Hope.Models;

namespace Hope.Controllers;

[ApiController]
[Route("[controller]")]
public class HeadlineController : ControllerBase
{
    private readonly ILogger<HeadlineController> _logger;
    private readonly DataContext _dataContext;

    public HeadlineController(ILogger<HeadlineController> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;
    }

    [HttpGet(Name = "GetHeadlines")]
    public async Task<IEnumerable<Headline>> Get()
    {
        return await _dataContext.Headlines.ToListAsync();
    }
}
