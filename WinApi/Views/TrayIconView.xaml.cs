using WinApi.Messages;
using GalaSoft.MvvmLight.Messaging;
using Notifications.Wpf;

namespace WinApi.Views
{
    public partial class TrayIconView
    {
        private NotifiWindowView notifiWindow;

        public TrayIconView()
        {
            notifiWindow = new NotifiWindowView();
            notifiWindow.Show();
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
