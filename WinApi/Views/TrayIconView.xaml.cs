using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Messages;
using Hardcodet.Wpf.TaskbarNotification;
using GalaSoft.MvvmLight.Messaging;
using WinApi.ViewModels;
using Notifications.Wpf;
using System.Threading;

namespace WinApi.Views
{
    public partial class TrayIconView
    {
        private readonly NotificationManager _notificationManager = new NotificationManager();

        public TrayIconView()
        {
            this.RegistrationMessage();
        }

        #region Message Registration
        private void RegistrationMessage()
        {
            Messenger.Default.Register<ShowBallonTipMessage>(this, (message) =>
            {
                trayIconTaskbar.ShowBalloonTip(message.Title, message.Msg, message.IconType);
            });

            Messenger.Default.Register<NotifiMessage>(this, (message) =>
            {
                this._notificationManager.Show(new NotificationContent { Title = message.AppName + message.Title, Message = message.Msg, Type = message.IconType }, expirationTime: System.TimeSpan.FromSeconds(message.ExpTime));
            });

            Messenger.Default.Register<ChangeIconMessage>(this, (message) =>
            {
                switch (message.Icon)
                {
                    case Enums.TrayIcons.Online:
                        trayIconTaskbar.Icon = Properties.Resources.online;
                        break;
                    case Enums.TrayIcons.Offline:
                        trayIconTaskbar.Icon = Properties.Resources.offline;
                        break;
                    case Enums.TrayIcons.Working:
                        trayIconTaskbar.Icon = Properties.Resources.working;
                        break;
                    default:
                        break;
                }
            });
        }
        #endregion
    }
}
