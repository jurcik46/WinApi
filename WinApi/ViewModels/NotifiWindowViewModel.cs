using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Messages;

namespace WinApi.ViewModels
{
    public class NotifiWindowViewModel : ViewModelBase
    {

        private string _loadingIcon = @"pack://application:,,,/Resources/img/Reload-1s-200px.gif";

        public string LoadingIcon { get { return _loadingIcon; } set { _loadingIcon = value; RaisePropertyChanged(); } }


        public NotifiWindowViewModel()
        {


            Messenger.Default.Register<BozpStatusPusherMessage>(this, (message) =>
            {
                ///LoadingIcon = @"pack://application:,,,/Resources/img/ok.png";

            });

        }
    }
}
