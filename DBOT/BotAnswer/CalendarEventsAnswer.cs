using Google.Apis.Calendar.v3.Data;
using System;

namespace DBOT.BotAnswer
{
    /// <summary>
    /// Used to represent message about calendar events.
    /// </summary>
    public class CalendarEventsAnswer : IBotAnswer
    {
        private readonly string _message;

        public string Message => _message;

        public CalendarEventsAnswer(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Creates message about calendar events.
        /// </summary>
        public static string FormUpReport(Events events, string title)
        {
            string report = title;

            if (events.Items == null || events.Items.Count <= 0)
                return string.Empty;

            for (int i = 0; i < events.Items.Count; i++)
            {
                Event eventItem = events.Items[i];
                string date = eventItem.Start.DateTime.Value.Date.ToString();
                string time = eventItem.Start.DateTime.Value.TimeOfDay.ToString();

                if (String.IsNullOrEmpty(date))
                {
                    date = eventItem.Start.Date;
                }

                if (String.IsNullOrEmpty(eventItem.Summary))
                    report += "<Без названия>";

                string location = string.Empty;
                if (!String.IsNullOrEmpty(eventItem.Location))
                    location = "\nМесто:  " + eventItem.Location;

                report += "\n\r" + eventItem.Summary + "\nДата:  " + date + "\nВремя:  " + time + location + "\n";
            }

            return report;
        }
    }
}