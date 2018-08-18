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

        public OptionsService()
        {
            this.Options = new OptionsModel();
            LoadOptionsFromSetting();
        }

        public void LoadOptionsFromSetting()
        {
            this.Options.ApiLink = Properties.Settings.Default.ApiLink;
            this.Options.Apikey = Properties.Settings.Default.ApiKey;
            this.Options.ObjecID = Properties.Settings.Default.ObjecID;
            this.Options.UserID = Properties.Settings.Default.UserID;
            this.Options.ProgramPath = Properties.Settings.Default.ProgramPath;
            this.Options.ProcessName = Properties.Settings.Default.ProcessName;
            this.Options.PusherKey = Properties.Settings.Default.PusherKey;
            this.Options.PusherAuthorizer = Properties.Settings.Default.PusherAuthorizer;
            this.Options.PusherON = Properties.Settings.Default.PusherON;
            this.Options.Succes = Properties.Settings.Default.Success;
            this.Options.InProcess = Properties.Settings.Default.InProcess;
            this.Options.Succes = true;
        }

        public void SaveOptionsToSetting()
        {
            Task.Run(() =>
            {
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
            }).ContinueWith(async t =>
            {

                //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                //{
                //    //    Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "Options Setting", Msg = "Nastavenia boli uspesne ulozene", IconType = Notifications.Wpf.NotificationType.Success, ExpTime = 3 });
                //});

                await Task.Delay(TimeSpan.FromSeconds(4));

            });
            //System.Threading.Thread.Sleep(1500);
        }
    }
}
