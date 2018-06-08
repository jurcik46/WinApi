
using System.Windows;
using System.Windows.Input;

namespace WinApi.Commands
{
    /// <summary>
    /// Closes app.
    /// </summary>
    public class RestartAppCommand : CommandBase<RestartAppCommand>
    {
        public override void Execute(object parameter)
        {

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }


        public override bool CanExecute(object parameter)
        {     
            
            return true;
        }
    }
}

