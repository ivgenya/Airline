﻿using AirlineService.Models;

namespace AirlineService.Services;

public interface IScheduleService
{
    Task<IEnumerable<Schedule>> GetAllScheduleAsync();
    Task<Schedule> GetScheduleByIdAsync(int scheduleId);
    Task AddScheduleAsync(Schedule schedule);
    Task UpdateScheduleAsync(Schedule schedule);
    Task DeleteScheduleAsync(int scheduleId);
}