﻿using GalaSoft.MvvmLight.Messaging;
using PusherClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Enums;
using WinApi.Interfaces.Model;
using WinApi.Interfaces.Service;
using WinApi.Logger;
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

        public ILogger Logger => Log.Logger.ForContext<PusherService>();


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
                Logger.With("PusherKey", _pusherOption.PusherKey)
                   .Debug(PusherServiceEvents.StartPusher);
                try
                {

                    _pusher = new Pusher(_pusherOption.PusherKey, new PusherOptions()
                    {
                        Authorizer = new HttpAuthorizer(_pusherOption.PusherAuthorizer),
                        Encrypted = true,
                        Cluster = "eu"
                    });

                    InitPusher();


                }
                catch (Exception ex)
                {
                    Logger.Error(ex, PusherServiceEvents.StartPusherError);
                }

            }
        }

        private void InitPusher()
        {
            Logger.With("Subscribe", "private-bozp-" + _apiOtion.ObjectID)
                    .Debug(PusherServiceEvents.InitPusher);

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

                Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = ViewModels.ViewModelLocator.rm.GetString("connectionTitle"), Msg = ViewModels.ViewModelLocator.rm.GetString("onlineConnection"), IconType = Notifications.Wpf.NotificationType.Success });
                Messenger.Default.Send<ChangeIconMessage>(new ChangeIconMessage() { Icon = Enums.TrayIcons.Online });

                _online = true;
            }
            if (state == ConnectionState.Disconnected)
            {

                if (_online)
                {
                    Messenger.Default.Send<NotifiMessage>(new NotifiMessage() { Title = ViewModels.ViewModelLocator.rm.GetString("connectionTitle"), Msg = ViewModels.ViewModelLocator.rm.GetString("lostConnection"), IconType = Notifications.Wpf.NotificationType.Error });
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
                Logger.Error(error, PusherServiceEvents.PusherError);
            }

        }
        #endregion

        #region Pusher channel binding
        private void PusherBinding()
        {
            _channel.Bind(String.Format("event-{0}", _apiOtion.UserID), (dynamic data) =>
             {
                 Logger.Debug(PusherServiceEvents.PusherBindingSign);

                 if (!_signatureService.InProcces)
                 {
                     Logger.Information(PusherServiceEvents.PusherBindingSignStart);

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
                if (_pusherOption.PusherON)
                    SendBozpStatus(message.Status);
                //log.information("odoslanie správy pre pusher: event = client-event-{0}, msg = {1}", opt.data.userid, msg);
            });
        }
        #endregion

        #region Pusher sending method
        public void SendBozpStatus(string msg)
        {
            //            log.information("odoslanie správy pre pusher: event = client-event-{0}, msg = {1}", opt.data.userid, msg);
            Logger.With("Message", msg)
                .Information(PusherServiceEvents.PusherBindingSignStart);

            _channel.Trigger(string.Format("client-event-{0}", _apiOtion.UserID), new { status = msg });
        }
        #endregion


    }
}
