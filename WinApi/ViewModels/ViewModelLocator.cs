using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using WinApi.Interfaces.Service;
using WinApi.Service;

namespace WinApi.ViewModels
{
    public static class ViewModelLocator
    {

        static ViewModelLocator()
        {

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                //SimpleIoc.Default.Register<ITestService, DesignTestService>();
            }
            else
            {
                // Create run time view services and models
                // SimpleIoc.Default.Register<ITestService, TestService>();
                SimpleIoc.Default.Register<IPasswordService, PasswordService>();

            }

            RegisterViewModels();

        }

        internal static void RegisterViewModels()
        {
            if (!SimpleIoc.Default.IsRegistered<TrayIconViewModel>())
            {
                SimpleIoc.Default.Register<TrayIconViewModel>();
            }

            if (!SimpleIoc.Default.IsRegistered<OptionsViewModel>())
            {
                SimpleIoc.Default.Register<OptionsViewModel>();
            }

            if (!SimpleIoc.Default.IsRegistered<OptionsLoginViewModel>())
            {
                SimpleIoc.Default.Register(() => new OptionsLoginViewModel(ServiceLocator.Current.GetInstance<IPasswordService>()));
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        public static TrayIconViewModel TrayIconViewModel => ServiceLocator.Current.GetInstance<TrayIconViewModel>();
        public static OptionsLoginViewModel OptionsLoginViewModel => ServiceLocator.Current.GetInstance<OptionsLoginViewModel>();


    }
}
