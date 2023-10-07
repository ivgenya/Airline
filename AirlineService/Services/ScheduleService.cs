using AirlineService.Models;

namespace AirlineService.Services;

public class ScheduleService: IScheduleService
{
    private readonly IScheduleRepository _repository;

    public ScheduleService(IScheduleRepository scheduleRepository)
    {
        _repository = scheduleRepository;
    }

    public async Task<IEnumerable<Schedule>> GetAllScheduleAsync()
    {
        return await _repository.GetAllScheduleAsync();
    }

    public async Task<Schedule> GetScheduleByIdAsync(int scheduleId)
    {
        return await _repository.GetScheduleByIdAsync(scheduleId);
    }

    public async Task AddScheduleAsync(Schedule schedule)
    {
        await _repository.AddScheduleAsync(schedule);
    }

    public async Task UpdateScheduleAsync(Schedule schedule)
    {
        await _repository.UpdateScheduleAsync(schedule);
    }

    public async Task DeleteScheduleAsync(int scheduleId)
    {
        await _repository.DeleteScheduleAsync(scheduleId);
    }
}