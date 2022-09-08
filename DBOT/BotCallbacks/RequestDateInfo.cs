using DBOT.BotAnswer;
using DBOT.GoogleServices;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DBOT.BotCallbacks
{
    public class RequestDateInfo : ICallback
    {
        private readonly string _keyWord = "d:";

        /// <summary>
        /// Keyword used to define callback type.
        /// </summary>
        public string KeyWord => _keyWord;

        /// <summary>
        /// Sends info about target date.
        /// </summary>
        public async Task Handle(ITelegramBotClient botClient, CalendarService calendarService, CallbackQuery callbackQuery, BotCallbackInfo botCallbackInfo)
        {
            DateTime timeMin = new DateTime(botCallbackInfo.DateTimeInfo.Year, botCallbackInfo.DateTimeInfo.Month, botCallbackInfo.DateTimeInfo.Day, 0, 0, 0);
            DateTime timeMax = new DateTime(botCallbackInfo.DateTimeInfo.Year, botCallbackInfo.DateTimeInfo.Month, botCallbackInfo.DateTimeInfo.Day, 23, 59, 59);
            Events events = GoogleCalendar.GetEvents(calendarService, timeMin, timeMax);

            string title = $"События на {botCallbackInfo.DateTimeInfo.Day}.{botCallbackInfo.DateTimeInfo.Month}.{botCallbackInfo.DateTimeInfo.Year}\n\n";
            string messageText = CalendarEventsAnswer.FormUpReport(events, title);

            if (String.IsNullOrEmpty(messageText))
                messageText = "На выбранный день нет запланированных событий.";

            await botClient.EditMessageTextAsync(callbackQuery.Message.Chat.Id, botCallbackInfo.EditingMessageId, messageText);
        }
    }
}
