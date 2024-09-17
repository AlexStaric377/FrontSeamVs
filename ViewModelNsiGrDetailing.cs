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
    public partial class ViewModelNsiGrDetailing : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        private string pathcontroller = "/api/GrDetalingController/";
        public static ModelGrDetailing selectedGrDetailing;
        public static ObservableCollection<ModelGrDetailing> NsiModelGrDetailings { get; set; }
        public ModelGrDetailing SelectedModelGrDetailing
        { get { return selectedGrDetailing; } set { selectedGrDetailing = value; OnPropertyChanged("SelectedModelGrDetailing"); } }
        // конструктор класса
        public ViewModelNsiGrDetailing()
        {
           
            string jason = pathcontroller + "0/" + ViewModelNsiDetailing.selectedDetailing.keyGrDetailing + "/0";
            CallServer.PostServer(pathcontroller, jason, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            ObservableNsiModelGrDetailings(CmdStroka);
        }
        public static void ObservableNsiModelGrDetailings(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelGrDetailing>(CmdStroka);
            List<ModelGrDetailing> res = result.ViewGrDetailing.ToList();
            NsiModelGrDetailings = new ObservableCollection<ModelGrDetailing>((IEnumerable<ModelGrDetailing>)res);
        }

        // команда закрытия окна
        RelayCommand? closeModelGrDetailing;
        public RelayCommand CloseModelGrDetailing
        {
            get
            {
                return closeModelGrDetailing ??
                  (closeModelGrDetailing = new RelayCommand(obj =>
                  {
                      WinNsiGrDetailing WindowMen = MainWindow.LinkMainWindow("WinNsiGrDetailing");
                      WindowMen.Close();
                  }));
            }
        }

        // команда выбора строки харакутера жалобы
        RelayCommand? selectModelGrDetailing;
        public RelayCommand SelectModelGrDetailing
        {
            get
            {
                return selectModelGrDetailing ??
                  (selectModelGrDetailing = new RelayCommand(obj =>
                  {
                      
                      MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
                      WinCreatIntreview WindowUri = MainWindow.LinkMainWindow("WinCreatIntreview");
                      if (selectedGrDetailing != null)
                      {
                          MapOpisViewModel.selectQualification = selectedGrDetailing.nameGrDetailing;
                          MapOpisViewModel.nameFeature3 = selectedGrDetailing.kodDetailing.ToString() + ":        " + selectedGrDetailing.nameGrDetailing.ToString();
                          MapOpisViewModel.SelectContentCompleted();
                          if (selectedGrDetailing.kodGroupQualification != null)
                          { if(selectedGrDetailing.kodGroupQualification.Length>0) OpenQualification();  }

                      }
                  }));
            }
        }
        private void OpenQualification()
        {
            WinNsiQualification NewOrder = new WinNsiQualification();
            NewOrder.Left = (MainWindow.ScreenWidth / 2) + 80;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 400; //350;
            NewOrder.ShowDialog();
        }

        // команда поиска наименования характера проявления болей
        RelayCommand? searchNameGrDeliting;
        public RelayCommand SearchNameGrDeliting
        {
            get
            {
                return searchNameGrDeliting ??
                  (searchNameGrDeliting = new RelayCommand(obj =>
                  {
                      WinNsiGrDetailing WindowWinNsiGrDetailing = MainWindow.LinkMainWindow("WinNsiGrDetailing");
                      if (WindowWinNsiGrDetailing.PoiskGrDeliting.Text.Trim() != "")
                      {
                          string jason = pathcontroller + "0/0/" + WindowWinNsiGrDetailing.PoiskGrDeliting.Text;
                          CallServer.PostServer(pathcontroller, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                          else ObservableNsiModelGrDetailings(CmdStroka);
                          WindowWinNsiGrDetailing.TablDeliting.ItemsSource = NsiModelGrDetailings;
                      }
                  }));
            }
        }
    }
}
