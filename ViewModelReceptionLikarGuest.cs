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
        private MainWindow ReceptionLIkarGuest = MainWindow.LinkNameWindow("WindowMain");
        public static bool addReceptionLIkarGuest = false;
        public static string pathcontrolerAdmissionPatients = "/api/ControllerAdmissionPatients/";
        public static string EdrpouMedZaklad = "", PacientPostIndex = "";
        public static AdmissionPatient admissionPatient;
        public ModelColectionInterview SelectReceptionLIkarGuest
        { get { return modelColectionInterview; } set { modelColectionInterview = value; OnPropertyChanged("SelectReceptionLIkarGuest"); } }



        public void MethodLoadReceptionLIkarGuest()
        {
            SelectReceptionLIkarGuest = modelColectionInterview;
        }
        public void MethodAddReceptionLIkarGuest()
        {
            if (OnOffStartGuest == false)
            {
                MainWindow.MessageError = "Увага!" + Environment.NewLine +
                "Для запису на прийом до лікаря необхідно пройти опитування." + Environment.NewLine + "Для цього натиснути на кнопку 'Кімната для опитування' потім натиснути на кнопку 'Додати'.";
                MessageWarning NewOrder = new MessageWarning(MainWindow.MessageError, 2, 10);
                NewOrder.Left = 150;
                NewOrder.Top = 300;
                NewOrder.ShowDialog();
                return;
            }
            
            if (addReceptionLIkarGuest == false)
            {
                ReceptionLIkarGuestTrue();

                SelectReceptionLIkarGuest = modelColectionInterview;
            }
            else
            {
                ReceptionLIkarGuestFalse();
            }
        }

        public void ReceptionLIkarGuestTrue()
        {
            addReceptionLIkarGuest = true;
            ReceptionLIkarGuest.ReceptionLikarFolderLikarGuest.Visibility = Visibility.Visible;
            ReceptionLIkarGuest.ReceptionLikarFolderGuestTime.Visibility = Visibility.Visible;
            ReceptionLIkarGuest.ReceptionLikarGuestFoldInterv.Visibility = Visibility.Visible;
            ReceptionLIkarGuest.ReceptionLikarGuestCompInterview.Visibility = Visibility.Visible;

            ReceptionLIkarGuest.ReceptionLikarGuest4.IsEnabled = true;
            ReceptionLIkarGuest.ReceptionLikarGuest4.Background = Brushes.AntiqueWhite;
            ReceptionLIkarGuest.ReceptionLikarGuest7.IsEnabled = true;
            ReceptionLIkarGuest.ReceptionLikarGuest7.Background = Brushes.AntiqueWhite;
        }

        public void ReceptionLIkarGuestFalse()
        {
            addReceptionLIkarGuest = false;
            ReceptionLIkarGuest.ReceptionLikarFolderLikarGuest.Visibility = Visibility.Hidden;
            ReceptionLIkarGuest.ReceptionLikarFolderGuestTime.Visibility = Visibility.Hidden;
            ReceptionLIkarGuest.ReceptionLikarGuestFoldInterv.Visibility = Visibility.Hidden;
            ReceptionLIkarGuest.ReceptionLikarGuestCompInterview.Visibility = Visibility.Hidden;
            ReceptionLIkarGuest.ReceptionLikarGuest3.IsEnabled = false;
            ReceptionLIkarGuest.ReceptionLikarGuest3.Background = Brushes.White;
            ReceptionLIkarGuest.ReceptionLikarGuest4.IsEnabled = false;
            ReceptionLIkarGuest.ReceptionLikarGuest4.Background = Brushes.White;
            ReceptionLIkarGuest.ReceptionLikarGuest7.IsEnabled = false;
            ReceptionLIkarGuest.ReceptionLikarGuest7.Background = Brushes.White;
        }

        public void MethodEditReceptionLIkarGuest()
        {

        }
        public void MethodRemoveReceptionLIkarGuest()
        {
            SelectReceptionLIkarGuest = new ModelColectionInterview();
            ReceptionLIkarGuestFalse();
        }
        public void MethodSaveReceptionLIkarGuest()
        {
            if (modelColectionInterview.dateInterview != "" )
            {
                if (ReceptionLIkarGuest.ReceptionLikarGuest3.Text.Length == 0 )
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine + "Не введено ім'я та прізвище пацієнта" ;
                    SelectedFalseLogin();
                    return;
                }

                if ( ReceptionLIkarGuest.ReceptionLikarGuest4.Text.Length == 0 )
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine +"Не введено дату та час прийому" ;
                    SelectedFalseLogin();
                    return;
                }
                if ( ReceptionLIkarGuest.ReceptionLikarGuest7.Text.Length == 0)
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine + "Не введено текст звернення" ;
                    SelectedFalseLogin();
                    return;
                }

                if (nameDoctor == "")
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви не вибрали лікаря";
                    SelectedFalseLogin();
                    return;
                }
                admissionPatient = new AdmissionPatient();
                string Koddoctor = nameDoctor.Contains(":") ? nameDoctor.Substring(0, nameDoctor.IndexOf(":")) : nameDoctor;
                admissionPatient.kodDoctor = Koddoctor;
                admissionPatient.kodPacient = MapOpisViewModel.selectedProfilPacient.kodPacient; //"Гість:  " + modelColectionInterview.namePacient;
                admissionPatient.kodProtokola = modelColectionInterview.kodProtokola;
                admissionPatient.kodComplInterv = modelColectionInterview.kodComplInterv;
                admissionPatient.topictVizita = "Гість:  " + WindowMain.ReceptionLikarGuest7.Text.ToString();
                admissionPatient.dateInterview = modelColectionInterview.dateInterview;
                admissionPatient.dateVizita = WindowMain.ReceptionLikarGuest4.Text.ToString();
                var json = JsonConvert.SerializeObject(admissionPatient);
                CallServer.PostServer(pathcontrolerAdmissionPatients, json, "POST");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.FalseServerGet();
                {
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    admissionPatient = JsonConvert.DeserializeObject<AdmissionPatient>(CallServer.ResponseFromServer);

                }
                SelectReceptionLIkarGuest = new ModelColectionInterview();
                admissionPatient = new AdmissionPatient();
                MapOpisViewModel.OnOffStartGuest = false;
                ReceptionLIkarGuestFalse();
                nameDoctor = "";
            }

        }
        public void MethodPrintReceptionLIkarGuest()
        {
            PrintCompletedInterview = false;
            PrintDiagnoz();
        }

        // команда просмотра содержимого интервью
        private RelayCommand? readColectionIntreview;
        public RelayCommand ReadColectionIntreview
        {
            get
            {
                return readColectionIntreview ??
                  (readColectionIntreview = new RelayCommand(obj =>
                  {
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

                  }));
            }
        }

        // команда просмотра содержимого интервью
        private RelayCommand? reseptionGuestLikars;
        public RelayCommand ReseptionGuestLikars
        {
            get
            {
                return reseptionGuestLikars ??
                  (reseptionGuestLikars = new RelayCommand(obj =>
                  {

                      if (MapOpisViewModel._pacientProfil == "")
                      { 
                            MainWindow.MessageError = "Увага!" + Environment.NewLine +
                            "Для запису на прийом до лікаря необхідно ввести початкові данні про себе. " + Environment.NewLine +
                            "Ви будете формувати особисту картку? ";
                            SelectedRemove();

                          if (MapOpisViewModel.DeleteOnOff == true)
                          {

                              MapOpisViewModel._pacientProfil = "";
                              WinProfilPacient NewPacient = new WinProfilPacient();
                              NewPacient.ShowDialog();
                          }
                      }
                      if (MapOpisViewModel._pacientProfil != "")
                      {

                            ReceptionLIkarGuest.ReceptionLikarGuest3.Text= MapOpisViewModel.selectedProfilPacient.name+" "+ MapOpisViewModel.selectedProfilPacient.surname + " " + MapOpisViewModel.selectedProfilPacient.tel;
                            modelColectionInterview.kodPacient = MapOpisViewModel.selectedProfilPacient.kodPacient;
                            modelColectionInterview.namePacient = MapOpisViewModel.selectedProfilPacient.name + " " + MapOpisViewModel.selectedProfilPacient.surname + " " + MapOpisViewModel.selectedProfilPacient.tel;
 
                            WinNsiMedZaklad MedZaklad = new WinNsiMedZaklad();
                            MedZaklad.ShowDialog();


                            EdrpouMedZaklad = ReceptionLIkarGuest.Likart8.Text.ToString();
                            if (EdrpouMedZaklad.Length > 0)
                            {
                                WinNsiLikar NewOrder = new WinNsiLikar();
                                NewOrder.ShowDialog();
                                if (nameDoctor.Length > 0)
                                {
                                    modelColectionInterview.nameDoctor = nameDoctor.Substring(nameDoctor.IndexOf(":"), nameDoctor.Length - (nameDoctor.IndexOf(":") + 1));

                                }
                            }                         
                      }                      
                      
                          
  


                      
  
                  }));
            }
        }

        // команда выбора даты приема
        private RelayCommand? selectDayVisiting;
        public RelayCommand SelectDaysVisiting
        {
            get
            {
                return selectDayVisiting ??
                  (selectDayVisiting = new RelayCommand(obj =>
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
                          ReceptionLIkarGuest.ReceptionLikarGuest4.Text = selectVisitingDays.dateVizita + " :" + selectVisitingDays.timeVizita;
                      }

                  }));
            }
        }


    }
}
