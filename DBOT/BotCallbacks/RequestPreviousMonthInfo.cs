using Google.Apis.Calendar.v3;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DBOT.BotCallbacks
{
    public class RequestPreviousMonthInfo : ICallback
    {
        private readonly string _keyWord = "prev:";

        /// <summary>
        /// Keyword used to define callback type.
        /// </summary>
        public string KeyWord => _keyWord;

        /// <summary>
        /// Sends keyboard calendar for previous month.
        /// </summary>
        public async Task Handle(ITelegramBotClient botClient, CalendarService calendarService, CallbackQuery callbackQuery, BotCallbackInfo botCallbackInfo)
        {
            InlineKeyboardMarkup keyboard = InlineCalendarFactory.GetKeyboard(botCallbackInfo.DateTimeInfo.AddMonths(-1), botCallbackInfo.EditingMessageId);

            await botClient.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, botCallbackInfo.EditingMessageId, keyboard);
        }
    }
}
