using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Views;

namespace WinApi.ViewModels
{
    public class OptionsLoginViewModel : ViewModelBase
    {
        private OptionsWindowView OptionsWindow;
        private RelayCommand _enter;

        public OptionsLoginViewModel()
        {
            this.CommandInit();
        }


        private void CommandInit()
        {
            this.Enter = new RelayCommand(this.EnterToOptions, this.CanEnter());
        }

        private bool CanEnter()
        {
            if (this.OptionsWindow != null)
                return (this.OptionsWindow.IsLoaded) ? false : true;
            else
                return true;
        }

        private void EnterToOptions()
        {
            this.OptionsWindow = new OptionsWindowView();
            this.OptionsWindow.Show();

        }

        public RelayCommand Enter { get => _enter; set => _enter = value; }
    }
}
