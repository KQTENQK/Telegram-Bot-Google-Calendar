namespace DBOT.BotAnswer
{
    /// <summary>
    /// Represents help message.
    /// </summary>
    public class HelpAnswer : IBotAnswer
    {
        private readonly string _message = "Список команд:\n\n" +
            "/events - просмотреть события на день\n\n" +
            "/notify - включить или выключить уведомления\n\n" +
            "/cal - выбрать дату\n\n" +
            "/help - просмотреть доступные команды";

        public string Message => _message;
    }
}