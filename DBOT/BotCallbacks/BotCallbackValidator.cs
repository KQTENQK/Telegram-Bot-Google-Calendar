using System;

namespace DBOT.BotCallbacks
{
    public static class BotCallbackValidator
    {
        /// <summary>
        /// Defines callback type.
        /// </summary>
        public static ICallback Validate(string callbackData)
        {
            if (callbackData.StartsWith(new Undefined().KeyWord))
                return new Undefined();

            if (callbackData.StartsWith(new RequestDateInfo().KeyWord))
                return new RequestDateInfo();

            if (callbackData.StartsWith(new RequestNextMonthInfo().KeyWord))
                return new RequestNextMonthInfo();

            if (callbackData.StartsWith(new RequestPreviousMonthInfo().KeyWord))
                return new RequestPreviousMonthInfo();

            throw new Exception("Unknown callback.");
        }
    }
}
