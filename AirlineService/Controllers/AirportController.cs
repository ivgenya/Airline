using AirlineService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AirportController : Controller
{
    private readonly AirlineDbContext _dbContext;

    public AirportController(AirlineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("getAllAirports")]
    public async Task<IActionResult> GetAllAirports()
    {
        var airports = await _dbContext.Airports.ToListAsync();
        return Ok(airports);
    }
}