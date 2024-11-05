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
using System.Diagnostics;

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    public class ViewModelNsiLikar : BaseViewModel
    {

        private WinNsiLikar WindowMen = MainWindow.LinkMainWindow("WinNsiLikar");
        private MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
        private string pathcontroller = "/api/ApiControllerDoctor/";
        public static string controlerLikarGrDiagnoz = "/api/LikarGrupDiagnozController/";
        public static ModelDoctor selectedLikar;
        public static ObservableCollection<ModelDoctor> NsiLikars { get; set; }


        public ModelDoctor SelectedLikar
        { get { return selectedLikar; } set { selectedLikar = value; OnPropertyChanged("SelectedLikar"); } }
        // конструктор класса
        public ViewModelNsiLikar()
        {
            if (MapOpisViewModel.EdrpouMedZaklad == "")
            {
                CallServer.PostServer(pathcontroller, pathcontroller, "GET");
            }
            else
            {
                CallServer.PostServer(pathcontroller, pathcontroller + "0/" + MapOpisViewModel.EdrpouMedZaklad + "/0", "GETID");
            }

            string CmdStroka = CallServer.ServerReturn();
            ObservableNsiModelLikar(CmdStroka);
        }
        public static void ObservableNsiModelLikar(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDoctor>(CmdStroka);
            List<ModelDoctor> res = result.ModelDoctor.ToList();
            NsiLikars = new ObservableCollection<ModelDoctor>((IEnumerable<ModelDoctor>)res);

            if (MapOpisViewModel.selectIcdGrDiagnoz != "")
            {
                ObservableCollection<ModelDoctor> TmpNsiLikars = new ObservableCollection<ModelDoctor>();
                foreach (ModelDoctor modelDoctor in NsiLikars)
                {
                    string json = controlerLikarGrDiagnoz + modelDoctor.kodDoctor + "/" + MapOpisViewModel.selectIcdGrDiagnoz.ToString() + "/0";
                    CallServer.PostServer(controlerLikarGrDiagnoz, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false) TmpNsiLikars.Add(modelDoctor);
                }
                if (TmpNsiLikars.Count != 0) NsiLikars = TmpNsiLikars;
            }
        }



        // команда закрытия окна
        RelayCommand? closeLikar;
        public RelayCommand CloseLikar
        {
            get
            {
                return closeLikar ??
                  (closeLikar = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
                      WindowMain.LikarIntert2.Text = "";
                      WindowMen.Close();
                  }));
            }
        }

        // команда выбора строки из списка жалоб
        RelayCommand? selectLikar;
        public RelayCommand SelectLikar
        {
            get
            {
                return selectLikar ??
                  (selectLikar = new RelayCommand(obj =>
                  {
                      MetodSelectTablLikars();
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? viewUriWebProfilLikar;
        public RelayCommand ViewUriWebProfilLikar
        {
            get
            {
                return viewUriWebProfilLikar ??
                  (viewUriWebProfilLikar = new RelayCommand(obj =>
                  {
                      if (selectedLikar.uriwebDoctor != "")
                      {
                          string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                          string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                          string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                          Process Rungoogle = new Process();
                          Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                          Rungoogle.StartInfo.Arguments = selectedLikar.uriwebDoctor;
                          //Rungoogle.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                          Rungoogle.StartInfo.UseShellExecute = false;
                          Rungoogle.EnableRaisingEvents = true;
                          Rungoogle.Start();
                      }
                  }));
            }
        }

        // команда выбора строки из списка жалоб
        RelayCommand? selectTablLikars;
        public RelayCommand SelectTablLikars
        {
            get
            {
                return selectTablLikars ??
                  (selectTablLikars = new RelayCommand(obj =>
                  {
                      MetodSelectTablLikars();
                  }));
            }
        }

        private void MetodSelectTablLikars()
        {
            WindowMain.AccountUsert5.Text = "";
            MapOpisViewModel.nameDoctor = "";
            if (selectedLikar != null)
            {
                MapOpisViewModel._kodDoctor = selectedLikar.kodDoctor.ToString();
                MapOpisViewModel.nameDoctor = selectedLikar.kodDoctor.ToString() + ": " + selectedLikar.name.ToString() + " " + selectedLikar.surname.ToString() + " " + selectedLikar.telefon.ToString();
                //if (MapOpisViewModel.ActCompletedInterview != "Guest")
                //{
                    WindowMain.LikarIntert2.Text = selectedLikar.kodDoctor.ToString() + ": " + selectedLikar.name.ToString() + " " + selectedLikar.surname.ToString() + " " + selectedLikar.telefon.ToString();
                    WindowMain.AccountUsert5.Text = selectedLikar.kodDoctor.ToString() + ": " + selectedLikar.name.ToString() + " " + selectedLikar.surname.ToString();
                    WindowMain.LikarIntert2.Text = selectedLikar.kodDoctor.ToString() + ": " + selectedLikar.name.ToString() + " " + selectedLikar.surname.ToString() + " " + selectedLikar.telefon.ToString();
                    WindowMain.AccountUsert5.Text = selectedLikar.kodDoctor.ToString() + ": " + selectedLikar.name.ToString() + " " + selectedLikar.surname.ToString();

                    if (MapOpisViewModel.CallViewProfilLikar == "ProfilLikar") MapOpisViewModel.selectedProfilLikar = selectedLikar;

                    if (MapOpisViewModel.ModelCall == "ReceptionLIkar")
                    {
                        MainWindow.MessageError = "Увага!" + Environment.NewLine +
                                  "Для запису на прийом до лікаря необхідно ввести початкові данні про себе. " + Environment.NewLine +
                                  "Ви будете формувати особисту картку? ";
                        MapOpisViewModel.SelectedDelete();

                        if (MapOpisViewModel.DeleteOnOff == true)
                        {

                            MapOpisViewModel._pacientProfil = "";
                            WinProfilPacient NewPacient = new WinProfilPacient();
                            NewPacient.ShowDialog();

                            if (MapOpisViewModel._pacientProfil != "")
                            {
                                MapOpisViewModel.admissionPatient = new AdmissionPatient();
                                MapOpisViewModel.admissionPatient.kodDoctor = MapOpisViewModel._kodDoctor;
                                MapOpisViewModel.admissionPatient.kodPacient = MapOpisViewModel.selectedProfilPacient.kodPacient;
                                MapOpisViewModel.admissionPatient.kodProtokola = MapOpisViewModel.modelColectionInterview.kodProtokola;
                                MapOpisViewModel.admissionPatient.kodComplInterv = MapOpisViewModel.modelColectionInterview.kodComplInterv;
                                MapOpisViewModel.admissionPatient.topictVizita = "Гість:  " + WindowMain.ReceptionLikarGuest7.Text.ToString();
                                MapOpisViewModel.admissionPatient.dateInterview = MapOpisViewModel.modelColectionInterview.dateInterview;
                                MapOpisViewModel.admissionPatient.dateVizita = WindowMain.ReceptionLikarGuest4.Text.ToString();
                                var json = JsonConvert.SerializeObject(MapOpisViewModel.admissionPatient);
                                CallServer.PostServer(MapOpisViewModel.pathcontrolerAdmissionPatients, json, "POST");
                                string CmdStroka = CallServer.ServerReturn();
                                if (CmdStroka.Contains("[]")) CallServer.FalseServerGet();

                            }


                        }
                    }
                //}
            }
            WindowMen.Close();
            WinResultInterview WindowResult = MainWindow.LinkMainWindow("WinResultInterview");
            if (WindowResult != null) WindowResult.Close();
            WinAnalogDiagnoz WinAnalog = MainWindow.LinkMainWindow("WinAnalogDiagnoz");
            if (WinAnalog != null) WinAnalog.Close();
        }

        // Выбор названия мед закладу
        private RelayCommand? searchPoiskSurnaeLikar;
        public RelayCommand SearchPoiskSurnaeLikar
        {
            get
            {
                return searchPoiskSurnaeLikar ??
                  (searchPoiskSurnaeLikar = new RelayCommand(obj =>
                  {
                      if (WindowMen.PoiskSurnaeLikar.Text.Trim() != "")
                      {
                          string jason = pathcontroller + "0/0/" + WindowMen.PoiskSurnaeLikar.Text;
                          CallServer.PostServer(pathcontroller, jason, "GETID");
                          string CmdStroka = CallServer.ServerReturn();
                          if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                          else ObservableNsiModelLikar(CmdStroka);
                          WindowMen.TablLikars.ItemsSource = NsiLikars;
                      }

                  }));
            }
        }


    }
}
