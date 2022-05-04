using DutyBot;
using Telegram.Bot;

namespace DutyBot.Services;

public class SenderService
{
    private readonly IConfiguration _configuration;
    private readonly Bot _bot;

    public SenderService(
        IConfiguration configuration,
        Bot bot)
    {
        _bot = bot;
        _configuration = configuration;
    }

    public async Task Send(string message)
    {
        var chatId = _configuration["ChatId"];
        await _bot.Client.SendTextMessageAsync(chatId, message);
    }
}
