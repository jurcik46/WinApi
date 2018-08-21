using GalaSoft.MvvmLight.Messaging;
using PusherClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Interfaces.Model;
using WinApi.Interfaces.Service;
using WinApi.Messages;

namespace WinApi.Service
{
    public class PusherService : IPusherService
    {
        private Pusher _pusher = null;
        private Channel _channel = null;
        private bool _online;
        private IPusherOptionModel _pusherOption;
        private IApiOptionModel _apiOtion;
        private ISignatureService _signatureService;

        public PusherService(IOptionsService optionsService, ISignatureService signatureService)
        {
            _pusherOption = optionsService.PusherOptions;
            _apiOtion = optionsService.ApiOptions;
            _signatureService = signatureService;

            MessagesInit();
            StartPusher();
        }


        #region  Pusher Start
        public void StartPusher()
        {
            if (_pusherOption.PusherON)
            {
                _pusher = new Pusher(_pusherOption.PusherKey, new PusherOptions()
                {
                    Authorizer = new HttpAuthorizer(_pusherOption.PusherAuthorizer),
                    Encrypted = true,
                    Cluster = "eu"
                });
                InitPusher();
            }
        }

        private void InitPusher()
        {
            _channel = _pusher.Subscribe("private-bozp-" + _apiOtion.ObjectID);
            _pusher.Connect();

            _pusher.ConnectionStateChanged += PusherConnectionStateChanged;
            _pusher.Error += PusherError;
            PusherBinding();

        }
        #endregion

        #region Pusher Connection change and Error
        public void PusherConnectionStateChanged(object sender, ConnectionState state)
        {

            if (state == ConnectionState.Connected)
            {

                Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "Status pripojenia", Msg = "Aplikácia  je pripojena k internetu", IconType = Notifications.Wpf.NotificationType.Success, ExpTime = 4 });
                Messenger.Default.Send<ChangeIconMessage>(new ChangeIconMessage() { Icon = Enums.TrayIcons.Online });

                _online = true;
            }
            if (state == ConnectionState.Disconnected)
            {

                if (_online)
                {
                    Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "Status pripojenia", Msg = "Aplikácia stratila pripojenie k internetu", IconType = Notifications.Wpf.NotificationType.Warning, ExpTime = 4 });
                    Messenger.Default.Send<ChangeIconMessage>(new ChangeIconMessage() { Icon = Enums.TrayIcons.Offline });

                    _online = false;
                }
            }
            if (state == ConnectionState.Connecting)
            {
                _pusher.Connect();

            }
        }


        private void PusherError(object sender, PusherException error)
        {
            if (error != null)
            {
                Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = "Status pripojenia", Msg = "Aplikácia stratila pripojenie k internetu", IconType = Notifications.Wpf.NotificationType.Warning, ExpTime = 4 });
                Messenger.Default.Send<ShowBallonTipMessage>(new ShowBallonTipMessage() { Title = "Status pripojenia", Msg = "Aplikácia stratila pripojenie k internetu", IconType = Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Error });

                //trayIconTaskbar.ShowBalloonTip("Chyba", error.ToString(), BalloonIcon.Error);
                //Log.Error("Pusher Error Msg = {0}", error.ToString());
            }

        }
        #endregion

        #region Pusher channel binding
        private void PusherBinding()
        {
            _channel.Bind(String.Format("event-{0}", _apiOtion.UserID), (dynamic data) =>
             {
                 if (!_signatureService.InProcces)
                 {
                     Task.Run(() =>
                     {
                         _signatureService.StartSign();

                     });
                 }
                 // Log.Information("Bind na event : event-{0}", option.Data.UserID);
                 //this.SingAction();
             });
        }


        #endregion

        #region Messages Init
        private void MessagesInit()
        {
            Messenger.Default.Register<BozpStatusPusherMessage>(this, (message) =>
            {
                SendBozpStatus(message.Status);
                //log.information("odoslanie správy pre pusher: event = client-event-{0}, msg = {1}", opt.data.userid, msg);
            });
        }
        #endregion

        #region Pusher sending method
        public void SendBozpStatus(string msg)
        {
            //            log.information("odoslanie správy pre pusher: event = client-event-{0}, msg = {1}", opt.data.userid, msg);
            _channel.Trigger(string.Format("client-event-{0}", _apiOtion.UserID), new { status = msg });
        }
        #endregion


    }
}
