using DBOT.BotAnswer;
using DBOT.DatabaseContexts;
using DBOT.GoogleServices;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DBOT.BotCommands
{
    /// <summary>
    /// Represents "/start" command.
    /// </summary>
    public class BotStart : IBotCommand
    {
        private readonly int _minutesFromEvent = int.Parse(ConfigurationManager.AppSettings.Get("MinutesFromEvent"));
        private readonly string _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        private readonly string _keyWord = "/start";

        /// <summary>
        /// Used to define command type.
        /// </summary>
        public string KeyWord => _keyWord;

        /// <summary>
        /// Registers user in database and informs about ongoing events.
        /// </summary>
        public async Task Handle(ITelegramBotClient botClient, Message message, CalendarService calendarService)
        {
            await Task.Run(() =>
            {
                using (NotifyingInterlocutorContext interlocutorContext = new NotifyingInterlocutorContext(_connectionString))
                {
                    NotifyingInterlocutor target = interlocutorContext.Interlocutors.Where(p => p.UId == message.Chat.Id).FirstOrDefault();
                    if (target == null)
                    {
                        target = new NotifyingInterlocutor
                        {
                            UId = message.Chat.Id,
                            IsNotifying = true
                        };

                        interlocutorContext.Interlocutors.Add(target);
                        interlocutorContext.SaveChanges();
                    }
                }
            });

            await botClient.SendTextMessageAsync(message.Chat.Id, new HelpAnswer().Message);

            DateTime targetOnGoing = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 59);
            targetOnGoing = targetOnGoing.AddMinutes(_minutesFromEvent);
            Events onGoing = GoogleCalendar.GetEvents(calendarService, DateTime.Now, targetOnGoing);
            string report = CalendarEventsAnswer.FormUpReport(onGoing, "Предстоящие события:\n");

            if (String.IsNullOrEmpty(report))
                return;

            await botClient.SendTextMessageAsync(message.Chat.Id, report);
        }
    }
}
