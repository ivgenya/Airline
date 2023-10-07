using AirlineService.DTO;
using AirlineService.Models;
using AirlineService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirlineService.Controllers;

[Route("api/flights")]
[ApiController]
public class FlightController : ControllerBase
{
    private readonly IFlightService _service;

    public FlightController(IFlightService flightService)
    {
        _service = flightService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlightById(int id)
    {
        var flight = await _service.GetFlightByIdAsync(id);
        if (flight == null)
            return NotFound();

        return Ok(flight);
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllFlights()
    {
        var flights = await _service.GetAllFlightsAsync();
        return Ok(flights);
    }

    [HttpGet]
    public async Task<IActionResult> GetFlights([FromQuery] DateTime date, [FromQuery] string departureLocation, [FromQuery] string arrivalLocation)
    {
        var flights = await _service.GetFlightsByDateAndLocationsAsync(date, departureLocation, arrivalLocation);
        return Ok(flights);
    }

    [HttpPost]
    public async Task<IActionResult> AddFlight([FromBody] FlightModel flightModel)
    {
        var flight = new Flight
        {
            ScheduleId = flightModel.ScheduleId,
            Date = flightModel.Date,
            PlaneId = flightModel.PlaneId,
            Type = flightModel.Type,
            Status = flightModel.Status,
            Gate = flightModel.Gate
        };
        await _service.AddFlightAsync(flight);
        return CreatedAtAction(nameof(GetFlightById), new { id = flight.Id }, flight);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFlight(int id, [FromBody] FlightModel flightModel)
    {
        var existingFlight = await _service.GetFlightByIdAsync(id);
        if (existingFlight == null)
            return NotFound();
    
        // Обновляем свойства существующего объекта Flight значениями из FlightModel
        existingFlight.ScheduleId = flightModel.ScheduleId;
        existingFlight.Date = flightModel.Date;
        existingFlight.PlaneId = flightModel.PlaneId;
        existingFlight.Type = flightModel.Type;
        existingFlight.Status = flightModel.Status;
        existingFlight.Gate = flightModel.Gate;

        await _service.UpdateFlightAsync(existingFlight);
    
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFlight(int id)
    {
        var existingFlight = await _service.GetFlightByIdAsync(id);
        if (existingFlight == null)
            return NotFound();

        await _service.DeleteFlightAsync(id);
        return NoContent();
    }
}