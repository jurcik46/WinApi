using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Interfaces.Service;
using WinApi.Messages;

namespace WinApi.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private string _password1;
        private string _password2;
        private RelayCommand _changePassword;

        private IPasswordService _passwordService;

        public ChangePasswordViewModel(IPasswordService passwordService)
        {
            this.ChangePassword = new RelayCommand(Change, CanChange);
            this._passwordService = passwordService;
        }

        private bool CanChange()
        {

            return (!NotIdentical && !string.IsNullOrWhiteSpace(this.Password1) && Password1.Length >= 5);
        }

        private void Change()
        {

            _passwordService.CreatePass(this.Password1);
            Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "Zmena hesla", Msg = "Heslo bolo uspesne zmenene", IconType = Notifications.Wpf.NotificationType.Success, ExpTime = 4 });

        }

        public bool NotIdentical
        {
            get
            {
                var result = !String.Equals(Password1, Password2);
                return result;
            }
        }
        public string Password1 { get => _password1; set => _password1 = value; }
        public string Password2 { get => _password2; set => _password2 = value; }
        public RelayCommand ChangePassword { get => _changePassword; set => _changePassword = value; }
    }
}
