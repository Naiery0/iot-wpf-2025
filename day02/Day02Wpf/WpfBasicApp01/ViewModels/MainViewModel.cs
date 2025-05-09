using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace WpfBasicApp01.ViewModels
{
    class MainViewModel : Conductor<object>
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        private string _greeting;
        public string Greeting
        {
            get => _greeting;
            set
            {
                _greeting = value;
                NotifyOfPropertyChange(() => Greeting);
            }
        }
        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            Greeting = "Hello, Caliburn.Micro!!";
        }

        public async void SayHello()
        {
            Greeting = "Hi, Everyone~!";
            //MessageBox.Show("Hi, Everyone~!", "Greeting", MessageBoxButton.OK, MessageBoxImage.Information);
            await _dialogCoordinator.ShowMessageAsync(this, "Greeting", "Hi, Everyone~!");
            
        }
    }
}
