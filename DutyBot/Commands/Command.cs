using System;


using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace DutyBot.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        private readonly IConfiguration _configuration;

        public Command(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public abstract Task Execute(Message message);

        public bool Contains(string command)
        {
            var name = _configuration["Name"];
            return command.Contains(Name) && command.Contains(name);
        }
    }
}