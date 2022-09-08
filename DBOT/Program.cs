using DBOT.BotAnswer;
using DBOT.BotCallbacks;
using DBOT.BotCommands;
using DBOT.DatabaseContexts;
using DBOT.GoogleServices;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DBOT
{
    public class Program
    {
        private static readonly string _applicationName = ConfigurationManager.AppSettings.Get("ApplicationName");
        private static readonly string _connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        private static readonly int _millisUpdatingCalendarDelay = int.Parse(ConfigurationManager.AppSettings.Get("MillisUpdatingCalendarDelay"));
        private static readonly int _millisNotifyingUserDelay = int.Parse(ConfigurationManager.AppSettings.Get("MillisNotifyingUserDelay"));
        private static readonly int _minutesFromEvent = int.Parse(ConfigurationManager.AppSettings.Get("MinutesFromEvent"));
        private static readonly string _credentialPath = ConfigurationManager.AppSettings.Get("CredentialPath");
        private static readonly string _userDataPath = ConfigurationManager.AppSettings.Get("UserDataPath");
        private static readonly TelegramBotClient _bot = new TelegramBotClient(ConfigurationManager.AppSettings.Get("TelegramBotToken"));
        private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static readonly Events _notifiedEvents = new Events();
        private static UserCredential _userCredential;
        private static CalendarService _calendarService;

        public static readonly string[] Scopes = { CalendarService.Scope.CalendarReadonly };

        private static void Main(string[] args)
        {
            GoogleAPI.Auth(_credentialPath, _userDataPath, Scopes, out _userCredential);

            ReceiverOptions receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.CallbackQuery
                }
            };

            _bot.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync, receiverOptions, _cancellationTokenSource.Token);

            _notifiedEvents.Items = new List<Event>();

            Task.Run(() => BeginUpdatingCalendar(_cancellationTokenSource.Token, _millisUpdatingCalendarDelay));
            Task.Run(() => BeginNotifyingUsers(_cancellationTokenSource.Token, _minutesFromEvent, _millisNotifyingUserDelay));

            Console.ReadLine();
        }

        private static async Task BeginUpdatingCalendar(CancellationToken cancellationToken, int millisDelay)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _calendarService = GoogleCalendar.UpdateLocaly(_userCredential, _applicationName);
                await Task.Delay(millisDelay);
            }
        }

        private static async Task BeginNotifyingUsers(CancellationToken cancellationToken, int minutesFromEvent, int millisDelay)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                DateTime target = DateTime.Now;
                target = target.AddMinutes(minutesFromEvent);
                Events events = GoogleCalendar.GetEvents(_calendarService, DateTime.Now, target);

                Events eventsToNotify = new Events
                {
                    Items = new List<Event>()
                };

                eventsToNotify.Items = events.Items.Where(onGoingEvent => _notifiedEvents.Items.Any(notifiedEvent => notifiedEvent.Id != onGoingEvent.Id)).ToList();

                if (_notifiedEvents.Items.Count == 0)
                    eventsToNotify.Items = events.Items;

                eventsToNotify.Items = eventsToNotify.Items.Where(p => p.Start.DateTime > DateTime.Now).ToList();

                string title = "Предстоящее событие:\n";
                string report = CalendarEventsAnswer.FormUpReport(eventsToNotify, title);

                if (eventsToNotify.Items.Count > 0)
                {
                    List<NotifyingInterlocutor> interlocuters;

                    using (NotifyingInterlocutorContext interlocutorContext = new NotifyingInterlocutorContext(_connectionString))
                    {
                        interlocuters = interlocutorContext.Interlocutors.Where(p => p.IsNotifying == true).ToList();
                    }

                    foreach (var interlocuter in interlocuters)
                    {
                        await _bot.SendTextMessageAsync(interlocuter.UId, report);
                    }

                    foreach (Event eventToNotify in eventsToNotify.Items)
                    {
                        _notifiedEvents.Items.Add(eventToNotify);
                    }
                }

                Events toRemove = new Events
                {
                    Items = new List<Event>()
                };

                foreach (Event notifiedEvent in _notifiedEvents.Items)
                {
                    if (notifiedEvent.Start.DateTime < DateTime.Now)
                        toRemove.Items.Add(notifiedEvent);
                }

                foreach (Event removingEvent in toRemove.Items)
                {
                    _notifiedEvents.Items.Remove(removingEvent);
                }

                await Task.Delay(millisDelay);
            }
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update == null)
                return;

            if (update.Type == UpdateType.Message)
            {
                Message message = update.Message;

                await HandleCommand(botClient, message);

                return;
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                CallbackQuery callbackQuery = update.CallbackQuery;

                await HandleCallbackQuery(botClient, callbackQuery);

                return;
            }

        }

        private static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Exception:{exception.Message}");

            return Task.CompletedTask;
        }

        private static async Task HandleCommand(ITelegramBotClient botClient, Message message)
        {
            if (String.IsNullOrEmpty(message.Text))
                return;

            IBotCommand botCommand = CommandValidator.Validate(message.Text);

            await botCommand.Handle(botClient, message, _calendarService);
        }

        private static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data == null)
                return;

            ICallback callback = BotCallbackValidator.Validate(callbackQuery.Data);

            if (callback is Undefined)
                return;

            BotCallbackInfo botCallbackInfo = BotCallbackParser.Parse(callbackQuery.Data);
            
            await callback.Handle(botClient, _calendarService, callbackQuery, botCallbackInfo);
        }
    }
}
