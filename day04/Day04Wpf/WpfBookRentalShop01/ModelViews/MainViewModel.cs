using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WpfBookRentalShop01.Helpers;
using WpfBookRentalShop01.Views;

namespace WpfBookRentalShop01.ModelViews
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        private string _greeting;

        public string Greeting
        {
            get => _greeting;
            set => SetProperty(ref _greeting, value);

        }

        private string _currentStatus;
        public string CurrentStatus
        {
            get => _currentStatus;
            set => SetProperty(ref _currentStatus, value);
        }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public MainViewModel(IDialogCoordinator coordinator)
        {
            this._dialogCoordinator = coordinator; // 다이얼로그 코디네이터 초기화

            Greeting = "BookRentalShop!";

            Common.LOGGER.Info("책대여점 프로그램 실행");
        }

        #region '화면 기능(이벤트) 처리'

        [RelayCommand]
        public async Task AppExit()
        {
            var result = await this._dialogCoordinator.ShowMessageAsync(this, "종료확인", "종료하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
            if(result == MessageDialogResult.Affirmative)
            {
                Application.Current.Shutdown();
            }
            else return;
            //MessageBox.Show("종료합니다");

        }

        [RelayCommand]
        public void ShowBookGenre()
        {
            //MessageBox.Show("책장르관리");
            var vm = new BookGenreViewModel(Common.DIALOGCOORDINATOR);
            var v = new BookGenreView
            {
                DataContext = vm,
            };
            CurrentView = v;
            CurrentStatus = "책 장르 관리 화면";
            Common.LOGGER.Info("책 장르 관리 화면 실행");
        }

        [RelayCommand]
        public void ShowBooks()
        {
            //MessageBox.Show("책관리");
            var vm = new BooksViewModel(Common.DIALOGCOORDINATOR);
            var v = new BooksView
            {
                DataContext = vm,
            };
            CurrentView = v;
            CurrentStatus = "책 관리 화면";
            Common.LOGGER.Info("책 관리 화면 실행");
        }
        #endregion
        [RelayCommand]
        public void ShowMembers()
        {
            var vm = new MembersViewModel(Common.DIALOGCOORDINATOR);
            var v = new MembersView
            {
                DataContext = vm,
            };
            CurrentView = v;
            CurrentStatus = "멤버 관리 화면";
            Common.LOGGER.Info("멤버 관리 화면 실행");
        }
        [RelayCommand]
        public void ShowRentals()
        {
            var vm = new RentalsViewModel();
            var v = new RentalsView
            {
                DataContext = vm,
            };
            CurrentView = v;
            CurrentStatus = "대여 관리 화면";
            Common.LOGGER.Info("대여 관리 화면 실행");
        }

    }
}
