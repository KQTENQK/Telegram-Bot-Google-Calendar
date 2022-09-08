using System;

namespace DBOT.BotCallbacks
{
    public static class BotCallbackParser
    {
        /// <summary>
        /// Parses callback string to <c>BotCallbackInfo</c>.
        /// *viewing date must be 8 numbers long in order to work correctly*
        /// </summary>
        public static BotCallbackInfo Parse(string callbackQueryData)
        {
            int index = callbackQueryData.IndexOf(':') + 1;
            int year = int.Parse(callbackQueryData.Substring(index, 4));
            index += 4;
            int month = int.Parse(callbackQueryData.Substring(index, 2));
            index += 2;
            int day = int.Parse(callbackQueryData.Substring(index, 2));

            string parsing = callbackQueryData.Substring(index);
            index = parsing.IndexOf(':') + 1;
            int editingMessageId = int.Parse(parsing.Substring(index));

            DateTime about = new DateTime(year, month, day);

            return new BotCallbackInfo(about, editingMessageId);
        }
    }
}
