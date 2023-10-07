using AirlineService.Data;
using AirlineService.Models;
using AirlineService.Services;
using Microsoft.EntityFrameworkCore;

namespace AirlineService.Repositories;

public class ScheduleRepository: IScheduleRepository
{
    private readonly AirlineDbContext _context;

    public ScheduleRepository(AirlineDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Schedule>> GetAllScheduleAsync()
    {
        return await _context.Schedules.ToListAsync();
    }

    public async Task<Schedule> GetScheduleByIdAsync(int scheduleId)
    {
        return await _context.Schedules.FindAsync(scheduleId);
    }

    public async Task AddScheduleAsync(Schedule schedule)
    {
        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateScheduleAsync(Schedule schedule)
    {
        _context.Entry(schedule).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteScheduleAsync(int scheduleId)
    {
        var schedule = await _context.Schedules.FindAsync(scheduleId);
        if (schedule != null)
        {
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }
    }
}