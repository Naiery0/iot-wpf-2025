using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WpfBookRentalShop01.Models;

namespace WpfBookRentalShop01.ModelViews
{
    public partial class BookGenreViewModel : ObservableObject
    {
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

        public BookGenreViewModel()
        {
            _isUpdate = false;
            LoadGridFromDb();
        }


        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            set => SetProperty(ref _genres, value);
        }

        private void LoadGridFromDb()
        {

            string connecetionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            string query = "SELECT division, names FROM divtbl";

            ObservableCollection<Genre> genres = new ObservableCollection<Genre>();

            using (MySqlConnection conn = new MySqlConnection(connecetionString)) { 
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

        }
        [RelayCommand]
        public void SetInit()
        {
            _isUpdate = false ;
            SelectedGenre = null;
        }

        [RelayCommand]
        public void SaveData()
        {

        }

        [RelayCommand]
        public void DelDate()
        {
            if (!_isUpdate)
            {
                MessageBox.Show("선택된 데이터가 아니면 삭제할 수 없습니다.");
                return;
            }
            string connecetionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            string query = "DELETE FROM divtbl WHERE division = @division";

            ObservableCollection<Genre> genres = new ObservableCollection<Genre>();

            using (MySqlConnection conn = new MySqlConnection(connecetionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@division", SelectedGenre.Division);

                int resultCnt = cmd.ExecuteNonQuery(); // 삭제한 건수만큼 리턴

                if (resultCnt > 0) { MessageBox.Show("삭제성공"); }
                else { MessageBox.Show("삭제실패"); }
            }
        }
        
    }
}
