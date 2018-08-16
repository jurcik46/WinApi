using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Messages;
using Hardcodet.Wpf.TaskbarNotification;
using GalaSoft.MvvmLight.Messaging;

namespace WinApi.Views
{
    public partial class TrayIconView
    {
        public TrayIconView()
        {
            this.RegistrationMessage();
        }

        private void RegistrationMessage()
        {
            Messenger.Default.Register<ShowBallonTipMessage>(this, (message) =>
            {

                trayIconTaskbar.ShowBalloonTip(message.Title, message.Msg, message.IconType);

                // ITestService test = ServiceLocator.Current.GetInstance<ITestService>();
                // test.Test = message.ButtonText;
            });

        }

        private void trayIconTaskbar_TrayMouseDoubleClick(object sender, System.Windows.RoutedEventArgs e)
        {
        }
    }
}
