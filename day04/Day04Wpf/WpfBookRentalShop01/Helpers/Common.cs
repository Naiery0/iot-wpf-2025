using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WpfBookRentalShop01.Helpers
{
    /// <summary>
    /// 프로젝트 공통 클래스
    /// </summary>
    public class Common
    {
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        public static readonly string CONNSTR = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";


        // MahApps.Metro 형태 다이얼로그 코디네이터
        public static IDialogCoordinator DIALOGCOORDINATOR;
    }
}
