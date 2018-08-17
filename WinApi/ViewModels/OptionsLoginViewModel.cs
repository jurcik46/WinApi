using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WinApi.Interfaces;
using WinApi.Interfaces.Service;
using WinApi.Messages;
using WinApi.Service;
using WinApi.Views;

namespace WinApi.ViewModels
{
    public class OptionsLoginViewModel : ViewModelBase
    {
        private string _passwrod;
        private IPasswordService _passwordService;
        private OptionsWindowView OptionsWindow;
        private RelayCommand<IClosable> _enter;

        public OptionsLoginViewModel(IPasswordService passwordService)
        {
            this._passwordService = passwordService;
            this.CommandInit();
        }



        private void CommandInit()
        {
            this.Enter = new RelayCommand<IClosable>(EnterToOptions, CanEnter);
        }

        private bool CanEnter(IClosable win)
        {
            if (this.OptionsWindow != null)
                return (this.OptionsWindow.IsLoaded) ? false : true;
            else
                return true;
        }

        private void EnterToOptions(IClosable win)
        {


            Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "asdsad", Msg = "asdsad", IconType = NotificationType.Success, ExpTime = 4 });

            this.OptionsWindow = new OptionsWindowView();
            this.OptionsWindow.Show();
            if (win != null)
            {
                win.Close();
            }
        }



        public string Passwrod { get => _passwrod; set => _passwrod = value; }
        public RelayCommand<IClosable> Enter { get => _enter; set => _enter = value; }
    }
}
