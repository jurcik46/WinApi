using WinApi.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Models;
using WinApi.Messages;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

namespace WinApi.Service
{
    public class OptionsService : IOptionsService
    {
        public OptionsModel Options { get; set; }
        public ApiOptionModel ApiOptions { get; set; }
        public PusherOptionModel PusherOptions { get; set; }
        public SignatureOptionModel SignatureOptions { get; set; }

        public OptionsService()
        {
            this.Options = new OptionsModel();
            this.ApiOptions = new ApiOptionModel();
            this.PusherOptions = new PusherOptionModel();
            this.SignatureOptions = new SignatureOptionModel();
            LoadOptionsFromSetting();
        }

        public void LoadOptionsFromSetting()
        {
            this.ApiOptions.ApiLink = Properties.Settings.Default.ApiLink;
            this.ApiOptions.Apikey = Properties.Settings.Default.ApiKey;
            this.ApiOptions.ObjectID = Properties.Settings.Default.ObjecID;
            this.ApiOptions.UserID = Properties.Settings.Default.UserID;
            this.SignatureOptions.ProgramPath = Properties.Settings.Default.ProgramPath;
            this.SignatureOptions.ProcessName = Properties.Settings.Default.ProcessName;
            this.PusherOptions.PusherKey = Properties.Settings.Default.PusherKey;
            this.PusherOptions.PusherAuthorizer = Properties.Settings.Default.PusherAuthorizer;
            this.PusherOptions.PusherON = Properties.Settings.Default.PusherON;
            this.Options.Succes = Properties.Settings.Default.Success;
            this.Options.InProcess = Properties.Settings.Default.InProcess;
            this.Options.Succes = true;
        }

        public void SaveOptionsToSetting()
        {
            Task.Run(() =>
            {
                Properties.Settings.Default.ApiLink = this.ApiOptions.ApiLink;
                Properties.Settings.Default.ApiKey = this.ApiOptions.Apikey;
                Properties.Settings.Default.ObjecID = this.ApiOptions.ObjectID;
                Properties.Settings.Default.UserID = this.ApiOptions.UserID;
                Properties.Settings.Default.ProgramPath = this.SignatureOptions.ProgramPath;
                Properties.Settings.Default.ProcessName = this.SignatureOptions.ProcessName;
                Properties.Settings.Default.PusherKey = this.PusherOptions.PusherKey;
                Properties.Settings.Default.PusherAuthorizer = this.PusherOptions.PusherAuthorizer;
                Properties.Settings.Default.PusherON = this.PusherOptions.PusherON;
                Properties.Settings.Default.Success = this.Options.Succes;
                Properties.Settings.Default.InProcess = this.Options.InProcess;

                Properties.Settings.Default.Save();

                //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                //{
                //    Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "dd", Msg = "dd", IconType = Notifications.Wpf.NotificationType.Success, ExpTime = 4 });

                //});

            }).ContinueWith(async t =>
            {


                await Task.Delay(TimeSpan.FromSeconds(4));
            });

        }
    }
}
