using AirlineService.Services;
using Quartz;

namespace AirlineService.Scheduler;

public static class DependencyInjection
{
    public static void AddScheduler(this IServiceCollection services) 
    {
        services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();

            var jobKey = JobKey.Create(nameof(CheckTicketsAndBookingsState));
            options
                .AddJob<CheckTicketsAndBookingsState>(jobKey)
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(300).RepeatForever()));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }
    
}