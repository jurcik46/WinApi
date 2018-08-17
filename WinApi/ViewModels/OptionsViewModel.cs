using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Messages;
using WinApi.Models;

namespace WinApi.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
        private OptionsModel _options;
        private RelayCommand _saveOptions;


        public OptionsViewModel()
        {
            this.Options = new OptionsModel();

        }

        private void CommandInit()
        {
            this.SaveOptions = new RelayCommand(SaveOptionsToSetting);
        }

        private void SaveOptionsToSetting()
        {
            // Log.Information("Ukladanie nastaveni : {0}", Data.ToString());
            Properties.Settings.Default.ApiLink = this.Options.ApiLink;
            Properties.Settings.Default.ApiKey = this.Options.Apikey;
            Properties.Settings.Default.ObjecID = this.Options.ObjecID;
            Properties.Settings.Default.UserID = this.Options.UserID;
            Properties.Settings.Default.ProgramPath = this.Options.ProgramPath;
            Properties.Settings.Default.ProcessName = this.Options.ProcessName;
            Properties.Settings.Default.PusherKey = this.Options.PusherKey;
            Properties.Settings.Default.PusherAuthorizer = this.Options.PusherAuthorizer;
            Properties.Settings.Default.PusherON = this.Options.PusherON;
            Properties.Settings.Default.Success = this.Options.Succes;
            Properties.Settings.Default.InProcess = this.Options.InProcess;

            Properties.Settings.Default.Save();

            Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "Options Setting", Msg = "Nastavenia boli uspesne ulozene", IconType = Notifications.Wpf.NotificationType.Success, ExpTime = 4 });

        }

        public RelayCommand SaveOptions { get => _saveOptions; set => _saveOptions = value; }
        internal OptionsModel Options { get => _options; set => _options = value; }
    }
}
