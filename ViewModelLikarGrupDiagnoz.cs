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
    class ViewModelLikarGrupDiagnoz : BaseViewModel
    {

        WinLikarGrupDiagnoz WindowLikarGrDiag = MainWindow.LinkMainWindow("WinLikarGrupDiagnoz");
        public static string controlerLikarGrDiagnoz = "/api/LikarGrupDiagnozController/";
        public static string controlerGrDiagnoz = "/api/MedGrupDiagnozController/";
        public static string controlerIcd = "/api/IcdController/";
        private static ModelLikarGrupDiagnoz selectedLikarGrupDiagnoz;
        public static ModelMedGrupDiagnoz selectedMedGrupDiagnoz;

        public static ObservableCollection<ModelLikarGrupDiagnoz> LikarGrupDiagnozs { get; set; }
        public static ObservableCollection<ModelLikarGrupDiagnoz> AddLikarGrupDiagnozs { get; set; }
        public static ObservableCollection<ModelIcd> VeiwModelIcds { get; set; }

        public ModelLikarGrupDiagnoz SelectedLikarGrupDiagnoz
        { get { return selectedLikarGrupDiagnoz; } set { selectedLikarGrupDiagnoz = value; OnPropertyChanged("SelectedLikarGrupDiagnoz"); } }

        public static void ObservableViewLikarGrDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelLikarGrupDiagnoz>(CmdStroka);
            List<ModelLikarGrupDiagnoz> res = result.ModelLikarGrupDiagnoz.ToList();
            LikarGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>((IEnumerable<ModelLikarGrupDiagnoz>)res);


        }
        // конструктор класса
        public ViewModelLikarGrupDiagnoz()
        {
            LikarGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>();
            AddLikarGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>();
            string json = controlerLikarGrDiagnoz + MapOpisViewModel._kodDoctor+"/0";
            CallServer.PostServer(controlerLikarGrDiagnoz, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            ObservableViewLikarGrDiagnoz(CmdStroka);
 

        }

        // команда закрытия окна
        RelayCommand? closeLikarGrDiagnoz;
        public RelayCommand CloseLikarGrDiagnoz
        {
            get
            {
                return closeLikarGrDiagnoz ??
                  (closeLikarGrDiagnoz = new RelayCommand(obj =>
                  {
                      WindowLikarGrDiag.Close();
                  }));
            }
        }

        RelayCommand? selectLikarGrDiagnoz;
        public RelayCommand SelectLikarGrDiagnoz
        {
            get
            {
                return selectLikarGrDiagnoz ??
                  (selectLikarGrDiagnoz = new RelayCommand(obj =>
                  {

                      MainWindow Windowmain = MainWindow.LinkNameWindow("WindowMain");
                      if (SelectedLikarGrupDiagnoz != null)
                      {
                          Windowmain.WorkDiagnozt1.Text = selectedLikarGrupDiagnoz.icdGrDiagnoz;
                      }
                      WindowLikarGrDiag.Close();
                  }));
            }
        }

        RelayCommand? removeLikarGrDiagnoz;
        public RelayCommand RemoveLikarGrDiagnoz
        {
            get
            {
                return removeLikarGrDiagnoz ??
                  (removeLikarGrDiagnoz = new RelayCommand(obj =>
                  {
                      if (WindowLikarGrDiag.TablLikarGrupDiagnoz.SelectedIndex >= 0)
                      {
                          string json = controlerLikarGrDiagnoz + LikarGrupDiagnozs[WindowLikarGrDiag.TablLikarGrupDiagnoz.SelectedIndex].id.ToString() + "/0"; ;
                          CallServer.PostServer(controlerLikarGrDiagnoz, json, "DELETE");
                          LikarGrupDiagnozs.Remove(LikarGrupDiagnozs[WindowLikarGrDiag.TablLikarGrupDiagnoz.SelectedIndex]);
                      }
                  }));
            }
        }



        RelayCommand? addLikarGrDiagnoz;
        public RelayCommand AddLikarGrDiagnoz
        {
            get
            {
                return addLikarGrDiagnoz ??
                  (addLikarGrDiagnoz = new RelayCommand(obj =>
                  {

                      MainWindow Windowmain = MainWindow.LinkNameWindow("WindowMain");
                      Windowmain.WorkDiagnozt1.Text = "";
                      WinNsiListGrDiagnoz NewOrder = new WinNsiListGrDiagnoz();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2);
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                      if (Windowmain.WorkDiagnozt1.Text != null && Windowmain.WorkDiagnozt1.Text !="") MetodAddLikarGrDiagnoz(Windowmain.WorkDiagnozt1.Text);
                  }));
            }
        }

        public static void MetodAddLikarGrDiagnoz(string grdiagnoz = "")
        {
            selectedLikarGrupDiagnoz = new ModelLikarGrupDiagnoz();
            selectedLikarGrupDiagnoz.kodDoctor = MapOpisViewModel._kodDoctor;
            selectedLikarGrupDiagnoz.icdGrDiagnoz = grdiagnoz;
            string json = JsonConvert.SerializeObject(selectedLikarGrupDiagnoz);
            CallServer.PostServer(controlerLikarGrDiagnoz, json, "POST");
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            ModelLikarGrupDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelLikarGrupDiagnoz>(CallServer.ResponseFromServer);
            if (LikarGrupDiagnozs == null) LikarGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>();
            if (Idinsert != null)
            {
                LikarGrupDiagnozs.Add(Idinsert);
                MapOpisViewModel.EdrpouMedZaklad = MapOpisViewModel.selectedGridProfilLikar.edrpou;
                MetodAddGrupDiagnozMedZaklad(selectedLikarGrupDiagnoz.icdGrDiagnoz);
                if (MapOpisViewModel.SelectActivGrupDiagnoz == "WorkGrupDiagnoz")
                {
                    AddLikarGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>();
                    AddLikarGrupDiagnozs.Add(Idinsert);
                }


            }
        }

        public static void MetodAddGrupDiagnozMedZaklad(string diagnoz = "")
        {

            selectedMedGrupDiagnoz = new ModelMedGrupDiagnoz();
            selectedMedGrupDiagnoz.edrpou = MapOpisViewModel.EdrpouMedZaklad;
            selectedMedGrupDiagnoz.icdGrDiagnoz = diagnoz;

            string jason = controlerIcd + "0/" + diagnoz;
            CallServer.PostServer(controlerIcd, jason, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]") == false)
            {
                var result = JsonConvert.DeserializeObject<ListModelIcd>(CmdStroka);
                List<ModelIcd> res = result.ModelIcd.ToList();
                VeiwModelIcds = new ObservableCollection<ModelIcd>((IEnumerable<ModelIcd>)res);
                selectedMedGrupDiagnoz.icdKey = VeiwModelIcds[0].keyIcd;
            }
            string json = JsonConvert.SerializeObject(selectedMedGrupDiagnoz);
            CallServer.PostServer(controlerGrDiagnoz, json, "POST");
            //CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            //ModelMedGrupDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelMedGrupDiagnoz>(CallServer.ResponseFromServer);
        }

    }
}
