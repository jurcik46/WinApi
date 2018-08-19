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
        private string _aaa;
        private Icon _icon;
        private IRestService _restService;
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
        public string Aaa { get => _aaa; set => _aaa = value; }
        public RelayCommand Options { get => _options; set => _options = value; }
        public RelayCommand Signature { get => _signature; set => _signature = value; }



        public string ToolTipText { get; set; } = "Dvojklik pre podpísanie a kliknutím pravým tlačidlom pre menu";

        public TrayIconViewModel()
        {
            // this.Icon = new System.Drawing.Icon(@"..\Properties\Icons\YourIcon.ico");
            //this.Icon = Properties.Resources.online;
            //Console.WriteLine(Properties.Resources.online);
            this._restService = ViewModelLocator.RestService;
            Aaa = "/Resources/Icons/online.ico";
            //var notificationManager = new NotificationManager();
            //_notificationManager.Show("asdsadasdasd");
            // _notificationManager.Show(new NotificationContent { Title = "asdad", Message = "asdsad" });

            //notificationManager.Show(new NotificationContent
            //{
            //    Title = "Sample notification",
            //    Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            //    Type = NotificationType.Success
            //}, expirationTime: TimeSpan.FromSeconds(60));


            //var timer = new System.Timers.Timer { Interval = 1000 };
            //timer.Elapsed += (sender, args) => _notificationManager.Show("Pink string from another thread!");
            //timer.Start();

            // Test. = Properties.Resources.online;
            this.CommandInit();
            this.MessagesInit();
        }

        #region Message and Command Init
        private void MessagesInit()
        {
            Messenger.Default.Register<NotifiMessage>(this, (message) =>
            {
                this._notificationManager.Show(new NotificationContent { Title = message.Title, Message = message.Msg, Type = message.IconType }, expirationTime: System.TimeSpan.FromSeconds(message.ExpTime));
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
            return true;
        }

        private void DoSignature()
        {
            var test = ViewModelLocator.RestService.GetDocumentToSignature();
            Console.WriteLine(test.Hash);
            Messenger.Default.Send<ChangeIconMessage>(new ChangeIconMessage() { Icon = Enums.TrayIcons.Working });


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
