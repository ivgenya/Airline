using AirlineService.DTO;
using AirlineService.Models;
using AirlineService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirlineService.Controllers;


[Route("api/schedule")]
[ApiController]
public class ScheduleController: ControllerBase
{
    private readonly IScheduleService _service;

    public ScheduleController(IScheduleService scheduleService)
    {
        _service = scheduleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetAllSchedules()
    {
        var schedules = await _service.GetAllScheduleAsync();
        return Ok(schedules);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Schedule>> GetScheduleById(int id)
    {
        var schedule = await _service.GetScheduleByIdAsync(id);
        if (schedule == null)
        {
            return NotFound();
        }
        return Ok(schedule);
    }

    [HttpPost]
    public async Task<ActionResult<Schedule>> CreateSchedule([FromBody] ScheduleModel scheduleModel)
    {
        Schedule schedule = new Schedule
        {
            AirlineId = scheduleModel.AirlineId,
            Number = scheduleModel.Number,
            DepartureAirportId = scheduleModel.DepartureAirportId,
            ArrivalAirportId = scheduleModel.ArrivalAirportId,
            DepartureTime = scheduleModel.DepartureTime,
            ArrivalTime = scheduleModel.ArrivalTime,
            FlightDuration = scheduleModel.FlightDuration,
            Terminal = scheduleModel.Terminal
        };
        await _service.AddScheduleAsync(schedule);
        return CreatedAtAction("GetScheduleById", new { id = schedule.Id }, schedule);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(int id, [FromBody] ScheduleModel scheduleModel)
    {
        var existingSchedule = await _service.GetScheduleByIdAsync(id);
        if (existingSchedule == null) 
            return NotFound();
        existingSchedule.AirlineId = scheduleModel.AirlineId;
        existingSchedule.Number = scheduleModel.Number;
        existingSchedule.DepartureAirportId = scheduleModel.DepartureAirportId;
        existingSchedule.ArrivalAirportId = scheduleModel.ArrivalAirportId;
        existingSchedule.DepartureTime = scheduleModel.DepartureTime;
        existingSchedule.ArrivalTime = scheduleModel.ArrivalTime;
        existingSchedule.FlightDuration = scheduleModel.FlightDuration;
        existingSchedule.Terminal = scheduleModel.Terminal;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        var existingSchedule = await _service.GetScheduleByIdAsync(id);
        if (existingSchedule == null)
        {
            return NotFound();
        }
        await _service.DeleteScheduleAsync(id);
        return NoContent();
    }
}
