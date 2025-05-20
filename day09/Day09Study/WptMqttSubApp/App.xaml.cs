using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using WptMqttSubApp.ViewModels;
using WptMqttSubApp.Views;

namespace WptMqttSubApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var coordinator = DialogCoordinator.Instance;
            var viewModel = new MainViewModel(coordinator);
            var view = new MainView
            {
                DataContext = viewModel
            };
            view.ShowDialog();
        }
    }

}
