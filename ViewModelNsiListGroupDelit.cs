using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Windows.Media;
using System.Windows.Controls;

namespace FrontSeam
{
    public partial class ViewModelNsiListGroupDelit : INotifyPropertyChanged
    {
        string controlerListGrDetailing = "/api/ControllerListGroupDetail/";
        private ModelListGrDetailing selectedListGrDetailing;
        public  static ObservableCollection<ModelListGrDetailing> ViewListGrDetailings { get; set; }

        public ModelListGrDetailing SelectedListGrDetailing
        { get { return selectedListGrDetailing; } set { selectedListGrDetailing = value; OnPropertyChanged("SelectedListGrDetailing"); } }
        // конструктор класса
        public ViewModelNsiListGroupDelit()
        {

            CallServer.PostServer(controlerListGrDetailing, controlerListGrDetailing, "GET");
            string CmdStroka = CallServer.ServerReturn();
            ObservableListGrDetailing(CmdStroka);
        }

        public static void ObservableListGrDetailing(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelListGrDetailing>(CmdStroka);
            List<ModelListGrDetailing> res = result.ViewListGrDetailing.ToList();
            ViewListGrDetailings = new ObservableCollection<ModelListGrDetailing>((IEnumerable<ModelListGrDetailing>)res);
        }

        // команда закрытия окна
        RelayCommand? closeListGrDetailing;
        public RelayCommand CloseListGrDetailing
        {
            get
            {
                return closeListGrDetailing ??
                  (closeListGrDetailing = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
                      WinNsiListGroupDelit WindowMen = MainWindow.LinkMainWindow("WinNsiListGroupDelit");
                      if (SelectedListGrDetailing != null)
                      {
                          //WindowMain.GrDetailingst2.Text = SelectedListGrDetailing.keyGrDetailing.ToString()+": "+ SelectedListGrDetailing.nameGrup.ToString();
                          //WindowMain.Detailingt4.Text = SelectedListGrDetailing.keyGrDetailing.ToString();
                          //WindowMain.Detailingt2.Text = SelectedListGrDetailing.nameGrup.ToString();

                      }
                      WindowMen.Close();
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
}
