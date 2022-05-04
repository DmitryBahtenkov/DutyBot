using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DutyBot
{
    public class Bot
    {
        private static ITelegramBotClient _telegramBotClient;

        public static async Task CreateClient(IConfiguration configuration)
        {
            if (_telegramBotClient is null)
            {
                var key = configuration["Key"];
                _telegramBotClient = new TelegramBotClient(key);
                var url = configuration["Url"];
                await _telegramBotClient.SetWebhookAsync(url);
            }
        }

        public ITelegramBotClient Client => _telegramBotClient ?? throw new ArgumentNullException(nameof(_telegramBotClient));
    }
}