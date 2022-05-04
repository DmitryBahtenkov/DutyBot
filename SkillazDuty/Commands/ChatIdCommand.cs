using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutyBot.Services;
using Telegram.Bot.Types;

namespace DutyBot.Commands;

public class ChatIdCommand : Command
{
    private readonly SenderService _senderService;

    public ChatIdCommand(IConfiguration configuration, SenderService senderService) : base(configuration)
    {
        _senderService = senderService;
    }

    public override string Name => "chatid";

    public override async Task Execute(Message message)
    {
        await _senderService.Send(message.Chat.Id.ToString());
    }
}
