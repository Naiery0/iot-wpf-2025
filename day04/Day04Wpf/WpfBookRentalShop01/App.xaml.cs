using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using WpfBookRentalShop01.Helpers;
using WpfBookRentalShop01.ModelViews;
using WpfBookRentalShop01.Views;

namespace WpfBookRentalShop01
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var coordibator = DialogCoordinator.Instance;

            Common.DIALOGCOORDINATOR = DialogCoordinator.Instance;
            var viewModel = new MainViewModel(Common.DIALOGCOORDINATOR);

            var view = new MainView
            {
                DataContext = viewModel,
            };

            view.ShowDialog();
        }
    }

}
