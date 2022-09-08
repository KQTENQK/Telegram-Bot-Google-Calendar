using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;

namespace DBOT.GoogleServices
{
    /// <summary>
    /// Gets info from google calendar.
    /// </summary>
    public static class GoogleCalendar
    {
        /// <summary>
        /// Updates calendar service from google server.
        /// </summary>
        public static CalendarService UpdateLocaly(UserCredential userCredential, string applicationName)
        {
            CalendarService calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = userCredential,
                ApplicationName = applicationName
            });

            return calendarService;
        }

        /// <summary>
        /// Gets sorted events using current calendar service.
        /// </summary>
        public static Events GetEvents(CalendarService calendarService, DateTime dateTimeFrom, DateTime dateTimeTarget, int maxEventsAmount = 10)
        {
            EventsResource.ListRequest request = calendarService.Events.List("primary");
            request.TimeMin = dateTimeFrom;
            request.TimeMax = dateTimeTarget;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = maxEventsAmount;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            Events events = request.Execute();

            return events;
        }
    }
}
