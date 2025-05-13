using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WpfBookRentalShop01.Helpers;
using WpfBookRentalShop01.Models;

namespace WpfBookRentalShop01.ModelViews
{
    public partial class BookGenreViewModel : ObservableObject
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private ObservableCollection<Genre> _genres;

        private Genre _selectedGenre;
        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set {
                SetProperty(ref _selectedGenre, value);
                _isUpdate = true; // 수정할 상태다
            }
        }

        private bool _isUpdate;

        public BookGenreViewModel(IDialogCoordinator coordinator)
        {
            this._dialogCoordinator = coordinator;
            InitVariable();
            LoadGridFromDb();
        }


        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            set => SetProperty(ref _genres, value);
        }

        private async void LoadGridFromDb()
        {
            try
            {
                //string connecetionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
                string query = "SELECT division, names FROM divtbl";

                ObservableCollection<Genre> genres = new ObservableCollection<Genre>();

                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var division = reader.GetString("division");
                        var names = reader.GetString("names");

                        genres.Add(new Genre
                        {
                            Division = division,
                            Names = names
                        });
                    }
                }

                Genres = genres;
                Common.LOGGER.Info("책 장르 데이터 로드");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message)
                await this._dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
                Common.LOGGER.Error(ex.Message);
            }
        }
        [RelayCommand]
        public void SetInit()
        {
            InitVariable();
            LoadGridFromDb();
        }
        private void InitVariable()
        {
            SelectedGenre = new Genre();
            SelectedGenre.Names = string.Empty;
            SelectedGenre.Division = string.Empty;
            _isUpdate = false;


        }

        [RelayCommand]
        public async void SaveData()
        {
            //Debug.WriteLine(SelectedGenre.Names);
            //Debug.WriteLine(SelectedGenre.Division);
            //Debug.WriteLine(_isUpdate);
            //string connecetionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            string query = string.Empty;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    if (_isUpdate) // 수정
                    {
                        query = "UPDATE divtbl SET names = @names WHERE division = @division";
                    }
                    else // 신규등록
                    {
                        query = "INSERT INTO divtbl VALUES (@division, names)";
                    }
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@division", SelectedGenre.Division);
                    cmd.Parameters.AddWithValue("@names", SelectedGenre.Names);

                    var resultCnt = cmd.ExecuteNonQuery();
                    if (resultCnt > 0)
                    {
                        await this._dialogCoordinator.ShowMessageAsync(this, "저장", "저장성공");
                        Common.LOGGER.Info("책장르 저장성공");
                    }
                    else
                    {
                        await this._dialogCoordinator.ShowMessageAsync(this, "저장", "저장실패");
                        Common.LOGGER.Warn("책장르 저장실패");
                    }
                }
            }
            catch (Exception ex)
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
            }
            LoadGridFromDb(); // 저장하고 DB내용 다시 그리드에 그리기
        }

        [RelayCommand]
        public async void DelDate()
        {
            if (!_isUpdate)
            {
                //MessageBox.Show("선택된 데이터가 아니면 삭제할 수 없습니다.");
                await this._dialogCoordinator.ShowMessageAsync(this, "삭제", "데이터를 선택하세요");
                return;
            }
            var result = await this._dialogCoordinator.ShowMessageAsync(this, "삭제여부", "삭제하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Negative ) { return; }
            try
            {
                string query = "DELETE FROM divtbl WHERE division = @division";

                ObservableCollection<Genre> genres = new ObservableCollection<Genre>();

                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@division", SelectedGenre.Division);

                    int resultCnt = cmd.ExecuteNonQuery(); // 삭제한 건수만큼 리턴

                    if (resultCnt > 0) {
                        //MessageBox.Show("삭제성공");
                        await this._dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제성공");
                        Common.LOGGER.Info($"책장르 {SelectedGenre.Division} 삭제 성공");
                    }
                    else { 
                        //MessageBox.Show("삭제실패");
                        await this._dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제성공");
                        Common.LOGGER.Warn("책장르 삭제 실패");
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                await this._dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
            }
            LoadGridFromDb(); // 저장하고 DB내용 다시 그리드에 그리기
        }
        
    }
}
