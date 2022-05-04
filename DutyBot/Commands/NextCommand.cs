using DutyBot.Services;
using Telegram.Bot.Types;

namespace DutyBot.Commands;

public class NextCommand : Command
{
    private readonly UserService _userService;
    private readonly SenderService _senderService;

    public NextCommand(
        IConfiguration configuration,
        SenderService senderService,
        UserService userService) : base(configuration)
    {
        _senderService = senderService;
        _userService = userService;
    }

    public override string Name => "next";

    public override async Task Execute(Message message)
    {
        var next = await _userService.SetNext();

        await _senderService.Send($"Сегодня дежурный {next.ToString()}");
    }
}