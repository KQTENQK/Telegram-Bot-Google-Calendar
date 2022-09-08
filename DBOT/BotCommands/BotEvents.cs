using DBOT.BotAnswer;
using DBOT.GoogleServices;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DBOT.BotCommands
{
    /// <summary>
    /// Represents "/events" command.
    /// </summary>
    public class BotEvents : IBotCommand
    {
        private readonly string _keyWord = "/events";

        /// <summary>
        /// Used to define command type.
        /// </summary>
        public string KeyWord => _keyWord;

        /// <summary>
        /// Sends info about events for today.
        /// </summary>
        public async Task Handle(ITelegramBotClient botClient, Message message, CalendarService calendarService)
        {
            Events events = GoogleCalendar.GetEvents(calendarService, DateTime.Now, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59));
            string todayReport = CalendarEventsAnswer.FormUpReport(events, "События на сегодня:\n");

            if (string.IsNullOrEmpty(todayReport))
                todayReport = "На сегодня событий нет.";

            await botClient.SendTextMessageAsync(message.Chat.Id, new CalendarEventsAnswer(todayReport).Message);
        }
    }
}
