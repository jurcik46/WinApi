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
        public const string DateTimeFormatService = "yyyy-MM-dd HH:mm:ss";
    }
}
