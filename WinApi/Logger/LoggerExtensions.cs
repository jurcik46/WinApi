using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using GalaSoft.MvvmLight.Messaging;
using Serilog;
using Serilog.Events;
using WinApi.Extensions;

namespace WinApi.Logger
{
    public static class LoggerExtensions
    {

        public const string EventIdFormat = "<{{EventType:l}}.{{EventId}}> {0}";
        //   public const string DiagnosticsFormat = "{0}\n{{@Diagnostics}}";
        public static ConcurrentDictionary<Enum, int> ErrorsDictionary = new ConcurrentDictionary<Enum, int>();
        public static ConcurrentDictionary<Enum, ConcurrentBag<string>> RenderedErrorsDictionary = new ConcurrentDictionary<Enum, ConcurrentBag<string>>();
        public static bool ErrorsWarningSent = false;

        // public static Func<bool, IDiagnostics> DiagnosticsFunc { get; set; }

        public static ILogger With(this ILogger logger, string propertyName, object value, bool nothingOnNull)
        {
            if (nothingOnNull && value == null)
            {
                return logger;
            }
            return logger.ForContext(propertyName, value, destructureObjects: true);
        }

        public static ILogger With(this ILogger logger, string propertyName, object value)
        {
            return logger.With(propertyName, value, false);
        }

        public static void LogMessage(this ILogger logger, MessageBase message, object sender)
        {
            logger.Debug(Constants.ReceivedMessageFormat, message, sender);
        }

        public static void Verbose(this ILogger logger, Enum eventId, string messageTemplate, params object[] propertyValues)
        {
            if (!logger.IsEnabled(LogEventLevel.Verbose))
            {
                return;
            }
            var allPropertyValues = new object[] { eventId.GetType().Name, eventId }.Concat(propertyValues).ToArray();
            var allMessageTemplate = string.Format(EventIdFormat, messageTemplate);
            logger.Verbose(allMessageTemplate, allPropertyValues);
        }

        public static void Debug(this ILogger logger, Enum eventId, string messageTemplate, params object[] propertyValues)
        {
            if (!logger.IsEnabled(LogEventLevel.Debug))
            {
                return;
            }
            var allPropertyValues = new object[] { eventId.GetType().Name, eventId }.Concat(propertyValues).ToArray();
            var allMessageTemplate = string.Format(EventIdFormat, messageTemplate);
            logger.Debug(allMessageTemplate, allPropertyValues);
        }

        public static void Debug(this ILogger logger, Enum eventId)
        {
            if (!logger.IsEnabled(LogEventLevel.Debug))
            {
                return;
            }
            var allMessageTemplate = string.Format(EventIdFormat, eventId.GetDescription());
            logger.Debug(allMessageTemplate, eventId.GetType().Name, eventId);
        }

        public static void Warning(this ILogger logger, Enum eventId, string messageTemplate, params object[] propertyValues)
        {
            if (!logger.IsEnabled(LogEventLevel.Warning))
            {
                return;
            }
            var allPropertyValues = new object[] { eventId.GetType().Name, eventId }.Concat(propertyValues).ToArray();
            var allMessageTemplate = string.Format(EventIdFormat, messageTemplate);
            logger.Warning(allMessageTemplate, allPropertyValues);
        }

        public static void Warning(this ILogger logger, Enum eventId)
        {
            if (!logger.IsEnabled(LogEventLevel.Warning))
            {
                return;
            }
            var allMessageTemplate = string.Format(EventIdFormat, eventId.GetDescription());
            logger.Warning(allMessageTemplate, eventId.GetType().Name, eventId);
        }

        public static void Information(this ILogger logger, Enum eventId, string messageTemplate, params object[] propertyValues)
        {
            if (!logger.IsEnabled(LogEventLevel.Information))
            {
                return;
            }
            var allPropertyValues = new object[] { eventId.GetType().Name, eventId }.Concat(propertyValues).ToArray();
            var allMessageTemplate = string.Format(EventIdFormat, messageTemplate);
            logger.Information(allMessageTemplate, allPropertyValues);
        }

        public static void Information(this ILogger logger, Enum eventId)
        {
            if (!logger.IsEnabled(LogEventLevel.Information))
            {
                return;
            }
            var allMessageTemplate = string.Format(EventIdFormat, eventId.GetDescription());
            logger.Information(allMessageTemplate, eventId.GetType().Name, eventId);
        }

        public static void Error(this ILogger logger, Enum eventId, string messageTemplate, params object[] propertyValues)
        {
            ErrorsDictionary.AddOrUpdate(eventId, 1, (id, value) => value + 1);
            if (!logger.IsEnabled(LogEventLevel.Error))
            {
                return;
            }
            var allPropertyValues = new object[] { eventId.GetType().Name, eventId }.Concat(propertyValues).ToArray();
            var allMessageTemplate = string.Format(EventIdFormat, messageTemplate);
            logger.Error(allMessageTemplate, allPropertyValues);
            AddRenderedMessage(eventId);
        }

        public static void Error(this ILogger logger, Enum eventId)
        {
            ErrorsDictionary.AddOrUpdate(eventId, 1, (id, value) => value + 1);
            if (!logger.IsEnabled(LogEventLevel.Error))
            {
                return;
            }
            var allMessageTemplate = string.Format(EventIdFormat, eventId.GetDescription());
            logger.Error(allMessageTemplate, eventId.GetType().Name, eventId);
            AddRenderedMessage(eventId);
        }

        public static void Error(this ILogger logger, Exception ex, Enum eventId)
        {
            ErrorsDictionary.AddOrUpdate(eventId, 1, (id, value) => value + 1);
            if (!logger.IsEnabled(LogEventLevel.Error))
            {
                return;
            }
            var allMessageTemplate = string.Format(EventIdFormat, eventId.GetDescription());
            logger.Error(ex, allMessageTemplate, eventId.GetType().Name, eventId);
            AddRenderedMessage(eventId);
        }

        public static void Error(this ILogger logger, Exception ex, Enum eventId, string messageTemplate, params object[] propertyValues)
        {
            ErrorsDictionary.AddOrUpdate(eventId, 1, (id, value) => value + 1);
            if (!logger.IsEnabled(LogEventLevel.Error))
            {
                return;
            }
            var allPropertyValues = new object[] { eventId.GetType().Name, eventId }.Concat(propertyValues).ToArray();
            var allMessageTemplate = string.Format(EventIdFormat, messageTemplate);
            logger.Error(ex, allMessageTemplate, allPropertyValues);
            AddRenderedMessage(eventId);
        }

        public static void Fatal(this ILogger logger, Enum eventId, string messageTemplate, bool includeStackTrace, params object[] propertyValues)
        {
            if (!logger.IsEnabled(LogEventLevel.Fatal))
            {
                return;
            }
            var allPropertyValues = new object[] { eventId.GetType().Name, eventId }.Concat(propertyValues).ToArray();
            var allMessageTemplate = string.Format(EventIdFormat, messageTemplate);
            //logger = logger.With("Diagnostics", DiagnosticsFunc?.Invoke(includeStackTrace));
            logger.Fatal(allMessageTemplate, allPropertyValues);
        }

        public static void Fatal(this ILogger logger, Exception ex, Enum eventId, string messageTemplate, bool includeStackTrace, params object[] propertyValues)
        {
            if (!logger.IsEnabled(LogEventLevel.Fatal))
            {
                return;
            }
            var allPropertyValues = new object[] { eventId.GetType().Name, eventId }.Concat(propertyValues).ToArray();
            var allMessageTemplate = string.Format(EventIdFormat, messageTemplate);
            // logger = logger.With("Diagnostics", DiagnosticsFunc?.Invoke(includeStackTrace));
            logger.Fatal(ex, allMessageTemplate, allPropertyValues);
        }

        public static void Fatal(this ILogger logger, Exception ex, Enum eventId, bool includeStackTrace)
        {
            if (!logger.IsEnabled(LogEventLevel.Fatal))
            {
                return;
            }
            var allMessageTemplate = string.Format(EventIdFormat, eventId.GetDescription());
            //logger = logger.With("Diagnostics", DiagnosticsFunc?.Invoke(includeStackTrace));
            logger.Fatal(ex, allMessageTemplate, eventId.GetType().Name, eventId);
        }

        public static IDisposable BeginTimedOperation(this ILogger logger, Enum eventId, string description = null)
        {
            if (!logger.IsEnabled(LogEventLevel.Debug))
            {
                return null;
            }
            var beginningMessage = description == null
                ? "Beginning operation {TimedOperationId}"
                : SerilogMetrics.TimedOperation.BeginningOperationTemplate;
            var completedMessage = description == null
                ? "Completed operation {TimedOperationId} {TimedOperationDescription} in {TimedOperationElapsed} ({TimedOperationElapsedInMs} ms)"
                : SerilogMetrics.TimedOperation.CompletedOperationTemplate;
            return logger.BeginTimedOperation(description, $"{eventId.GetType().Name}.{eventId:G}", LogEventLevel.Debug, beginningMessage: beginningMessage, completedMessage: completedMessage);
        }

        public static void SendErrorsWarningMessage(ILogger logger)
        {
            //var errorCount = ErrorsDictionary.Sum(e => e.Value);
            //if (!ErrorsWarningSent && errorCount >= Constants.MinimalErrorCountToWarning)
            //{
            //    var renderedErrors = new List<string>();
            //    foreach (var entry in RenderedErrorsDictionary)
            //    {
            //        renderedErrors.AddRange(entry.Value);
            //    }
            //    const string message = "Too many errors ({ErrorCount}) occured in specified interval of {Interval}s.\n\nList of events:\n{@ErrorsDictionary}\n\nRendered errors:\n{@RenderedErrorsDictionary:l}";
            //   // logger.Fatal(ApplicationEvents.CheckErrors, message, false, errorCount, Constants.ErrorCountInterval.TotalSeconds, ErrorsDictionary, renderedErrors);
            //    ErrorsWarningSent = true;
            //}
            //RenderedErrorsDictionary.Clear();
            //ErrorsDictionary.Clear();
        }

        private static void AddRenderedMessage(Enum eventId)
        {
            RenderedErrorsDictionary.AddOrUpdate(eventId, new ConcurrentBag<string>(), (id, list) =>
            {
                //  list.Add(StringSink.LastEvent);
                return list;
            });
        }
    }
}
