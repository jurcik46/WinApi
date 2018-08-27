using System.ComponentModel;


namespace WinApi.Enums
{
    public enum ApplicationEvents
    {
            DispatcherThread,

            CheckErrors,

            CheckUpdate,

            [Description("Last check for update was less than 12 hours ago.")]
            CheckUpdateCheckActual,

            [Description("Application is not deployed over network.")]
            NotDeployed,

            [Description("No application update is available at a time.")]
            UpdateNotAvailable,

            [Description("Starting timer for application update reminder.")]
            UpdateReminderStarted,

            [Description("Failed to initialize database.")]
            DatabaseError,

            SetLanguage,

            ApplicationEnded,

            ApplicationStarted,

            DispatcherUnhandledException,

            UnobservedTaskException,

            CurrentDomainUnhandledException,

            WindowAdjusted,

            TempFolderError,
    }
}
