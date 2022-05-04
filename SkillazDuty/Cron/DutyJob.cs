using DutyBot.Services;
using Quartz;

namespace DutyBot.Cron;
public class DutyJob : IJob
{
    private readonly SenderService _senderService;
    private readonly UserService _userService;
    private readonly DayOffService _dayOffService;
    private readonly IConfiguration _configuration;


    public DutyJob(
        SenderService senderService,
        DayOffService dayOffService,
            UserService userService,
            IConfiguration configuration)
    {
        _configuration = configuration;
        _userService = userService;
        _dayOffService = dayOffService;
        _senderService = senderService;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        if (await _dayOffService.IsWorking(DateTime.UtcNow) || _configuration["Always"] == "1")
        {
            var user = await _userService.SetNext();
            await _senderService.Send($"Сегодня дежурный {user.Name}. {user.Telegram}");
        }
    }
}
