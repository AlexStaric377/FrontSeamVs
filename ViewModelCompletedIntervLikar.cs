﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using System.Text.Json;
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
using System.Text.RegularExpressions;
using System.Diagnostics;

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    public partial class MapOpisViewModel : BaseViewModel
    {
        // ViewModelReestrCompletedInterview модель ViewQualification
        //  клавиша в окне: "Рекомендації щодо звернення до вказаного лікаря"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>

        public  static MainWindow WindowIntevLikar = MainWindow.LinkNameWindow("WindowMain");

        public static string ColectioncontrollerIntevLikar = "/api/ColectionInterviewController/";
        public static string CompletedcontrollerIntevLikar = "/api/CompletedInterviewController/";
        public static string PacientcontrollerIntevLikar = "/api/PacientController/";
        public static string DoctorcontrollerIntevLikar = "/api/ApiControllerDoctor/";
        public static string ProtocolcontrollerIntevLikar = "/api/DependencyDiagnozController/";
        public static string DiagnozcontrollerIntevLikar = "/api/DiagnozController/";
        public static string RecomencontrollerIntevLikar = "/api/RecommendationController/";
        public static string Interviewcontroller = "/api/InterviewController/", TextContentInterv = "";

        public static ModelColectionInterview selectedIntevLikar;
        public static ColectionInterview selectedColectionIntevLikar;
        public static ModelDependency InsertIntevLikar;
        public static ModelColectionDiagnozInterview selectModelColectionDiagnozInterview;
        public static ObservableCollection<ModelColectionInterview> ColectionInterviewIntevLikars { get; set; }
        public static ObservableCollection<ModelColectionInterview> ColectionDiagnozIntevLikars { get; set; }
        public static ObservableCollection<ColectionInterview> ColectionIntevLikars { get; set; }
        public static ObservableCollection<ModelColectionDiagnozInterview> ColectionDiagnozLikars { get; set; }
        public ModelColectionInterview SelectedColectionIntevLikar
        {
            get { return selectedIntevLikar; }
            set { selectedIntevLikar = value; OnPropertyChanged("SelectedColectionIntevLikar"); }
        }

        public ModelColectionDiagnozInterview SelectModelColectionDiagnozInterview
        {
            get { return selectModelColectionDiagnozInterview; }
            set { selectModelColectionDiagnozInterview = value; OnPropertyChanged("SelectModelColectionDiagnozInterview"); }
        }
        public static void ObservablelColectionIntevLikar(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListColectionInterview>(CmdStroka);
            List<ColectionInterview> res = result.ColectionInterview.ToList();
            ColectionIntevLikars = new ObservableCollection<ColectionInterview>((IEnumerable<ColectionInterview>)res);
            BildModelColectionIntevLikar();
            WindowIntevLikar.ColectionIntevLikarTablGrid.ItemsSource = ColectionInterviewIntevLikars;
            if (ColectionDiagnozIntevLikars == null)
            {
                ColectionDiagnozIntevLikars = new ObservableCollection<ModelColectionInterview>();
                foreach (ModelColectionInterview modelColectionInterview in ColectionInterviewIntevLikars)
                {
                    ColectionDiagnozIntevLikars.Add(modelColectionInterview);
                }
            }
        }

        public static void BildModelColectionIntevLikar()
        {
            ColectionInterviewIntevLikars  = new ObservableCollection<ModelColectionInterview>();
            foreach (ColectionInterview colectionInterview in ColectionIntevLikars)
            {
                selectedIntevLikar = new ModelColectionInterview();
                if (colectionInterview.kodPacient != null && colectionInterview.kodPacient.Length != 0) MethodPacientIntevLikars(colectionInterview, false);
                //else { WindowIntevLikar.LikarIntert3.Text = "Гість"; selectedIntevLikar.namePacient = "Гість"; }
                if (colectionInterview.kodDoctor != null && colectionInterview.kodDoctor.Length != 0) MethodDoctorIntevLikars(colectionInterview, false);
                if (colectionInterview.kodProtokola != null && colectionInterview.kodProtokola.Length != 0) MethodProtokolaIntevLikars(colectionInterview, false);

                selectedIntevLikar.id = colectionInterview.id;
                selectedIntevLikar.kodComplInterv = colectionInterview.kodComplInterv;
                selectedIntevLikar.kodProtokola = colectionInterview.kodProtokola;
                selectedIntevLikar.dateInterview = colectionInterview.dateInterview;
                selectedIntevLikar.resultDiagnoz = colectionInterview.resultDiagnoz;
                ColectionInterviewIntevLikars.Add(selectedIntevLikar);
            }

            int indexDiagnoz = 0;
            ColectionDiagnozLikars = new ObservableCollection<ModelColectionDiagnozInterview>();
            foreach (ModelColectionInterview modelColectionInterview in ColectionInterviewIntevLikars.OrderBy(x => x.kodProtokola))
            {
                selectModelColectionDiagnozInterview = new ModelColectionDiagnozInterview();
                selectModelColectionDiagnozInterview.kodDoctor = modelColectionInterview.kodDoctor;
                selectModelColectionDiagnozInterview.kodPacient = modelColectionInterview.kodPacient;
                selectModelColectionDiagnozInterview.kodProtokola = modelColectionInterview.kodProtokola;
                selectModelColectionDiagnozInterview.nameDiagnoz = modelColectionInterview.nameDiagnoz;
                
                

                if (ColectionDiagnozLikars.Count == 0) ColectionDiagnozLikars.Add(selectModelColectionDiagnozInterview);
                if (ColectionDiagnozLikars[indexDiagnoz].kodProtokola == modelColectionInterview.kodProtokola) ColectionDiagnozLikars[indexDiagnoz].quanntityDiagnoz++;
                else
                {
                    selectModelColectionDiagnozInterview.quanntityDiagnoz++;
                    ColectionDiagnozLikars.Add(selectModelColectionDiagnozInterview);
                    indexDiagnoz++;
                }
            }
            WindowMen.ColectionDiagnozTablGrid.ItemsSource = ColectionDiagnozLikars;
        }

        public static void MethodPacientIntevLikars(ColectionInterview colectionInterview, bool boolname)
        {

            
            string json = PacientcontrollerIntevLikar + colectionInterview.kodPacient.ToString()+ "/0/0/0/0";
            CallServer.PostServer(PacientcontrollerIntevLikar, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                selectedIntevLikar.namePacient = Idinsert.name + " " + Idinsert.surname + " " + Idinsert.profession + " " + Idinsert.tel;
                selectedIntevLikar.kodPacient = Idinsert.kodPacient;
                WindowIntevLikar.LikarInterviewAvtort7.Text = selectedIntevLikar.namePacient;
                if (boolname == true) namePacient = selectedIntevLikar.namePacient;
            }
        }

        public static void MethodDoctorIntevLikars(ColectionInterview colectionInterview, bool boolname)
        {

           
            var json = DoctorcontrollerIntevLikar + colectionInterview.kodDoctor.ToString() + "/0/0";
            CallServer.PostServer(DoctorcontrollerIntevLikar, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDoctor Insert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                selectedIntevLikar.nameDoctor = Insert.name+" "+ Insert.surname + " " + Insert.specialnoct + " " + Insert.telefon;
                selectedIntevLikar.kodDoctor = Insert.kodDoctor;
                WindowIntevLikar.LikarInterviewAvtort7.Text = selectedIntevLikar.nameDoctor;
                //selectedIntevLikar.namePacient = "";
                if (boolname == true) nameDoctor = selectedIntevLikar.nameDoctor;
                //WindowIntevLikar.LikarIntert3.Text = "";
            }

        }

        public static void MethodProtokolaIntevLikars(ColectionInterview colectionInterview, bool boolname)
        {
           

            var json = ProtocolcontrollerIntevLikar + "0/" + colectionInterview.kodProtokola.ToString() + "/0";
            CallServer.PostServer(ProtocolcontrollerIntevLikar, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDependency Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                if (Insert != null)
                {
                   
                    json = DiagnozcontrollerIntevLikar + Insert.kodDiagnoz.ToString()+"/0/0";
                    CallServer.PostServer(DiagnozcontrollerIntevLikar, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        selectedIntevLikar.nameDiagnoz = Insert1.nameDiagnoza;
                        selectedIntevLikar.nameInterview = Insert1.uriDiagnoza;
                        if (boolname == true) WindowIntevLikar.LikarInterviewt6.Text = Insert1.nameDiagnoza;
                    }

                    
                    json = RecomencontrollerIntevLikar + Insert.kodRecommend.ToString() + "/0";
                    CallServer.PostServer(RecomencontrollerIntevLikar, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelRecommendation Insert2 = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                        selectedIntevLikar.nameRecomen = Insert2.contentRecommendation;
                        if (boolname == true) WindowIntevLikar.LikarInterviewt5.Text = Insert2.contentRecommendation;
                    }
                }

            }
        }


        #region Команды вставки, удаления и редектирования 
        /// <summary>
        /// Команды вставки, удаления и редектирования 
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        public static void MethodLoadtableColectionIntevLikar()
        {

            if (_kodDoctor == "") { WarningMessageOfProfilLikar(); return; }
   
            IndexAddEdit = "";
            WindowMen.ColectionDiagnozTablGrid.Visibility = Visibility.Hidden;
            CallServer.PostServer(ColectioncontrollerIntevLikar, ColectioncontrollerIntevLikar+ "0/" + _kodDoctor + "/0", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservablelColectionIntevLikar(CmdStroka);
        }


        private void BoolTrueIntevLikarCompl()
        {
            //addtboolInterview = true;
            editboolIntevLikar = true;
            WindowIntevLikar.LikarFoldInterv.Visibility = Visibility.Visible;
            WindowIntevLikar.LikarFolderRecomen.Visibility = Visibility.Visible;
            WindowIntevLikar.LikarFolderDiagn.Visibility = Visibility.Visible;
            WindowIntevLikar.LikarInterviewt7.IsEnabled = true;
            WindowIntevLikar.LikarInterviewt7.Background = Brushes.AntiqueWhite;

        }

        private void BoolFalseIntevLikarCompl()
        {
            //addtboolInterview = false;
            editboolIntevLikar = false;
            WindowIntevLikar.LikarFoldInterv.Visibility = Visibility.Hidden;
            WindowIntevLikar.LikarFolderRecomen.Visibility = Visibility.Hidden;
            WindowIntevLikar.LikarFolderDiagn.Visibility = Visibility.Hidden;
            WindowIntevLikar.LikarInterviewt7.IsEnabled = false;
            WindowIntevLikar.LikarInterviewt7.Background = Brushes.White;
            WindowIntevLikar.LikarInterviewAvtort7.Text = "";
            WindowIntevLikar.LikarFoldUrlHtpps.Visibility = Visibility.Hidden;
            WindowIntevLikar.LikarFoldMixUrlHtpps.Visibility = Visibility.Hidden;
            IndexAddEdit = "";
        }
        // команда удаления
        public void MethodRemoveColectionIntevLikar()
        { 
                      if (selectedIntevLikar != null)
                      {
                          string json = CompletedcontrollerIntevLikar + selectedIntevLikar.kodComplInterv.ToString() + "/0";
                          CallServer.PostServer(CompletedcontrollerIntevLikar, json, "DELETE");
                        
                          json = ColectioncontrollerIntevLikar + selectedIntevLikar.id.ToString()+"/0/0";
                          CallServer.PostServer(ColectioncontrollerIntevLikar, json, "DELETE");
                          ColectionInterviewIntevLikars.Remove(selectedIntevLikar);
                          selectedIntevLikar = new ModelColectionInterview();
                      }
                      BoolFalseIntevLikarCompl();
                      WindowIntevLikar.ColectionIntevLikarTablGrid.SelectedItem = null;       
        }

        // команда  редактировать
        public void MethodEditCompletedInterviewLikar()
        {
                   if (WindowIntevLikar.ColectionIntevLikarTablGrid.SelectedIndex >= 0)
                      {
                          IndexAddEdit = "editCommand";
                          if (editboolIntevLikar == false && selectedIntevLikar != null)
                          {
                              selectedColectionIntevLikar = new ColectionInterview();
                              selectedColectionIntevLikar = ColectionIntevLikars[WindowIntevLikar.ColectionIntevLikarTablGrid.SelectedIndex];
                              BoolTrueIntevLikarCompl();
                          }
                          else
                          {
                              BoolFalseIntevLikarCompl();
                              
                              GetidkodProtokola = "";
                              WindowIntevLikar.ColectionIntevLikarTablGrid.SelectedItem = null;
                          }
                      }

        }

        // команда сохранить редактирование
         public void MethodSaveColectionIntevLikar()
        { 
                       ModelDependency Insert = new ModelDependency();
                      string json = "";
                      BoolFalseIntevLikarCompl();
                      // ОБращение к серверу измнить корректируемую запись в БД
                      if (selectedIntevLikar != null)
                      {
                          if (selectedIntevLikar.kodProtokola != "")
                          {

                            json = ProtocolcontrollerIntevLikar + "0/" + selectedIntevLikar.kodProtokola + "/0";
                            CallServer.PostServer(ProtocolcontrollerIntevLikar, json, "GETID");
                            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                            
                          }
                          if (Insert == null)
                          { 
                              
                              json = Interviewcontroller + "0/0/0";
                              CallServer.PostServer(Interviewcontroller, json, "GETID");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                              int Numbkey = Convert.ToInt32(Insert.kodProtokola.Substring(Insert.kodProtokola.IndexOf(".") + 1, Insert.kodProtokola.Length - (Insert.kodProtokola.IndexOf(".") + 1)));
                              Numbkey++;
                              string _repl = "000000000";
                             _repl = _repl.Length - Numbkey.ToString().Length > 0 ? _repl.Substring(0, _repl.Length - Numbkey.ToString().Length) : "";
                              Insert.kodProtokola = Insert.kodProtokola.Substring(0,Insert.kodProtokola.IndexOf(".")+1) + _repl + Numbkey.ToString();
                          } 

                          
                          if (WindowIntevLikar.LikarInterviewt5.Text.ToString().Length > 0) Insert.kodRecommend = WindowIntevLikar.LikarInterviewt5.Text.ToString().Substring(0, WindowIntevLikar.LikarInterviewt5.Text.ToString().IndexOf(":"));
                          if (WindowIntevLikar.LikarInterviewt6.Text.ToString().Length > 0) Insert.kodDiagnoz = WindowIntevLikar.LikarInterviewt6.Text.ToString().Substring(0, WindowIntevLikar.LikarInterviewt6.Text.ToString().IndexOf(":"));
                          string Method = selectedIntevLikar.kodProtokola == "" ? "POST" : "PUT";
                          Insert.id = selectedIntevLikar.kodProtokola == "" ? 0 : Insert.id;
                         
                          var jsonDepency = JsonConvert.SerializeObject(Insert);
                          CallServer.PostServer(ProtocolcontrollerIntevLikar, jsonDepency, Method);
                          if (selectedColectionIntevLikar == null) selectedColectionIntevLikar = new ColectionInterview();
                          selectedColectionIntevLikar.kodDoctor = selectedIntevLikar.kodDoctor; 
                         
                          json = JsonConvert.SerializeObject(selectedColectionIntevLikar);
                          CallServer.PostServer(ColectioncontrollerIntevLikar, json, "PUT");
                      }
                      WindowIntevLikar.ColectionIntevLikarTablGrid.SelectedItem = null;       
        }
 
        // команда печати
        public void MethodPrintColectionIntevLikar()
        { 
                      if (ColectionInterviewIntevLikars != null)
                      {
                          MessageBox.Show("Результат діагнозу :" + ColectionInterviewIntevLikars[0].resultDiagnoz.ToString());
                      }        
        }

        // команда 
        private RelayCommand? readColectionIntevLikars;
        public RelayCommand ReadColectionIntevLikars
        {
            get
            {
                return readColectionIntevLikars ??
                  (readColectionIntevLikars = new RelayCommand(obj =>
                  {
                      IndexAddEdit = "editCommand";
                      ModelCall = "ModelColectionInterview";
                      GetidkodProtokola = selectedIntevLikar.kodComplInterv + "/0";
                      ComandreadColectionIntevLikars();
                      //BoolFalseIntevLikarCompl();
                      editboolIntevLikar = false;
                  }));
            }
        }


        private void ComandreadColectionIntevLikars()
        {

            WinCreatIntreview NewOrder = new WinCreatIntreview();
            NewOrder.Left = (MainWindow.ScreenWidth / 2) - 100;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();


        }

        private RelayCommand? listIntevLikars;
        public RelayCommand ListIntevLikars
        {
            get
            {
                return listIntevLikars ??
                  (listIntevLikars = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.CallViewProfilLikar = "WinNsiLikar";
                      WinNsiLikar NewOrder = new WinNsiLikar();
                      //NewOrder.Left = 450;
                      //NewOrder.Top = 320;
                      NewOrder.ShowDialog();
                      MapOpisViewModel.CallViewProfilLikar = "";
                      if (selectedIntevLikar != null && WindowMain.LikarIntert2.Text.ToString().Trim() != "")
                      { 
                          selectedIntevLikar.kodDoctor = WindowMain.LikarIntert2.Text.Substring(0, WindowMain.LikarIntert2.Text.IndexOf(":"));
                          selectedIntevLikar.nameDoctor = WindowMain.LikarIntert2.Text.Substring(WindowMain.LikarIntert2.Text.IndexOf(":"), WindowMain.LikarIntert2.Text.Length - WindowMain.LikarIntert2.Text.IndexOf(":"));

                      }
                  }));
            }
        }

        private RelayCommand? listPacientIntevLikars;
        public RelayCommand ListPacientIntevLikars
        {
            get
            {
                return listPacientIntevLikars ??
                  (listPacientIntevLikars = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.CallViewProfilLikar = "WinNsiPacient";
                      WinNsiPacient NewOrder = new WinNsiPacient();
                      //NewOrder.Left = 450;
                      //NewOrder.Top = 320;
                      NewOrder.ShowDialog();
                      MapOpisViewModel.CallViewProfilLikar = "";
                      if (selectedIntevLikar != null && WindowMain.LikarIntert3.Text.ToString().Trim() != "")
                      { 
                        selectedIntevLikar.kodPacient = WindowMain.LikarIntert3.Text.Substring(0, WindowMain.LikarIntert3.Text.IndexOf(":"));
                        selectedIntevLikar.namePacient = WindowMain.LikarIntert3.Text.Substring(WindowMain.LikarIntert3.Text.IndexOf(":"), WindowMain.LikarIntert3.Text.Length-WindowMain.LikarIntert3.Text.IndexOf(":"));
                      } 

                  }));
            }
        }

        private RelayCommand? listRecomendaciyaIntevLikars;
        public RelayCommand ListRecomendaciyaIntevLikars
        {
            get
            {
                return listRecomendaciyaIntevLikars ??
                  (listRecomendaciyaIntevLikars = new RelayCommand(obj =>
                  {
                      WinNsiListRecommen NewOrder = new WinNsiListRecommen();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2) - 100;
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                  }));
            }
        }

        private RelayCommand? listDiagnozIntevLikars;
        public RelayCommand ListDiagnozIntevLikars
        {
            get
            {
                return listDiagnozIntevLikars ??
                  (listDiagnozIntevLikars = new RelayCommand(obj =>
                  {
                      WinNsiListDiagnoz NewOrder = new WinNsiListDiagnoz();
                      NewOrder.Left = (MainWindow.ScreenWidth / 2) - 100;
                      NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                      NewOrder.ShowDialog();
                  }));
            }
        }

        private RelayCommand? onVisibleObjIntevLikars;
        public RelayCommand OnVisibleObjIntevLikars
        {
            get
            {
                return onVisibleObjIntevLikars ??
                  (onVisibleObjIntevLikars = new RelayCommand(obj =>
                  {
                      if (IndexAddEdit == "")
                      {
                          if (WindowIntevLikar.ColectionIntevLikarTablGrid.SelectedIndex == -1) return;
                          if (ColectionInterviewIntevLikars != null)
                          {
                              MainWindow WindowIntevLikar = MainWindow.LinkNameWindow("WindowMain");

                              if (editboolIntevLikar == true) BoolFalseIntevLikarCompl();
                              if (ColectionInterviewIntevLikars.Count != 0)
                              { 
                                  WindowIntevLikar.LikarFoldInterv.Visibility = Visibility.Visible;
                                  ColectionInterview selectedColection = ColectionIntevLikars[WindowIntevLikar.ColectionIntevLikarTablGrid.SelectedIndex];
                                  WindowIntevLikar.LikarFoldUrlHtpps.Visibility = Visibility.Visible;
                                  WindowIntevLikar.LikarFoldMixUrlHtpps.Visibility = Visibility.Visible;
                                  int Index = 0;
                                  TextContentInterv = "https://www.google.com/search?q=";
                                  GetidkodProtokola = selectedColection.kodComplInterv + "/0";
                                  CallServer.PostServer(ViewModelCreatInterview.Completedcontroller, ViewModelCreatInterview.Completedcontroller + MapOpisViewModel.GetidkodProtokola, "GETID");
                                  string CmdStroka = CallServer.ServerReturn();
                                  if (CmdStroka.Contains("[]") == false) ViewModelCreatInterview.ObservableContentInterv(CmdStroka);
                                  foreach (ModelContentInterv modelContentInterv in ViewModelCreatInterview.ContentIntervs)
                                  {
                                      if (Index > 0) TextContentInterv += modelContentInterv.detailsInterview.Replace(" ", "+");
                                      Index++;
                                      if (Index > 2) break;
                                  }
                                  WindowIntevLikar.LikarInterviewt7.Text = selectedColection.resultDiagnoz = TextContentInterv;
                              }
 
                          }                    
                      }

                  }));
            }
        }

        private RelayCommand? selectedListWorkDiagnoz;
        public RelayCommand SelectedListWorkDiagnoz
        {
            get
            {
                return selectedListWorkDiagnoz ??
                (selectedListWorkDiagnoz = new RelayCommand(obj =>
                {
                    if (WindowIntevLikar.ColectionDiagnozTablGrid.Visibility == Visibility.Hidden) WindowIntevLikar.ColectionDiagnozTablGrid.Visibility = Visibility.Visible;
                    else WindowIntevLikar.ColectionDiagnozTablGrid.Visibility = Visibility.Hidden;
 
                }));
            }
        }


        private RelayCommand? onVisibleObjDiagnozLikars;
        public RelayCommand OnVisibleObjDiagnozLikars
        {
            get
            {
                return onVisibleObjDiagnozLikars ??
                  (onVisibleObjDiagnozLikars = new RelayCommand(obj =>
                  {

                      if (ColectionDiagnozLikars != null)
                      {
                          if (ColectionDiagnozLikars.Count != 0 && WindowIntevLikar.ColectionDiagnozTablGrid.SelectedIndex >= 0)
                          {

                              selectModelColectionDiagnozInterview = ColectionDiagnozLikars[WindowIntevLikar.ColectionDiagnozTablGrid.SelectedIndex];
                              ColectionInterviewIntevLikars = new ObservableCollection<ModelColectionInterview>();
                              foreach (ModelColectionInterview colectionInterview in ColectionDiagnozIntevLikars)
                              {
                                  if (selectModelColectionDiagnozInterview.kodProtokola == colectionInterview.kodProtokola) ColectionInterviewIntevLikars.Add(colectionInterview);
                              }
                              WindowIntevLikar.ColectionIntevLikarTablGrid.ItemsSource = ColectionInterviewIntevLikars;
                              WindowIntevLikar.ColectionDiagnozTablGrid.Visibility = Visibility.Hidden;
  
                          }
                      }

                  }));
            }
        }

        

        // команда просмотра содержимого интервью
        private RelayCommand? readOpisIntevUrl;
        public RelayCommand ReadOpisIntevUrl
        {
            get
            {
                return readOpisIntevUrl ??
                  (readOpisIntevUrl = new RelayCommand(obj =>
                  {
                      string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                      string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                      string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                      Process Rungoogle = new Process();
                      Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                      Rungoogle.StartInfo.Arguments = selectedIntevLikar.nameInterview;
                      //Rungoogle.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                      Rungoogle.StartInfo.UseShellExecute = false;
                      Rungoogle.EnableRaisingEvents = true;
                      Rungoogle.Start();
                  }));
            }
        }

        // команда просмотра содержимого интервью
        private RelayCommand? readOpisMixUrlHtpps;
        public RelayCommand ReadOpisMixUrlHtpps
        {
            get
            {
                return readOpisMixUrlHtpps ??
                  (readOpisMixUrlHtpps = new RelayCommand(obj =>
                  {

                      string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                      string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                      string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                      Process Rungoogle = new Process();
                      Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                      Rungoogle.StartInfo.Arguments = TextContentInterv.Trim();
                      //Rungoogle.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                      Rungoogle.StartInfo.UseShellExecute = false;
                      Rungoogle.EnableRaisingEvents = true;
                      Rungoogle.Start();
                  }));
            }
        }

 

        #endregion
        #endregion
    }
}


