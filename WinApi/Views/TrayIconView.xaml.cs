using WinApi.Messages;
using GalaSoft.MvvmLight.Messaging;
using Notifications.Wpf;

namespace WinApi.Views
{
    public partial class TrayIconView
    {
        private readonly NotificationManager _notificationManager = new NotificationManager();
        private NotificationContent conetn;
        // private NotificationArea _n = new NotificationArea();

        public TrayIconView()
        {
            conetn = new NotificationContent();

            //_n.MaxItems = 1;
            // _n.Position = NotificationPosition.BottomLeft;
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
                conetn.Title = message.Title;
                conetn.Message = message.Msg;
                conetn.Type = message.IconType;

                this._notificationManager.Show(conetn, expirationTime: System.TimeSpan.FromSeconds(message.ExpTime));
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
