using GalaSoft.MvvmLight;
using Hardcodet.Wpf.TaskbarNotification;
using Notifications.Wpf;
using System.Windows;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using WinApi.Messages;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using WinApi.Views;
using System;
using WinApi.Interfaces.Service;
using System.Drawing;

namespace WinApi.ViewModels
{
    public class TrayIconViewModel : ViewModelBase
    {
        private RelayCommand _options;
        private RelayCommand _signature;
        private Icon _icon;
        private string _toolTipText = ViewModelLocator.rm.GetString("iconTitleDefualt");
        private IPusherService _pusherService;
        //private IRestService _restService;
        private ISignatureService _signatureService;
        private OptionsLoginWindowView OptionsLoginWindow;
        private readonly NotificationManager _notificationManager = new NotificationManager();

        public Icon Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                RaisePropertyChanged();
            }
        }
        public RelayCommand Options { get => _options; set => _options = value; }
        public RelayCommand Signature { get => _signature; set => _signature = value; }


        public string ToolTipText
        {
            get { return _toolTipText; }
            set
            {
                _toolTipText = value;
                RaisePropertyChanged();
            }
        }

        public TrayIconViewModel()
        {
            this._signatureService = ViewModelLocator.SignatureService;
            this._pusherService = ViewModelLocator.PusherService;


            this.CommandInit();
            this.MessagesInit();
        }

        #region Message and Command Init
        private void MessagesInit()
        {
            Messenger.Default.Register<ChangeIconMessage>(this, (message) =>
            {
                switch (message.Icon)
                {
                    case Enums.TrayIcons.Online:
                        ToolTipText = ViewModelLocator.rm.GetString("iconTitleDefualt");
                        break;
                    case Enums.TrayIcons.Offline:
                        ToolTipText = ViewModelLocator.rm.GetString("lostConnection");
                        break;
                    case Enums.TrayIcons.Working:
                        ToolTipText = ViewModelLocator.rm.GetString("iconTitleWorking");
                        break;
                    default:
                        break;
                }
            });
        }

        private void CommandInit()
        {
            this.Options = new RelayCommand(this.ShowOptionsLogin, this.CanShowOptionsLogin);
            this.Signature = new RelayCommand(this.DoSignature, this.CanSignature);
        }
        #endregion

        #region Signature double click Command
        private bool CanSignature()
        {
            //// Create the notification object
            //var newNotification = new Notification()
            //{
            //    Title = "Machine error",
            //    Message = "Error!! Please check your Machine Code and Try Again"
            //};
            //// call the ShowNotificationWindow Method with the notification object
            //_notifserve.ShowNotificationWindow(newNotification);

            if (!_signatureService.InProcces)
            {
                return true;
            }
            else
            {
                Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = ViewModelLocator.rm.GetString("signatureTitle"), Msg = ViewModelLocator.rm.GetString("inProccesDocument"), IconType = Notifications.Wpf.NotificationType.Error, ExpTime = 5 });
                return false;
            }
        }

        private void DoSignature()
        {
            Task.Run(() =>
            {
                _signatureService.StartSign();

            });
        }
        #endregion

        #region Options login command
        private bool CanShowOptionsLogin()
        {
            if (this.OptionsLoginWindow != null)
                return (this.OptionsLoginWindow.IsLoaded) ? false : true;
            else
                return true;
        }

        private void ShowOptionsLogin()
        {
            this.OptionsLoginWindow = new OptionsLoginWindowView();
            this.OptionsLoginWindow.Show();
        }

        #endregion



    }
}
