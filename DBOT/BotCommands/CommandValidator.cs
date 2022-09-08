namespace DBOT.BotCommands
{
    public static class CommandValidator
    {
        /// <summary>
        /// Used to define command.
        /// </summary>
        public static IBotCommand Validate(string commandText)
        {
            if (commandText.StartsWith(new BotStart().KeyWord))
                return new BotStart();

            if (commandText.StartsWith(new BotNotify().KeyWord))
                return new BotNotify();

            if (commandText.StartsWith(new BotEvents().KeyWord))
                return new BotEvents();

            if (commandText.StartsWith(new BotCal().KeyWord))
                return new BotCal();

            return new BotHelp();
        }
    }
}
