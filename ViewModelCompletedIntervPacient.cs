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

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {
      
       

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        public static string namePacient = "", nameDoctor = "", nameFeature3 = "", nameDetailing="", nameGrDetailing="";
        public static string CallViewDetailing = "";
        private bool editboolIntevLikar = false, loadboolIntevLikar = false;
        public static string ModelCallIntevLikar = "", ModelCall = "";

        private static MainWindow WindowIntevPacient = MainWindow.LinkNameWindow("WindowMain");
        private bool editboolIntevPacient = false, loadboolIntevPacient = false;
        public static string ModelCallIntevPacient = "";
        public static string ColectioncontrollerIntevPacient = "/api/ColectionInterviewController/";
        public static string CompletedcontrollerIntevPacient = "/api/CompletedInterviewController/";
        public static string PacientcontrollerIntev = "/api/PacientController/";
        public static string DoctorcontrollerIntev = "/api/ApiControllerDoctor/";
        public static string ProtocolcontrollerDependency = "/api/DependencyDiagnozController/";
        public static string DiagnozcontrollerIntev = "/api/DiagnozController/";
        public static string RecomencontrollerIntev = "/api/RecommendationController/";

        public static ModelColectionInterview selectedIntevPacient;
        public static ColectionInterview selectedColectionIntevPacient;
        public static ModelDependency InsertIntevPacient;
        public static ObservableCollection<ModelColectionInterview> ColectionInterviewIntevPacients { get; set; }
        public static ObservableCollection<ColectionInterview> ColectionIntevPacients { get; set; }
        public ModelColectionInterview SelectedColectionIntevPacient
        {
            get { return selectedIntevPacient; }
            set { selectedIntevPacient = value; OnPropertyChanged("SelectedColectionIntevPacient"); }
        }


        public static void ObservablelColectionIntevPacient(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListColectionInterview>(CmdStroka);
            List<ColectionInterview> res = result.ColectionInterview.ToList();
            ColectionIntevPacients = new ObservableCollection<ColectionInterview>((IEnumerable<ColectionInterview>)res);
            BildModelColectionIntevPacient();
            WindowIntevPacient.ColectionIntevPacientTablGrid.ItemsSource = ColectionInterviewIntevPacients;
        }

        private static void BildModelColectionIntevPacient()
        {
            ColectionInterviewIntevPacients = new ObservableCollection<ModelColectionInterview>();
            foreach (ColectionInterview colectionInterview in ColectionIntevPacients)
            {
                selectedIntevPacient = new ModelColectionInterview();
                if (colectionInterview.kodPacient != null && colectionInterview.kodPacient.Length != 0) MethodPacientIntevPacient(colectionInterview, false);
                else { WindowIntevPacient.PacientIntert3.Text = "Гість"; selectedIntevPacient.namePacient = "Гість"; }
                if (colectionInterview.kodDoctor != null && colectionInterview.kodDoctor.Length != 0) MethodDoctorIntevPacient(colectionInterview, false);
                if (colectionInterview.kodProtokola != null && colectionInterview.kodProtokola.Length != 0) MethodProtokolaIntevPacient(colectionInterview, false);

                selectedIntevPacient.id = colectionInterview.id;
                selectedIntevPacient.kodComplInterv = colectionInterview.kodComplInterv;
                selectedIntevPacient.kodProtokola = colectionInterview.kodProtokola;
                selectedIntevPacient.dateInterview = colectionInterview.dateInterview;
                selectedIntevPacient.resultDiagnoz = colectionInterview.resultDiagnoz;
                selectedIntevPacient.nameInterview = colectionInterview.nameInterview;
                selectedIntevPacient.dateDoctor = colectionInterview.dateDoctor;
                ColectionInterviewIntevPacients.Add(selectedIntevPacient);
            }
        }

        private static void MethodPacientIntevPacient(ColectionInterview colectionInterview, bool boolname)
        {

            string json = PacientcontrollerIntev + colectionInterview.kodPacient.ToString()+ "/0/0/0/0";
            CallServer.PostServer(PacientcontrollerIntev, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                selectedIntevPacient.namePacient = Idinsert.name + " " + Idinsert.surname + " " + Idinsert.profession + " " + Idinsert.tel;
                selectedIntevPacient.kodPacient = Idinsert.kodPacient;
                if (boolname == true) WindowIntevPacient.PacientIntert3.Text = selectedIntevPacient.namePacient;
            }
        }

        private static void MethodDoctorIntevPacient(ColectionInterview colectionInterview, bool boolname)
        {

            var json = DoctorcontrollerIntev + colectionInterview.kodDoctor.ToString() + "/0/0";
            CallServer.PostServer(DoctorcontrollerIntev, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDoctor Insert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                selectedIntevPacient.nameDoctor = Insert.name + " " + Insert.surname + " " + Insert.specialnoct + " " + Insert.telefon;
                selectedIntevPacient.kodDoctor = Insert.kodDoctor;
                if (boolname == true) WindowIntevPacient.PacientIntert2.Text = selectedIntevPacient.nameDoctor;
            }

        }

        private static void MethodProtokolaIntevPacient(ColectionInterview colectionInterview, bool boolname)
        {

            var json = ProtocolcontrollerDependency + "0/" + colectionInterview.kodProtokola.ToString() + "/0/0";
            CallServer.PostServer(ProtocolcontrollerDependency, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDependency Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                if (Insert != null)
                {
                    json = DiagnozcontrollerIntev + Insert.kodDiagnoz.ToString() + "/0/0";
                    CallServer.PostServer(DiagnozcontrollerIntev, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        selectedIntevPacient.nameDiagnoz = Insert1.nameDiagnoza;
                        if (boolname == true) WindowIntevPacient.PacientInterviewt6.Text = Insert1.nameDiagnoza;
                    }

                    json = RecomencontrollerIntev + Insert.kodRecommend.ToString() + "/0/0";
                    CallServer.PostServer(RecomencontrollerIntev, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelRecommendation Insert2 = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                        selectedIntevPacient.nameRecomen = Insert2.contentRecommendation;
                        if (boolname == true) WindowIntevPacient.PacientInterviewt5.Text = Insert2.contentRecommendation;
                    }
                }

            }
        }


        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

        // загрузка справочника по нажатию клавиши Завантажити
        public static void MethodLoadtableColectionIntevPacient()
        {
            IndexAddEdit = "";
            CallServer.PostServer(ColectioncontrollerIntevPacient, ColectioncontrollerIntevPacient + "0/0/" + _pacientProfil, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservablelColectionIntevPacient(CmdStroka);
        }


        private void BoolTrueIntevPacientCompl()
        {
            WindowIntevPacient.PacientFoldInterv.Visibility = Visibility.Visible;
        }

        private void BoolFalseIntevPacientCompl()
        {
            WindowIntevPacient.PacientFoldInterv.Visibility = Visibility.Hidden;
        }
        
        // команда удаления
        public void MethodRemoveColectionIntevPacient()
        { 
                    if (selectedIntevPacient != null)
                      {

                              string json = CompletedcontrollerIntevPacient + selectedIntevPacient.kodComplInterv.ToString() + "/0";
                              CallServer.PostServer(CompletedcontrollerIntevPacient, json, "DELETE");
                              json = ColectioncontrollerIntevPacient + selectedIntevPacient.id.ToString() + "/0/0";
                              CallServer.PostServer(ColectioncontrollerIntevPacient, json, "DELETE");
                              ColectionInterviewIntevPacients.Remove(selectedIntevPacient);
                              selectedIntevPacient = new ModelColectionInterview();                         
 
 
                      }
                      WindowIntevPacient.ColectionIntevPacientTablGrid.SelectedItem = null;
        
        }
        // команда сохранить редактирование
        public void MethodSaveColectionIntevPacient()
        { 
                     BoolFalseIntevPacientCompl();
                      WindowIntevPacient.ColectionIntevPacientTablGrid.SelectedItem = null;        
        }
        // команда печати
        public void MethodPrintColectionIntevPacient()
        { 
            if (ColectionInterviewIntevPacients != null)
            {
                MessageBox.Show("Результат діагнозу :" + ColectionInterviewIntevPacients[0].resultDiagnoz.ToString());
            }
        }

        // команда выбора новой жалобы для записи новой строки 
        private RelayCommand? readColectionIntevPacients;
        public RelayCommand ReadColectionIntevPacients
        {
            get
            {
                return readColectionIntevPacients ??
                  (readColectionIntevPacients = new RelayCommand(obj =>
                  {
                      IndexAddEdit = "editCommand";
                      ModelCall = "ModelColectionInterview";
                      GetidkodProtokola = selectedIntevPacient.kodComplInterv + "/0";
                      WinCreatIntreview NewOrder = new WinCreatIntreview();
                      NewOrder.Left = 600;
                      NewOrder.Top = 130;
                      NewOrder.ShowDialog();
                  }));
            }
        }

        private RelayCommand? onVisibleObjIntevPacients;
        public RelayCommand OnVisibleObjIntevPacients
        {
            get
            {
                return onVisibleObjIntevPacients ??
                  (onVisibleObjIntevPacients = new RelayCommand(obj =>
                  {
                      if (IndexAddEdit == "")
                      {
                          if (WindowIntevPacient.ColectionIntevPacientTablGrid.SelectedIndex == -1) return;
                          if (ColectionInterviewIntevPacients != null)
                          {
                              MainWindow WindowIntevLikar = MainWindow.LinkNameWindow("WindowMain");
                              WindowIntevPacient.PacientFoldInterv.Visibility = Visibility.Visible;
                              ColectionInterview selectedColection = ColectionIntevPacients[WindowIntevPacient.ColectionIntevPacientTablGrid.SelectedIndex];
                              if (selectedColection.kodPacient != null && selectedColection.kodPacient.Length != 0) MethodPacientIntevPacient(selectedColection, true);
                              if (selectedColection.kodDoctor != null && selectedColection.kodDoctor.Length != 0) MethodDoctorIntevPacient(selectedColection, true);
                              if (selectedColection.kodProtokola != null && selectedColection.kodProtokola.Length != 0) MethodProtokolaIntevPacient(selectedColection, true);

                          }
                      }

                  }));
            }
        }

 
        #endregion
        #endregion
    }
}
