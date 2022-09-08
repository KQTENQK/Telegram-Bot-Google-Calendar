using Google.Apis.Calendar.v3;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DBOT.BotCommands
{
    public interface IBotCommand
    {
        /// <summary>
        /// Used to define command type.
        /// </summary>
        string KeyWord { get; }

        Task Handle(ITelegramBotClient botClient, Message message, CalendarService calendarService);
    }
}
