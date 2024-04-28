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

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{

    public class NsiViewModelDiagnoz : INotifyPropertyChanged
    {

        WinNsiListDiagnoz WindowNsiListUri = MainWindow.LinkMainWindow("WinNsiListDiagnoz");
        public static string controlerNsiDiagnoz = "/api/DiagnozController/";
        private ModelDiagnoz selectedVeiwDiagnoz;
        public static ObservableCollection<ModelDiagnoz> VeiwDiagnozs { get; set; }

        public ModelDiagnoz SelectedVeiwDiagnoz
        { get { return selectedVeiwDiagnoz; } set { selectedVeiwDiagnoz = value; OnPropertyChanged("SelectedVeiwDiagnoz"); } }
        // конструктор класса
        public NsiViewModelDiagnoz()
        {

            if (MapOpisViewModel.SelectActivGrupDiagnoz == "")
            {
                CallServer.PostServer(controlerNsiDiagnoz, controlerNsiDiagnoz, "GET");
                string CmdStroka = CallServer.ServerReturn();
                ObservableViewNsiDiagnoz(CmdStroka);
            }
            else
            {
                if (MapOpisViewModel.AllWorkDiagnozs.Count > 0)
                {
                    VeiwDiagnozs = new ObservableCollection<ModelDiagnoz>();
                    foreach (ModelDiagnoz modelDiagnoz in MapOpisViewModel.AllWorkDiagnozs)
                    {
                        if (modelDiagnoz.icdGrDiagnoz == MapOpisViewModel.SelectActivGrupDiagnoz) VeiwDiagnozs.Add(modelDiagnoz);
 
                    }

                }
                else
                {
                    string json = controlerNsiDiagnoz + "0/" + MapOpisViewModel.SelectActivGrupDiagnoz;
                    CallServer.PostServer(controlerNsiDiagnoz, json, "GETID");
                    string CmdStroka = CallServer.ServerReturn();
                    ObservableViewNsiDiagnoz(CmdStroka);
                }
            }

 

        }

        public static void ObservableViewNsiDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDiagnoz>(CmdStroka);
            List<ModelDiagnoz> res = result.ModelDiagnoz.ToList();
            VeiwDiagnozs = new ObservableCollection<ModelDiagnoz>((IEnumerable<ModelDiagnoz>)res);

        }

        // команда закрытия окна
        RelayCommand? closeVeiwDiagnoz;
        public RelayCommand CloseVeiwDiagnoz
        {
            get
            {
                return closeVeiwDiagnoz ??
                  (closeVeiwDiagnoz = new RelayCommand(obj =>
                  {
                      WindowNsiListUri.Close();
                  }));
            }
        }

        RelayCommand? selectVeiwDiagnoz;
        public RelayCommand SelectVeiwDiagnoz
        {
            get
            {
                return selectVeiwDiagnoz ??
                  (selectVeiwDiagnoz = new RelayCommand(obj =>
                  {
                      
                      MainWindow Windowmain = MainWindow.LinkNameWindow("WindowMain");
                      if (SelectedVeiwDiagnoz != null)
                      {
                          Windowmain.LikarInterviewt6.Text = SelectedVeiwDiagnoz.kodDiagnoza.ToString() + ": " + SelectedVeiwDiagnoz.nameDiagnoza;
                      }
                      WindowNsiListUri.Close();
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
