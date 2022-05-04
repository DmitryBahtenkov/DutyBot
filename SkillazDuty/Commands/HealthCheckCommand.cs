using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutyBot.Services;
using Telegram.Bot.Types;

namespace DutyBot.Commands
{
    public class HealthCheckCommand : Command
    {
        private readonly SenderService _senderService;

        public HealthCheckCommand(IConfiguration configuration, SenderService senderService) : base(configuration)
        {
            _senderService = senderService;
        }

        public override string Name => "healthcheck";

        public override async Task Execute(Message message)
        {
            await _senderService.Send("Ok!");
        }
    }
}