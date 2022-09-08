using Google.Apis.Calendar.v3;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DBOT.BotCallbacks
{
    public class Undefined : ICallback
    {
        private readonly string _keyWord = "undefined:";

        /// <summary>
        /// Keyword used to define callback type.
        /// </summary>
        public string KeyWord => _keyWord;

        /// <summary>
        /// Does nothing.
        /// </summary>
        public async Task Handle(ITelegramBotClient botClient, CalendarService calendarService, CallbackQuery callbackQuery, BotCallbackInfo botCallbackInfo) { return; }
    }
}
