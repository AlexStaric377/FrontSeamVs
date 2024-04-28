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
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {
        // GroupQualificationViewModel модель ViewQualification
        //  клавиша в окне: "Групы квалифікації"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        MainWindow Windowmain = MainWindow.LinkNameWindow("WindowMain");
        public static bool activWorkViewDiagnoz = false, loadboolWorkDiagnoz = false, loadWorkGrupDiagnoz = false, cikl = true;
        public static string controlerLikarGrDiagnoz = "/api/LikarGrupDiagnozController/", SelectActivGrupDiagnoz = "";
        public static string pathcontrolerDependency = "/api/DependencyDiagnozController/";
        public static ModelDiagnoz selectedWorkDiagnoz;
        public static ModelInterview selectedInterview;
        public static ObservableCollection<ModelDiagnoz> ViewWorkDiagnozs { get; set; }
        public static ObservableCollection<ModelDiagnoz> TmpWorkDiagnozs = new ObservableCollection<ModelDiagnoz>();
        public static ObservableCollection<ModelDiagnoz> AllWorkDiagnozs = new ObservableCollection<ModelDiagnoz>();
        public static ObservableCollection<ModelDiagnoz> TmpDiagnozs = new ObservableCollection<ModelDiagnoz>();
        public static ObservableCollection<ModelLikarGrupDiagnoz> LikarGrupDiagnozs { get; set; }
        public ModelDiagnoz SelectedViewWorkDiagnoz
        { get { return selectedWorkDiagnoz; } set { selectedWorkDiagnoz = value; OnPropertyChanged("SelectedViewWorkDiagnoz"); } }

        public static void ObservableViewWorkDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDiagnoz>(CmdStroka);
            List<ModelDiagnoz> res = result.ModelDiagnoz.ToList();
            TmpWorkDiagnozs = new ObservableCollection<ModelDiagnoz>((IEnumerable<ModelDiagnoz>)res);
        }

        public static void ObservableViewLikarGrDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelLikarGrupDiagnoz>(CmdStroka);
            List<ModelLikarGrupDiagnoz> res = result.ModelLikarGrupDiagnoz.ToList();
            LikarGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>((IEnumerable<ModelLikarGrupDiagnoz>)res);


        }
        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника 
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        //private RelayCommand? loadViewWorkDiagnoz;
        //public RelayCommand LoadViewWorkDiagnoz
        //{
        //    get
        //    {
        //        return loadViewWorkDiagnoz ??
        //          (loadViewWorkDiagnoz = new RelayCommand(obj =>
        //          {
        //              if(_kodDoctor == "") RegSetAccountUser();
        //              if (_kodDoctor == "") return;
        //              MethodloadtablWorkDiagnoz();
        //          }));
        //    }
        //}



        // загрузка справочника диагнозов
        public void MethodViewWorkDiagnoz()
        {
            MethodloadtablWorkDiagnoz();

        }

        private void MethodloadtablWorkDiagnoz()
        {
            MainWindow Windowmain = MainWindow.LinkNameWindow("WindowMain");
            loadWorkGrupDiagnoz = false;
            AllWorkDiagnozs = new ObservableCollection<ModelDiagnoz>();
            Windowmain.WorkLoadDia.Visibility = Visibility.Hidden;
            Windowmain.WorkFoldInterv.Visibility = Visibility.Hidden;
            Windowmain.WorkCompInterviewLab.Visibility = Visibility.Hidden;
            string json = controlerLikarGrDiagnoz + _kodDoctor + "/0";
            CallServer.PostServer(controlerLikarGrDiagnoz, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ViewModelLikarGrupDiagnoz.ObservableViewLikarGrDiagnoz(CmdStroka);
            ViewWorkDiagnozs = new ObservableCollection<ModelDiagnoz>();
            foreach (ModelLikarGrupDiagnoz likarGrupDiagnoz in ViewModelLikarGrupDiagnoz.LikarGrupDiagnozs)
            {
                ModelDiagnoz likarGrupDiagnozs = new ModelDiagnoz();
                if (likarGrupDiagnoz.icdGrDiagnoz != "")
                {
                    likarGrupDiagnozs.icdGrDiagnoz = likarGrupDiagnoz.icdGrDiagnoz;
                    likarGrupDiagnozs.nameDiagnoza = likarGrupDiagnoz.icdGrDiagnoz;
                    likarGrupDiagnozs.id = likarGrupDiagnoz.id;
                    ViewWorkDiagnozs.Add(likarGrupDiagnozs);
                    ModelDiagnoz Idinsert = new ModelDiagnoz();
                    if (likarGrupDiagnoz.icdGrDiagnoz != "")
                    {
                        json = controlerViewDiagnoz + "0/" + likarGrupDiagnoz.icdGrDiagnoz;
                        CallServer.PostServer(controlerViewDiagnoz, json, "GETID");
                        CmdStroka = CallServer.ServerReturn();
                        ObservableViewWorkDiagnoz(CmdStroka);
                        foreach (ModelDiagnoz modelDiagnoz in TmpWorkDiagnozs)
                        {
                            AllWorkDiagnozs.Add(modelDiagnoz);
                        }
                    }
                }


            }
            Windowmain.WorkDiagnozTablGrid.ItemsSource = ViewWorkDiagnozs;
            loadboolWorkDiagnoz = true;
        
        }

        private void MethodaddcomWorkDiagnoz()
        {
            selectedDiagnoz = new ModelDiagnoz();
            SelectActivGrupDiagnoz = "";
            WinNsiListGrDiagnoz NewOrder = new WinNsiListGrDiagnoz();
            NewOrder.Left = (MainWindow.ScreenWidth / 2);
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
            if (Windowmain.WorkDiagnozt1.Text.Length != 0)
            {
                string AddWorkGrDiagnoz = Windowmain.WorkDiagnozt1.Text;
                if (loadWorkGrupDiagnoz == true) MethodloadtablWorkDiagnoz();

                SelectActivGrupDiagnoz = "WorkGrupDiagnoz";
                ViewModelLikarGrupDiagnoz.MetodAddLikarGrDiagnoz(AddWorkGrDiagnoz);

                foreach (ModelLikarGrupDiagnoz modelLikarGrupDiagnoz in ViewModelLikarGrupDiagnoz.AddLikarGrupDiagnozs)
                {
                    ModelDiagnoz Idinsert = new ModelDiagnoz();
                    Idinsert.icdGrDiagnoz = modelLikarGrupDiagnoz.icdGrDiagnoz;
                    Idinsert.nameDiagnoza = modelLikarGrupDiagnoz.icdGrDiagnoz;
                    Idinsert.id = modelLikarGrupDiagnoz.id;
                    ViewWorkDiagnozs.Add(Idinsert);
                    if (Idinsert.icdGrDiagnoz != "")
                    {
                        string json = controlerViewDiagnoz + "0/" + Idinsert.icdGrDiagnoz;
                        CallServer.PostServer(controlerViewDiagnoz, json, "GETID");
                        string CmdStroka = CallServer.ServerReturn();
                        ObservableViewWorkDiagnoz(CmdStroka);
                        foreach (ModelDiagnoz modelDiagnoz in TmpWorkDiagnozs)
                        {
                            AllWorkDiagnozs.Add(modelDiagnoz);
                        }
                    }

                }
                Windowmain.WorkDiagnozTablGrid.ItemsSource = ViewWorkDiagnozs;
                loadWorkGrupDiagnoz = false;
            }
            SelectActivGrupDiagnoz = "";
        }
        // команда загрузки  строки исх МКХ11 по указанному коду для вівода наименования болезни
        private RelayCommand? findNameWorkMkx;
        public RelayCommand FindNameWorkMkx
        {
            get
            {
                return findNameWorkMkx ??
                  (findNameWorkMkx = new RelayCommand(obj =>
                  { ComandFindNameWorkMkx(); }));
            }
        }

        private void ComandFindNameWorkMkx()
        {
            if (selectedWorkDiagnoz != null)
            {
                if (ViewWorkDiagnozs != null)
                {
                    if (WindowMen.WorkDiagnozTablGrid.SelectedIndex >= 0)
                    {
                        selectedWorkDiagnoz = ViewWorkDiagnozs[WindowMen.WorkDiagnozTablGrid.SelectedIndex];
                        if (loadWorkGrupDiagnoz == false)
                        {
                            SelectActivGrupDiagnoz = selectedWorkDiagnoz.icdGrDiagnoz;
                            SelectedViewWorkDiagnoz = new ModelDiagnoz();
                            WinNsiListDiagnoz NewOrder = new WinNsiListDiagnoz();
                            NewOrder.Left = (MainWindow.ScreenWidth / 2);
                            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                            NewOrder.ShowDialog();

                        }
                        else
                        {
                            Windowmain.WorkDiagnozt3.Text = "";
                            selectedInterview = new ModelInterview();
                            selectedInterview.uriInterview = selectedWorkDiagnoz.uriDiagnoza;
                            if (selectedWorkDiagnoz.keyIcd != "")
                            {

                                string json = controlerNsiIcd + selectedWorkDiagnoz.keyIcd.ToString()+"/0";
                                CallServer.PostServer(controlerNsiIcd, json, "GETID");
                                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                ModelIcd Idinsert = JsonConvert.DeserializeObject<ModelIcd>(CallServer.ResponseFromServer);
                                if (Idinsert != null)WindowMen.WorkDiagnozt3.Text = Idinsert.name;
                            }
                            Windowmain.WorkFoldInterv.Visibility = Visibility.Visible;
                            Windowmain.WorkCompInterviewLab.Visibility = Visibility.Visible;
                        }



                    }
                }


            }
        }


        // команда загрузки  строки исх МКХ11 по указанному коду для вівода наименования болезни
        private RelayCommand? selectedListWorkGrDiagnoz;
        public RelayCommand SelectedListWorkGrDiagnoz
        {
            get
            {
                return selectedListWorkGrDiagnoz ??
                  (selectedListWorkGrDiagnoz = new RelayCommand(obj =>
                  { ComandFindNameWorkGrDiagnoz(); }));
            }
        }

        private void ComandFindNameWorkGrDiagnoz()
        {
            SelectActivGrupDiagnoz = "GrupDiagnoz";
            if (_kodDoctor != "")
            {
                WinLikarGrupDiagnoz Order = new WinLikarGrupDiagnoz();
                Order.Left = (MainWindow.ScreenWidth / 2);
                Order.Top = (MainWindow.ScreenHeight / 2) - 350;
                Order.ShowDialog();
            }
            if (Windowmain.WorkDiagnozt1.Text.Length != 0)
            {
                ViewWorkDiagnozs = new ObservableCollection<ModelDiagnoz>();
                loadWorkGrupDiagnoz = true;
                foreach (ModelDiagnoz modelDiagnoz in AllWorkDiagnozs)
                { 
                    if(modelDiagnoz.icdGrDiagnoz == WindowMen.WorkDiagnozt1.Text.Trim()) ViewWorkDiagnozs.Add(modelDiagnoz);
                }
            }
            Windowmain.WorkDiagnozTablGrid.ItemsSource = ViewWorkDiagnozs;
            SelectActivGrupDiagnoz = "";
        }

        // команда выбора новой жалобы для записи новой строки 
        private RelayCommand? readColectionIntev;
        public RelayCommand ReadColectionIntev
        {
            get
            {
                return readColectionIntev ??
                  (readColectionIntev = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.IndexAddEdit = "";
                      MapOpisViewModel.ModelCall = "ModelColectionInterview";
                      string json = pathcontrolerDependency + selectedWorkDiagnoz.kodDiagnoza + "/0";
                      CallServer.PostServer(pathcontrolerDependency, json, "GETID");
                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                      ModelDependencyDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelDependencyDiagnoz>(CallServer.ResponseFromServer);
                      if (Idinsert != null)
                      { 
                        MapOpisViewModel.GetidkodProtokola = Idinsert.kodProtokola;
                        WinCreatIntreview NewOrder = new WinCreatIntreview();
                        NewOrder.Left = 600;
                        NewOrder.Top = 130;
                        NewOrder.ShowDialog();
                      } 
                      

                  }));
            }
        }
        #endregion
        #endregion

    }
}
