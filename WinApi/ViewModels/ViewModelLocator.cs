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
                SimpleIoc.Default.Register<IOptionsService, OptionsService>();
                SimpleIoc.Default.Register<IRestService, RestService>();
                SimpleIoc.Default.Register<ISignatureService, SignatureService>();
            }

            RegisterViewModels();

        }

        internal static void RegisterViewModels()
        {
            if (!SimpleIoc.Default.IsRegistered<TrayIconViewModel>())
            {
                //SimpleIoc.Default.Register(() => new TrayIconViewModel(ServiceLocator.Current.GetInstance<IRestService>()));
                SimpleIoc.Default.Register<TrayIconViewModel>();
            }

            if (!SimpleIoc.Default.IsRegistered<OptionsViewModel>())
            {
                SimpleIoc.Default.Register(() => new OptionsViewModel(ServiceLocator.Current.GetInstance<IOptionsService>()));
            }

            if (!SimpleIoc.Default.IsRegistered<OptionsLoginViewModel>())
            {
                SimpleIoc.Default.Register(() => new OptionsLoginViewModel(ServiceLocator.Current.GetInstance<IPasswordService>()));
            }

            if (!SimpleIoc.Default.IsRegistered<ChangePasswordViewModel>())
            {
                SimpleIoc.Default.Register(() => new ChangePasswordViewModel(ServiceLocator.Current.GetInstance<IPasswordService>()));
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        public static TrayIconViewModel TrayIconViewModel => ServiceLocator.Current.GetInstance<TrayIconViewModel>();
        public static OptionsLoginViewModel OptionsLoginViewModel => ServiceLocator.Current.GetInstance<OptionsLoginViewModel>();
        public static ChangePasswordViewModel ChangePasswordViewModel => ServiceLocator.Current.GetInstance<ChangePasswordViewModel>();
        public static OptionsViewModel OptionsViewModel => ServiceLocator.Current.GetInstance<OptionsViewModel>();
        public static IRestService RestService => ServiceLocator.Current.GetInstance<IRestService>();
        public static ISignatureService SignatureService => ServiceLocator.Current.GetInstance<ISignatureService>();



    }
}
