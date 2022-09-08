using DBOT.BotCallbacks;
using Google.Apis.Calendar.v3;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DBOT.BotCommands
{
    /// <summary>
    /// Represents "/cal" command.
    /// </summary>
    public class BotCal : IBotCommand
    {
        private readonly string _keyWord = "/cal";

        /// <summary>
        /// Used to define command type.
        /// </summary>
        public string KeyWord => _keyWord;

        /// <summary>
        /// Sends keyboard calendar for current month.
        /// </summary>
        public async Task Handle(ITelegramBotClient botClient, Message message, CalendarService calendarService)
        {
            string title = "Выберите число:";
            Message editing = await botClient.SendTextMessageAsync(message.Chat.Id, title, replyMarkup: new InlineKeyboardMarkup(new InlineKeyboardButton[]
                                                                {
                                                                            InlineKeyboardButton.WithCallbackData(DateTime.Now.Month.ToString()
                                                                            + "." + DateTime.Now.Year.ToString()), new Undefined().KeyWord
                                                                }));

            InlineKeyboardMarkup keyboard = InlineCalendarFactory.GetKeyboard(DateTime.Now, editing.MessageId);

            await botClient.EditMessageReplyMarkupAsync(message.Chat.Id, editing.MessageId, keyboard);
        }
    }
}
