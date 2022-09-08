using DBOT.DatabaseContexts;
using Google.Apis.Calendar.v3;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DBOT.BotCommands
{
    /// <summary>
    /// Represents "/notify" command.
    /// </summary>
    public class BotNotify : IBotCommand
    {
        private readonly string _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        private readonly string _keyWord = "/notify";
        private readonly string _notifyDisabled = "Уведомления выключены";
        private readonly string _notifyEnabled = "Уведомления включены";

        /// <summary>
        /// Used to define command type.
        /// </summary>
        public string KeyWord => _keyWord;

        /// <summary>
        /// Turns on/off notifications about ongoing events.
        /// The users chat id must be in a database in order to work correctly.
        /// </summary>
        public async Task Handle(ITelegramBotClient botClient, Message message, CalendarService calendarService)
        {
            await Task.Run(() =>
            {
                using (NotifyingInterlocutorContext interlocutorContext = new NotifyingInterlocutorContext(_connectionString))
                {
                    NotifyingInterlocutor target = interlocutorContext.Interlocutors.Where(p => p.UId == message.Chat.Id).FirstOrDefault();
                    if (target != null)
                    {
                        target.IsNotifying = !target.IsNotifying;
                        interlocutorContext.SaveChanges();
                    }

                    string answerMessage = _notifyDisabled;

                    if (target.IsNotifying)
                        answerMessage = _notifyEnabled;

                    botClient.SendTextMessageAsync(message.Chat.Id, answerMessage);
                }
            });
        }
    }
}
