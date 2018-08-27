using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Logger
{
    public class Constants
    {
        public const string NotAvailable = @"N\A";
        public const string FileLogFormat = @"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {MachineName}:{ThreadId} [{Level}] {Message}{NewLine}{Exception}";
        public const string EmailLogFormat = @"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {MachineName}:{ThreadId} [{Level}] {Message}{NewLine}{Exception}{NewLine}{@Diagnostics}";
        public const string ReceivedMessageFormat = @"Message: {@message} Target: {target}";
        public const string EmailSubjectFormat = @"{0} Error on {1}";
        public const string FromEmail = @"applicationslogs@outlook.com";
        public const string ToEmail = @"applicationslogs+entrance@gmail.com";
        public const string MailServer = @"smtp-mail.outlook.com";
        public const string MailUser = @"applicationslogs@outlook.com";
        public const string MailPassword = @"fdkfqltuevthqvnt";
        public const int MinimalErrorCountToWarning = 10;
        public static TimeSpan ErrorCountInterval = TimeSpan.FromMinutes(1);
        public static TimeSpan UpdateInterval = TimeSpan.FromMinutes(30);
        public const int ECVMinLength = 4;
        public const int PhoneNumberMinLength = 5;
        public const int CardNumberMinLength = 8;
        public const int CardNumberMaxLength = 15;
        public const int IntMinimum = 0;
        public const int IntMaximum = 10000;
        public const string ApplicationCategory = "Application";
        public const string SignCategory = "Sign";
        public const string DeliveryCategory = "Delivery";
        public const string DeliverySynchronizationCategory = "Delivery Synchronization";
        public const string LogCategory = "Logging";
        public const string NFCCategory = "NFC";
        public const string RestCategory = "REST API";
        public const string SynchronizationCategory = "Synchronization";
        public const string V2Category = "Version2";
        public const string DateTimeFormatService = "yyyy-MM-dd HH:mm:ss";
    }
}
