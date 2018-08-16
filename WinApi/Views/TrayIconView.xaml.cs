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
            Messenger.Default.Register<TestMessage>(this, (message) =>
            {

                trayIconTaskbar.ShowBalloonTip("asdad", message.AaT, BalloonIcon.Info);

                // ITestService test = ServiceLocator.Current.GetInstance<ITestService>();
                // test.Test = message.ButtonText;
            });
        }

        private void trayIconTaskbar_TrayMouseDoubleClick(object sender, System.Windows.RoutedEventArgs e)
        {
        }
    }
}
