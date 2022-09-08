using Google.Apis.Calendar.v3;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DBOT.BotCallbacks
{
    public interface ICallback
    {
        /// <summary>
        /// Keyword used to define callback type.
        /// </summary>
        string KeyWord { get; }

        /// <summary>
        /// Handles request.
        /// </summary>
        Task Handle(ITelegramBotClient botClient, CalendarService calendarService, CallbackQuery callbackQuery, BotCallbackInfo botCallbackInfo);
    }
}
