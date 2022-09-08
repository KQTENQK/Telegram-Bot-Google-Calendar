using DBOT.BotAnswer;
using Google.Apis.Calendar.v3;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DBOT.BotCommands
{
    /// <summary>
    /// Represents "/help" command.
    /// </summary>
    public class BotHelp : IBotCommand
    {
        private readonly string _keyWord = "/help";

        /// <summary>
        /// Used to define command type.
        /// </summary>
        public string KeyWord => _keyWord;

        /// <summary>
        /// Sends help info.
        /// </summary>
        public async Task Handle(ITelegramBotClient botClient, Message message, CalendarService calendarService)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, new HelpAnswer().Message);
        }
    }
}
