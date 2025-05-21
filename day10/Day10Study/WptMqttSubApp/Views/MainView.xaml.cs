using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using WpfMqttSubApp.ViewModels;

namespace WptMqttSubApp.Views
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();
            
            var vm = new MainViewModel(DialogCoordinator.Instance);
            this.DataContext = vm;
            vm.PropertyChanged += (sender, e) => { 
                if (e.PropertyName == nameof(vm.LogText))
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        LogBox.ScrollToEnd();
                    });
                }    
            };
        }
    }
}
