using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinApi.Interfaces.Service;
using WinApi.Messages;
using WinApi.Models;

namespace WinApi.ViewModels
{
    public class OptionsViewModel : ViewModelBase
    {
        private RelayCommand _saveOptions;

        public IOptionsService OptionsService { get; set; }

        public OptionsViewModel(IOptionsService optionsService)
        {
            this.OptionsService = optionsService;
            this.CommandInit();
        }

        private void CommandInit()
        {
            this.SaveOptions = new RelayCommand(SaveOptionsToSetting, CanSave);
        }

        private bool CanSave()
        {
            return true;
        }

        private void SaveOptionsToSetting()
        {

            OptionsService.SaveOptionsToSetting();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        public RelayCommand SaveOptions { get => _saveOptions; set => _saveOptions = value; }
    }
}
