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

namespace FrontSeam
{
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {
        public static bool addboolAppointment = false, editboolAppointment = false, addAnalogDiagnoz=false;
        public static string NameInterviewPacient = "", KodInterviewPacient = "", KodProtokola = "", DateInterview = "";
        public static string pathcontrollerAppointment = "/api/RegistrationAppointmentController/";
        public static ModelRegistrationAppointment selectRegistrationAppointment;
        public static ModelVisitingDays selectVisitingDays;

        public ModelColectionInterview SelectedColectionReceptionPatient
        {
            get { return modelColectionInterview; }
            set { modelColectionInterview = value; OnPropertyChanged("SelectedColectionReceptionPatient"); }
        }
        public static ObservableCollection<ModelRegistrationAppointment> ViewRegistrAppoints { get; set; }
        public static ObservableCollection<ModelColectionInterview> ViewReceptionPatients { get; set; }
        public void MethodLoadReceptionPacient()
        {
            LoadReceptionPacient();
        }

        public static void LoadReceptionPacient()
        {
            CallServer.PostServer(pathcontrollerAppointment, pathcontrollerAppointment + _pacientProfil, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservablelColectionRegistrationAppointment(CmdStroka);
        }


        public static void ObservablelColectionRegistrationAppointment(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelRegistrationAppointment>(CmdStroka);
            List<ModelRegistrationAppointment> res = result.ModelRegistrationAppointment.ToList();
            ViewRegistrAppoints = new ObservableCollection<ModelRegistrationAppointment>((IEnumerable<ModelRegistrationAppointment>)res);
            BildModelReceptionPatient();
            WindowIntevLikar.ReceptionLikarTablGrid.ItemsSource = ViewReceptionPatients;


        }

        public static void BildModelReceptionPatient()
        {

            ViewReceptionPatients = new ObservableCollection<ModelColectionInterview>();
            foreach (ModelRegistrationAppointment modelReceptionPatient in ViewRegistrAppoints)
            {
                modelColectionInterview = new ModelColectionInterview();
                if (modelReceptionPatient.kodDoctor != null && modelReceptionPatient.kodDoctor.Length != 0) MethodReceptionDoctor(modelReceptionPatient);
                if (modelReceptionPatient.kodPacient != null && modelReceptionPatient.kodPacient.Length != 0) MethodReceptionPacient(modelReceptionPatient);
                if (modelReceptionPatient.kodProtokola != null && modelReceptionPatient.kodProtokola.Length != 0) MethodReceptionProtokol(modelReceptionPatient);
                modelColectionInterview.kodComplInterv = modelReceptionPatient.kodComplInterv;
                modelColectionInterview.dateInterview = modelReceptionPatient.dateInterview;
                modelColectionInterview.dateDoctor = modelReceptionPatient.dateDoctor;
                modelColectionInterview.kodProtokola = modelReceptionPatient.kodProtokola;
                modelColectionInterview.resultDiagnoz = modelReceptionPatient.topictVizita;
                ViewReceptionPatients.Add(modelColectionInterview);
            }

        }

        public static void MethodReceptionDoctor(ModelRegistrationAppointment colectionInterview)
        {
            var json = DoctorcontrollerIntev + colectionInterview.kodDoctor.ToString()+"/0";
            CallServer.PostServer(DoctorcontrollerIntev, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDoctor Insert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                modelColectionInterview.nameDoctor = Insert.name + " " + Insert.surname + " " + Insert.specialnoct + " " + Insert.telefon;
                modelColectionInterview.kodDoctor = Insert.kodDoctor;
            }

        }

        public static void MethodReceptionPacient(ModelRegistrationAppointment colectionInterview)
        {

            string json = PacientcontrollerIntevLikar + colectionInterview.kodPacient.ToString() + "/0/0/0/0";
            CallServer.PostServer(PacientcontrollerIntevLikar, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelPacient Insert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                modelColectionInterview.namePacient = Insert.name + " " + Insert.surname + " " + Insert.profession + " " + Insert.tel;
                modelColectionInterview.kodPacient = Insert.kodPacient;
            }
        }

        public static void MethodReceptionProtokol(ModelRegistrationAppointment colectionInterview)
        {
            var json = ProtocolcontrollerIntevLikar + "0/" + colectionInterview.kodProtokola.ToString();
            CallServer.PostServer(ProtocolcontrollerIntevLikar, json, "GETID");
            if (CallServer.ResponseFromServer.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDependency Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                if (Insert != null)
                {
                    json = DiagnozcontrollerIntevLikar + Insert.kodDiagnoz.ToString()+"/0";
                    CallServer.PostServer(DiagnozcontrollerIntevLikar, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        modelColectionInterview.nameDiagnoz = Insert1.nameDiagnoza;
                    }

                    json = RecomencontrollerIntevLikar + Insert.kodRecommend.ToString();
                    CallServer.PostServer(RecomencontrollerIntevLikar, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelRecommendation Insert2 = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                        modelColectionInterview.nameRecomen = Insert2.contentRecommendation;
                    }
                }

            }
        }


        public void MethodAddReceptionPacient()
        {
            IndexAddEdit = "addCommand";
            if (addAnalogDiagnoz == false)
            {
                SelectedColectionReceptionPatient = null;
                modelColectionInterview = new ModelColectionInterview();
            }
            else
            {
                SelectedColectionReceptionPatient = modelColectionInterview;
            }
;
            modelColectionInterview.kodPacient = _pacientProfil;
            if (addboolAppointment == false) BoolTrueAppointment();
            else BoolFalseAppointment();

        }

        public void BoolTrueAppointment()
        {
            addboolAppointment = true;
            WindowIntevLikar.ReceptionLikarAddInterv.Visibility = Visibility.Visible;
            WindowIntevLikar.ReceptionLikarAddCompInterview.Visibility = Visibility.Visible;
            WindowIntevLikar.ReceptionLikarFolderTime.Visibility = Visibility.Visible;
            MethodEditTrue();
        }

        private void BoolFalseAppointment()
        {
            IndexAddEdit = "";
            addboolAppointment = false;
            WindowIntevLikar.ReceptionLikarAddInterv.Visibility = Visibility.Hidden;
            WindowIntevLikar.ReceptionLikarAddCompInterview.Visibility = Visibility.Hidden;
            WindowIntevLikar.ReceptionLikarCompInterview.Visibility = Visibility.Hidden;
            WindowIntevLikar.ReceptionLikarFolderTime.Visibility = Visibility.Hidden;
            MethodEditFalse();
        }

        public void MethodEditTrue()
        {
            editboolAppointment = true;
            //WindowIntevLikar.ReceptionLikar4.IsEnabled = true;
            //WindowIntevLikar.ReceptionLikar4.Background = Brushes.AntiqueWhite;
            WindowIntevLikar.ReceptionLikar7.IsEnabled = true;
            WindowIntevLikar.ReceptionLikar7.Background = Brushes.AntiqueWhite;
            WindowIntevLikar.ReceptionLikarFolderLikar.Visibility = Visibility.Visible;

        }


        private void MethodEditFalse()
        {
            editboolAppointment = false;
            WindowIntevLikar.ReceptionLikar4.IsEnabled = false;
            WindowIntevLikar.ReceptionLikar4.Background = Brushes.White;
            WindowIntevLikar.ReceptionLikar7.IsEnabled = false;
            WindowIntevLikar.ReceptionLikar7.Background = Brushes.White;
            WindowIntevLikar.ReceptionLikarFoldInterv.Visibility = Visibility.Hidden;
            WindowIntevLikar.ReceptionLikarFolderLikar.Visibility = Visibility.Hidden;

        }
        public void MethodEditReceptionPacient()
        {
            if (modelColectionInterview.dateInterview != "")
            {
                IndexAddEdit = "editCommand";
                if (editboolAppointment == true) { MethodEditFalse(); return; }
                MethodEditTrue();
                WindowIntevLikar.ReceptionLikarFoldInterv.Visibility = Visibility.Visible;
            }

        }
        public void MethodRemoveReceptionPacient()
        {
            if (WindowIntevLikar.ReceptionLikarTablGrid.SelectedIndex >= 0)
            { 
               if (modelColectionInterview.dateInterview != "")
                {
                
                    selectRegistrationAppointment = ViewRegistrAppoints[WindowIntevLikar.ReceptionLikarTablGrid.SelectedIndex];
                    string json = pathcontrollerAppointment + selectRegistrationAppointment.id.ToString()+"/0/0";
                    CallServer.PostServer(pathcontrollerAppointment, json, "DELETE");

                    modelColectionInterview = ViewReceptionPatients[WindowIntevLikar.ReceptionLikarTablGrid.SelectedIndex];
                    json = pathcontrolerAdmissionPatients + modelColectionInterview.kodPacient.ToString() + "/" + modelColectionInterview.kodDoctor.ToString() + "/" + modelColectionInterview.kodComplInterv + "/0";
                    CallServer.PostServer(pathcontrolerAdmissionPatients, json, "GETID");
                    string CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        admissionPatient = JsonConvert.DeserializeObject<AdmissionPatient>(CallServer.ResponseFromServer);
                        json = pathcontrolerAdmissionPatients + admissionPatient.id+"/0";
                        CallServer.PostServer(pathcontrolerAdmissionPatients, json, "DELETE");
                        CmdStroka = CallServer.ServerReturn();
                        if (CmdStroka.Contains("[]")) { CallServer.FalseServerGet(); return; }
                    }

                    ViewRegistrAppoints.Remove(selectRegistrationAppointment);
                    ViewReceptionPatients.Remove(modelColectionInterview);
                    WindowIntevLikar.ReceptionLikarTablGrid.ItemsSource = ViewReceptionPatients;
                    WindowIntevLikar.ReceptionLikarTablGrid.SelectedItem = null;
                    modelColectionInterview = new ModelColectionInterview();
              

                }            
            }
            BoolFalseAppointment();
        }
        public void MethodSaveReceptionPacient()
        {

            if (modelColectionInterview.dateInterview != "")
            {
                MainWindow WindowIntevPacient = MainWindow.LinkNameWindow("WindowMain");
                if (WindowIntevPacient.ReceptionLikar2.Text.Length == 0)
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви не вибрали лікаря";
                    SelectedFalseLogin();
                    return;
                }
                if (WindowIntevPacient.ReceptionLikar4.Text.Length == 0)
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine + "Не введено дату та час прийому";
                    SelectedFalseLogin();
                    return;
                }
                if (WindowIntevPacient.ReceptionLikar7.Text.Length == 0)
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine + "Не введено текст звернення";
                    SelectedFalseLogin();
                    return;
                }
                if (IndexAddEdit == "editCommand" && WindowIntevLikar.ReceptionLikarTablGrid.SelectedIndex>=0) selectRegistrationAppointment = ViewRegistrAppoints[WindowIntevLikar.ReceptionLikarTablGrid.SelectedIndex];
                modelColectionInterview.dateDoctor = WindowIntevLikar.ReceptionLikar4.Text.ToString();
                modelColectionInterview.resultDiagnoz = WindowIntevLikar.ReceptionLikar7.Text.ToString();
                if(selectRegistrationAppointment == null) selectRegistrationAppointment = new ModelRegistrationAppointment();

                selectRegistrationAppointment.kodDoctor = modelColectionInterview.kodDoctor; //nameDoctor.Substring(0, modelColectionInterview.nameDoctor.IndexOf(":"));
                selectRegistrationAppointment.kodPacient = modelColectionInterview.kodPacient; //.namePacient;
                selectRegistrationAppointment.kodProtokola = modelColectionInterview.kodProtokola;
                selectRegistrationAppointment.kodComplInterv = modelColectionInterview.kodComplInterv;
                selectRegistrationAppointment.topictVizita = modelColectionInterview.resultDiagnoz;
                selectRegistrationAppointment.dateInterview = modelColectionInterview.dateInterview;
                selectRegistrationAppointment.dateDoctor = modelColectionInterview.dateDoctor; // selectReceptionPatient.dateDoctor;
                var json = JsonConvert.SerializeObject(selectRegistrationAppointment);
                string Method = "POST";
                if (IndexAddEdit == "editCommand" && selectRegistrationAppointment.id != 0)
                {
                    Method = "PUT";
                    selectRegistrationAppointment.id = ViewRegistrAppoints[WindowIntevLikar.ReceptionLikarTablGrid.SelectedIndex].id;
                }
                CallServer.PostServer(pathcontrollerAppointment, json, Method);
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) { CallServer.FalseServerGet(); return; }
                {
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    selectRegistrationAppointment = JsonConvert.DeserializeObject<ModelRegistrationAppointment>(CallServer.ResponseFromServer);

                }

                admissionPatient = new AdmissionPatient();
                admissionPatient.kodDoctor = modelColectionInterview.kodDoctor; ;
                admissionPatient.kodPacient = modelColectionInterview.kodPacient;
                admissionPatient.kodProtokola = modelColectionInterview.kodProtokola;
                admissionPatient.kodComplInterv = modelColectionInterview.kodComplInterv;
                admissionPatient.topictVizita = modelColectionInterview.resultDiagnoz;
                admissionPatient.dateInterview = modelColectionInterview.dateInterview;
                admissionPatient.dateVizita = modelColectionInterview.dateDoctor;
        
                if ((IndexAddEdit == "editCommand" && Method == "POST") || IndexAddEdit == "addCommand")
                {

                    json = JsonConvert.SerializeObject(admissionPatient);
                    CallServer.PostServer(pathcontrolerAdmissionPatients, json, Method);
                    CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]")) CallServer.FalseServerGet();
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        admissionPatient = JsonConvert.DeserializeObject<AdmissionPatient>(CallServer.ResponseFromServer);

                    }
                }
                else 
                {
                    var admission = new AdmissionPatient();
                    json = pathcontrolerAdmissionPatients + modelColectionInterview.kodPacient.ToString() + "/" + modelColectionInterview.kodDoctor.ToString() + "/" + modelColectionInterview.kodComplInterv + "/0";
                    CallServer.PostServer(pathcontrolerAdmissionPatients, json, Method);
                    CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]")== false) 
                    {
                        
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        admission = JsonConvert.DeserializeObject<AdmissionPatient>(CallServer.ResponseFromServer);
                        admissionPatient.id = admission.id;
                        admissionPatient.topictVizita = admission.topictVizita;
                        json = JsonConvert.SerializeObject(admissionPatient);
                        CallServer.PostServer(pathcontrolerAdmissionPatients, json, "PUT");
                    }
                }

                admissionPatient = new AdmissionPatient();
                IndexAddEdit = "editCommand";
                CopycolectionInterview();
                SaveInterviewProtokol();
                IndexAddEdit = "addCommand";
                ViewRegistrAppoints = new ObservableCollection<ModelRegistrationAppointment>();
                LoadReceptionPacient();           
            }
            BoolFalseAppointment();
        }
        public void MethodPrintReceptionPacient()
        {

        }

        // команда просмотра содержимого интервью
        private RelayCommand? readColectionIntreviewReception;
        public RelayCommand ReadColectionIntreviewReception
        {
            get
            {
                return readColectionIntreviewReception ??
                  (readColectionIntreviewReception = new RelayCommand(obj =>
                  {
                      string TempIndexAddEdit = IndexAddEdit;
                      if (MapOpisViewModel.modelColectionInterview.kodComplInterv != "")
                      {
                          
                          MapOpisViewModel.IndexAddEdit = "editCommand";
                          MapOpisViewModel.GetidkodProtokola = MapOpisViewModel.modelColectionInterview.kodComplInterv + "/0";
                      }
                      else
                      {
                          MapOpisViewModel.IndexAddEdit = "";
                          MapOpisViewModel.ModelCall = "ModelColectionInterview";
                          MapOpisViewModel.GetidkodProtokola = MapOpisViewModel.modelColectionInterview.kodProtokola;
                      }
                      MapOpisViewModel.ModelCall = "ModelColectionInterview";

                      WinCreatIntreview NewOrder = new WinCreatIntreview();
                      NewOrder.Left = 600;
                      NewOrder.Top = 130;
                      NewOrder.ShowDialog();

                      IndexAddEdit = TempIndexAddEdit;
                  }));
            }
        }

        // команда выбора доктора
        private RelayCommand? reseptionPacientLikars;
        public RelayCommand ReseptionPacientLikars
        {
            get
            {
                return reseptionPacientLikars ??
                  (reseptionPacientLikars = new RelayCommand(obj =>
                  {

                      WinNsiMedZaklad MedZaklad = new WinNsiMedZaklad();
                      MedZaklad.Left = 450;
                      MedZaklad.Top = 320;
                      MedZaklad.ShowDialog();
                      EdrpouMedZaklad = ReceptionLIkarGuest.Likart8.Text.ToString();

                      if (EdrpouMedZaklad != "")
                      { 
                         WinNsiLikar NewOrder = new WinNsiLikar();
                          NewOrder.Left = 450;
                          NewOrder.Top = 320;
                          NewOrder.ShowDialog();
                          if (MapOpisViewModel.nameDoctor != "")
                          { 
                             if (modelColectionInterview == null) modelColectionInterview = new ModelColectionInterview();

                              modelColectionInterview.nameDoctor = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - MapOpisViewModel.nameDoctor.IndexOf(":") - 1);
                              modelColectionInterview.kodDoctor = MapOpisViewModel.nameDoctor.Substring(0, MapOpisViewModel.nameDoctor.IndexOf(":"));
                              WindowIntevLikar.ReceptionLikar2.Text = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - MapOpisViewModel.nameDoctor.IndexOf(":") - 1);                                                
                          }
 
                      }
 
                  }));
            }
        }

        // команда выбора доктора
        private RelayCommand? reseptionPacientInterview;
        public RelayCommand ReseptionPacientInterview
        {
            get
            {
                return reseptionPacientInterview ??
                  (reseptionPacientInterview = new RelayCommand(obj =>
                  {

                      WinListInteviewPacient NewOrder = new WinListInteviewPacient();
                      NewOrder.Left = 600;
                      NewOrder.Top = 200;
                      NewOrder.ShowDialog();
                      if (KodProtokola.Length != 0)
                      {
                          if (modelColectionInterview == null) modelColectionInterview = new ModelColectionInterview();
                          modelColectionInterview.kodComplInterv = KodInterviewPacient;
                          modelColectionInterview.nameInterview = NameInterviewPacient;
                          modelColectionInterview.dateInterview = DateInterview;
                          modelColectionInterview.kodProtokola = KodProtokola;

                          ModelRegistrationAppointment modelReceptionPatient = new ModelRegistrationAppointment();
                          modelReceptionPatient.kodProtokola = KodProtokola;
                          MethodReceptionProtokol(modelReceptionPatient);
                          WindowIntevLikar.ReceptionLikar1.Text = modelColectionInterview.dateInterview;
                          WindowIntevLikar.ReceptionLikar5.Text = modelColectionInterview.nameRecomen;
                          WindowIntevLikar.ReceptionLikar6.Text = modelColectionInterview.nameDiagnoz;
                          WindowIntevLikar.ReceptionLikarFoldInterv.Visibility = Visibility.Visible;
                          WindowIntevLikar.ReceptionLikarCompInterview.Visibility = Visibility.Visible;
                      }

                  }));
            }
        }

        private RelayCommand? onVisibleReceptionPacients;
        public RelayCommand OnVisibleReceptionPacients
        {
            get
            {
                return onVisibleReceptionPacients ??
                  (onVisibleReceptionPacients = new RelayCommand(obj =>
                  {

                      if (WindowIntevLikar.ReceptionLikarTablGrid.SelectedIndex == -1) return;
                      if (ViewReceptionPatients != null)
                      {
                          MainWindow WindowIntevPacient = MainWindow.LinkNameWindow("WindowMain");
                          modelColectionInterview = ViewReceptionPatients[WindowIntevLikar.ReceptionLikarTablGrid.SelectedIndex];
                          modelColectionInterview.namePacient = _pacientName;
                          //WindowIntevPacient.ReceptionLikarCompInterview.Visibility = Visibility.Visible;
                          //WindowIntevPacient.ReceptionLikarFoldInterv.Visibility = Visibility.Visible;
                          WindowIntevPacient.ReceptionLikarLoadinterv.Visibility = Visibility.Hidden;
                      }


                  }));
            }
        }

        // команда выбора даты приема
        private RelayCommand? selectDayVisitingLikar;
        public RelayCommand SelectDaysVisitingLikar
        {
            get
            {
                return selectDayVisitingLikar ??
                  (selectDayVisitingLikar = new RelayCommand(obj =>
                  {

                      if (_kodDoctor.Length == 0)
                      {
                          MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви не вибрали лікаря";
                          SelectedFalseLogin();
                          return;
                      }

                      WinVisitingDays NewOrder = new WinVisitingDays();
                      NewOrder.Left = 350;
                      NewOrder.Top = 320;
                      NewOrder.ShowDialog();
                      if (selectVisitingDays != null)
                      {
                          MainWindow ReceptionLIkarPacient = MainWindow.LinkNameWindow("WindowMain");
                          ReceptionLIkarPacient.ReceptionLikar4.Text = selectVisitingDays.dateVizita + " :" + selectVisitingDays.timeVizita;
                      }

                  }));
            }
        }



    }

  
}
