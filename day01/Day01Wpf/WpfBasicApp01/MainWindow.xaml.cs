using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfBasicApp01;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
    {
        LoadControlFromDb();
        LoadGridFromDb();
    }

    private async void LoadControlFromDb()
    {
        // 1. 연결문자열
        string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
        // 2. 사용쿼리
        string query = "SELECT division, names FROM divtbl";

        List<KeyValuePair<string, string>> divisions = new List<KeyValuePair<string, string>>();

        // 3. DB연결, 명령, 리더
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();  // 데이터를 가져올 때

                while (reader.Read())
                {
                    var division = reader.GetString("division");
                    var names = reader.GetString("names");

                    divisions.Add(new KeyValuePair<string, string>(division, names));

                }
            }
            catch (MySqlException ex)
            {
                await this.ShowMessageAsync($"에러 {ex.Message}", "에러");
            }
        }

        CboDivisions.ItemsSource = divisions;
    }

    private void LoadGridFromDb()
    {        
        // 1. 연결문자열
        string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
        // 2. 사용쿼리
        string query = @"SELECT b.Idx, b.Author, b.Division, b.Names, b.ReleaseDate, b.ISBN, b.Price,
                                d.Names AS dNames
                           FROM bookstbl AS b, divtbl AS d
                          WHERE b.Division = d.Division
                          ORDER by b.Idx";


        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                GrdBooks.ItemsSource = dt.DefaultView;
            }
            catch (MySqlException ex)
            {

            }
        }
    }

    private async void GrdBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (GrdBooks.SelectedItems.Count == 1)
        {
            var item = GrdBooks.SelectedItems[0] as DataRowView;
            NudIdx.Value = Convert.ToDouble(item.Row["idx"]);
            CboDivisions.SelectedValue = Convert.ToString(item.Row["Division"]);
            TxtNames.Text = Convert.ToString(item.Row["Names"]);
            TxtAuthor.Text = Convert.ToString(item.Row["Author"]);
            TxtIsbn.Text = Convert.ToString(item.Row["ISBN"]);
            TxtPrice.Text = Convert.ToString(item.Row["Price"]);
            DpcRekeaseDate.Text = Convert.ToString(item.Row["ReleaseDate"]);
        }
        await this.ShowMessageAsync($"처리완료", "메시지");
    }
}