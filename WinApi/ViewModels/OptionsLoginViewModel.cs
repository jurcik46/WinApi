using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WinApi.Views;

namespace WinApi.ViewModels
{
    public class OptionsLoginViewModel : ViewModelBase
    {
        private string _passwrod;
        private OptionsWindowView OptionsWindow;
        private RelayCommand<Window> _enter;

        public OptionsLoginViewModel()
        {
            this.CommandInit();
        }



        private void CommandInit()
        {
            this.Enter = new RelayCommand<Window>(EnterToOptions, CanEnter);
        }

        private bool CanEnter(Window win)
        {
            //var notificationManager = new NotificationManager();
            //notificationManager.Show(new NotificationContent
            //{
            //    Title = Properties.Settings.Default.password,
            //    Message = this.Passwrod,
            //    Type = NotificationType.Success
            //}, expirationTime: TimeSpan.FromSeconds(2));
            if (this.OptionsWindow != null)
                return (this.OptionsWindow.IsLoaded) ? false : true;

            if (this.Passwrod == Properties.Settings.Default.password)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void EnterToOptions(Window win)
        {
            this.OptionsWindow = new OptionsWindowView();
            this.OptionsWindow.Show();
            if (win != null)
            {
                win.Close();
            }
        }



        public string Passwrod { get => _passwrod; set => _passwrod = value; }
        public RelayCommand<Window> Enter { get => _enter; set => _enter = value; }
    }
}
