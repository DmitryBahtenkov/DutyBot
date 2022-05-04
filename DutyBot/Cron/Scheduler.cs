using Quartz;
using Quartz.Impl;

namespace DutyBot.Cron;

public class Scheduler
{
    public static async void Start(IServiceProvider serviceProvider)
    {
        var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        scheduler.JobFactory = serviceProvider.GetService<JobFactory>()!;
        await scheduler.Start();

        var jobDetail = JobBuilder.Create<DutyJob>().Build();
        var trigger = TriggerBuilder.Create()
            .WithIdentity("MailingTrigger", "default")
            .WithSchedule(CronScheduleBuilder
                .DailyAtHourAndMinute(7, 15)
                .InTimeZone(TimeZoneInfo.Utc)
                .WithMisfireHandlingInstructionIgnoreMisfires())
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}