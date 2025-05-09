using MahApps.Metro.Controls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfBasicApp02.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainView : MetroWindow
{
    public MainView()
    {
        InitializeComponent();
    }

    
    private void GrdBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        //    if (GrdBooks.SelectedItems.Count == 1)
        //    {
        //        var item = GrdBooks.SelectedItems[0] as DataRowView;
        //        NudIdx.Value = Convert.ToDouble(item.Row["idx"]);
        //        CboDivisions.SelectedValue = Convert.ToString(item.Row["Division"]);
        //        TxtNames.Text = Convert.ToString(item.Row["Names"]);
        //        TxtAuthor.Text = Convert.ToString(item.Row["Author"]);
        //        TxtIsbn.Text = Convert.ToString(item.Row["ISBN"]);
        //        TxtPrice.Text = Convert.ToString(item.Row["Price"]);
        //        DpcRekeaseDate.Text = Convert.ToString(item.Row["ReleaseDate"]);
        //    }
    }
}