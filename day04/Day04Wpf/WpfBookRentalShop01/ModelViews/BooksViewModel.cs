using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows;
using WpfBookRentalShop01.Helpers;
using WpfBookRentalShop01.Models;

namespace WpfBookRentalShop01.ModelViews
{
    

    public partial class BooksViewModel : ObservableObject
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private ObservableCollection<Book> _books;

        private Book _selectedBook;
        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                SetProperty(ref _selectedBook, value);
                _isUpdate = true; // 수정할 상태다
            }
        }

        private bool _isUpdate;

        public BooksViewModel(IDialogCoordinator coordinator)
        {
            this._dialogCoordinator = coordinator;
            InitVariable();
            LoadGridFromDb();
        }


        public ObservableCollection<Book> Books
        {
            get => _books;
            set => SetProperty(ref _books, value);
        }
        
        // 장르 콤보박스 구현을 위해 (미완)
        //private List<string> genres = new List<string>();
        //private List<string> distinctGenres = new List<string>();

        //private async void LoadGenre()
        //{
        //    string query = @"SELECT division ,names
        //                       FROM divtbl";
        //    ObservableCollection<Genre> genres = new ObservableCollection<Genre>();
        //    using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
        //    {
        //        conn.Open();
        //        MySqlCommand cmd = new MySqlCommand(query, conn);
        //        MySqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read()) { 
        //            var names = reader.GetString("names");
        //            genres.Add(names);
        //        }
        //    }
        //}

        private async void LoadGridFromDb()
        {
            try
            {
                //string connecetionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
                string query = @"SELECT idx, author, b.division as division, d.names as dnames, b.names, releaseDate, isbn, price
                                   FROM bookstbl as b, divtbl as d
                                  WHERE b.division = d.division
                               ORDER BY idx";

                ObservableCollection<Book> books = new ObservableCollection<Book>();

                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var idx = reader.GetInt32("idx");
                        var division = reader.GetString("division");
                        var names = reader.GetString("names");
                        var dnames = reader.GetString("dnames");
                        var author = reader.GetString("author");
                        var releaseDate = reader.GetDateTime("releaseDate");
                        var isbn = reader.GetString("isbn");
                        var price = reader.GetDecimal("price");

                        books.Add(new Book
                        {   
                            Idx = idx,
                            Division = division,
                            Names = names,
                            DNames = dnames,
                            Author = author,
                            ReleaseDate = releaseDate,
                            Isbn = isbn.ToString(),
                            Price = price,
                        });
                    }
                }
                Books = books;
                Common.LOGGER.Info("책 데이터 로드");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //await this._dialogCoordinator.ShowMessageAsync(this, "오류", ex.Message);
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
            SelectedBook = new Book();
            SelectedBook.Idx = 0;
            SelectedBook.Names = string.Empty;
            SelectedBook.Division = string.Empty;
            SelectedBook.DNames = string.Empty;
            SelectedBook.Author = string.Empty;
            SelectedBook.ReleaseDate = DateTime.MinValue;
            SelectedBook.Isbn = string.Empty;
            SelectedBook.Price = 0;
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
                        query = @"UPDATE bookstbl
                                     SET author = @author,
                                         division = @division,
                                         names = @names,
                                         dnames = @dnames,
                                         releaseDate = @releaseDate,
                                         isbn = @isbn,
                                         price = @price
                                   WHERE idx = @idx;";
                    }
                    else // 신규등록
                    {
                        query = @"INSERT INTO bookstbl (idx, author, division, dnames, names, releaseDate, isnc, price)
                                       VALUES (@idx, @author, @division, @dnames, @names, @releaseDate, @isbn, @price);";
                    }
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idx", SelectedBook.Idx);
                    cmd.Parameters.AddWithValue("@names", SelectedBook.Names);
                    cmd.Parameters.AddWithValue("@dnames", SelectedBook.DNames);
                    cmd.Parameters.AddWithValue("@division", SelectedBook.Division);
                    cmd.Parameters.AddWithValue("@author", SelectedBook.Author);
                    cmd.Parameters.AddWithValue("@realeseDate", SelectedBook.ReleaseDate);
                    cmd.Parameters.AddWithValue("@isbn", SelectedBook.Isbn);
                    cmd.Parameters.AddWithValue("@price", SelectedBook.Price);

                    var resultCnt = cmd.ExecuteNonQuery();
                    if (resultCnt > 0)
                    {
                        await this._dialogCoordinator.ShowMessageAsync(this, "저장", "저장성공");
                        Common.LOGGER.Info("책 저장성공");
                    }
                    else
                    {
                        await this._dialogCoordinator.ShowMessageAsync(this, "저장", "저장실패");
                        Common.LOGGER.Warn("책 저장실패");
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
            if (result == MessageDialogResult.Negative) { return; }
            try
            {
                string query = @"DELETE FROM bookstbl
                                       WHERE idx = @idx;";

                ObservableCollection<Book> books = new ObservableCollection<Book>();

                using (MySqlConnection conn = new MySqlConnection(Common.CONNSTR))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@idx", SelectedBook.Idx);

                    int resultCnt = cmd.ExecuteNonQuery(); // 삭제한 건수만큼 리턴

                    if (resultCnt > 0)
                    {
                        //MessageBox.Show("삭제성공");
                        await this._dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제성공");
                        Common.LOGGER.Info($"책 {SelectedBook.Names} 삭제 성공");
                    }
                    else
                    {
                        //MessageBox.Show("삭제실패");
                        await this._dialogCoordinator.ShowMessageAsync(this, "삭제", "삭제성공");
                        Common.LOGGER.Warn("책 삭제 실패");
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
