
using System.Windows;
using System.Windows.Input;

namespace WinApi.Commands
{
    /// <summary>
    /// Closes app.
    /// </summary>
    public class ExitAppCommand : CommandBase<ExitAppCommand>
    {
        public override void Execute(object parameter)
        {
        
            Application.Current.Shutdown();
        }


        public override bool CanExecute(object parameter)
        {     
            
            return true;
        }
    }
}

