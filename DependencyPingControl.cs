using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel;

namespace FrontSeam
{
    // Проверка подключения сервера. Если не запущен то прекратить работу

    public class PingControl : DependencyObject
    {
        public PingControl()
        {

         
            CallServer.PostServer("/api/PingController", MainWindow.UrlServer, "PING");
            if (CallServer.ResponseFromServer.Length == 0) { Environment.Exit(0); }


        }
    }

}
