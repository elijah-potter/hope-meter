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
    [ResponseCache(Duration = 60)]
    public async Task<double> Get()
    {
        return await HopeCalculator.CalculateHope(_dataContext);
    }
}
