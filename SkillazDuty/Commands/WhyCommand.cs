using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutyBot.Services;
using Telegram.Bot.Types;

namespace DutyBot.Commands
{
    public class WhyCommand : Command
    {
        private readonly UserService _userService;
        private readonly SenderService _senderService;

        public WhyCommand(
            UserService userService,
            SenderService senderService,
            IConfiguration configuration) : base(configuration)
        {
            _senderService = senderService;
            _userService = userService;
        }

        public override string Name => "who";



        public override async Task Execute(Message message)
        {
            var user = await _userService.Current();

            await _senderService.Send($"Сегодня дежурный {user.Name}. {user.Telegram}");
        }
    }
}