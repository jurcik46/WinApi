using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Messages;
using Hardcodet.Wpf.TaskbarNotification;
using GalaSoft.MvvmLight.Messaging;
using WinApi.ViewModels;

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

            Messenger.Default.Register<ShowBallonTipMessage>(this, (message) =>
            {

                trayIconTaskbar.ShowBalloonTip(message.Title, message.Msg, message.IconType);

                // ITestService test = ServiceLocator.Current.GetInstance<ITestService>();
                // test.Test = message.ButtonText;
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

            //  tb = (TaskbarIcon)Application.Current.FindResource("MyTray");


        }

        private void trayIconTaskbar_TrayMouseDoubleClick(object sender, System.Windows.RoutedEventArgs e)
        {
        }
    }
}
