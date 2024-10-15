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

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    public class ViewModelNsiPacient : BaseViewModel
    {
        private static MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
        private WinNsiPacient WindowMen = MainWindow.LinkMainWindow("WinNsiPacient");
        private static string pathcontroller = "/api/PacientController/";

        private string _namePacient;
        public string NamePacient
        {
            get => _namePacient;
            set
            {
                _namePacient = value;
                OnPropertyChanged(nameof(NamePacient));
            }
        }

        private string _surnamePacient;
        public string SurNamePacient
        {
            get => _surnamePacient;
            set
            {
                _surnamePacient = value;
                OnPropertyChanged(nameof(SurNamePacient));
            }
        }

        private string _telefonPacient;
        public string TelefonPacient
        {
            get => _telefonPacient;
            set
            {
                _telefonPacient = value;
                OnPropertyChanged(nameof(TelefonPacient));
            }
        }
        public static ModelPacient selectedPacient;
        public static ObservableCollection<ModelPacient> NsiPacients { get; set; }


        public ModelPacient SelectedPacient
        { get { return selectedPacient; } set { selectedPacient = value; OnPropertyChanged("SelectedPacient"); } }
        // конструктор класса
        public ViewModelNsiPacient()
        {
            LoadNsiPacient();
        }

        public static void LoadNsiPacient()
        {
            CallServer.PostServer(pathcontroller, pathcontroller, "GET");
            string CmdStroka = CallServer.ServerReturn();
            ObservableNsiModelPacient(CmdStroka);
            

        }
        public static void ObservableNsiModelPacient(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelPacient>(CmdStroka);
            List<ModelPacient> res = result.ViewPacient.ToList();
            NsiPacients = new ObservableCollection<ModelPacient>((IEnumerable<ModelPacient>)res);
            
        }



        // команда закрытия окна
        RelayCommand? closePacient;
        public RelayCommand ClosePacient
        {
            get
            {
                return closePacient ??
                  (closePacient = new RelayCommand(obj =>
                  {
                      WindowMen.Close();
                  }));
            }
        }


        
        // команда создания профиля пациента
        RelayCommand? pacientProfilInsert;
        public RelayCommand PacientProfilInsert 
        {
            get
            {
                return pacientProfilInsert ??
                  (pacientProfilInsert = new RelayCommand(obj =>
                  {
                      MapOpisViewModel._pacientProfil = "";
                      WinProfilPacient NewOrder = new WinProfilPacient();
                      NewOrder.ShowDialog();


                  }));
            }
        }

        // команда поиска пациента по имени фамилии телефону
        RelayCommand? searchPacient;
        public RelayCommand SearchPacient
        {
            get
            {
                return searchPacient ??
                  (searchPacient = new RelayCommand(obj =>
                  {
                      string CmdStroka = "";
                      if (NamePacient == null && SurNamePacient == null && TelefonPacient == null)
                      {
                          CallServer.PostServer(pathcontroller, pathcontroller, "GET");
                      }
                      else
                      {
                          if (NamePacient != null && SurNamePacient != null && TelefonPacient != null)
                          {
                              CallServer.PostServer(pathcontroller, pathcontroller + "0/0/" + NamePacient + "/" + SurNamePacient + "/" + TelefonPacient, "GETID");
                          }
                          else
                          {
                              if (NamePacient != null && SurNamePacient != null && TelefonPacient == null)
                              {
                                  CallServer.PostServer(pathcontroller, pathcontroller + "0/0/" + NamePacient + "/" + SurNamePacient + "/0", "GETID");

                              }
                              else
                              {
                                  if (NamePacient != null && SurNamePacient == null && TelefonPacient == null)
                                  {
                                      CallServer.PostServer(pathcontroller, pathcontroller + "0/0/" + NamePacient + "/0/0", "GETID");

                                  }
                                  else
                                  {
                                      if (NamePacient == null && SurNamePacient != null && TelefonPacient != null)
                                      {
                                          CallServer.PostServer(pathcontroller, pathcontroller + "0/0/0/" + SurNamePacient + "/" + TelefonPacient, "GETID");

                                      }
                                      else
                                      {
                                          if (NamePacient == null && SurNamePacient != null && TelefonPacient == null)
                                          {
                                              CallServer.PostServer(pathcontroller, pathcontroller + "0/0/0/" + SurNamePacient + "/0", "GETID");

                                          }
                                          else
                                          {
                                              if (NamePacient == null && SurNamePacient == null && TelefonPacient != null)
                                              {
                                                  CallServer.PostServer(pathcontroller, pathcontroller + "0/0/0/0/" + TelefonPacient, "GETID");
                                              }
                                              else
                                              {
                                                  if (NamePacient != null && SurNamePacient == null && TelefonPacient != null)
                                                  {
                                                      CallServer.PostServer(pathcontroller, pathcontroller + "0/0/" + NamePacient + "/0/" + TelefonPacient, "GETID");
                                                  }
                                                  else NsiPacients = new ObservableCollection<ModelPacient>();
                                              }
                                          }

                                      }

                                  }

                              }

                          }

                      }

                      CmdStroka = CallServer.ServerReturn();
                      if (CmdStroka.Contains("[]") == false) ObservableNsiModelPacient(CmdStroka);
                      else NsiPacients = new ObservableCollection<ModelPacient>();

                      WindowMen.TablPacients.ItemsSource = NsiPacients;
                      NamePacient = null;
                      SurNamePacient = null;
                      TelefonPacient = null;

                  }));
            }
        }

        // команда выбора строки из списка
        RelayCommand? selectPacient;
        public RelayCommand SelectPacient
        {
            get
            {
                return selectPacient ??
                  (selectPacient = new RelayCommand(obj =>
                  {
                      MetodselectPacient();
                  }));
            }
        }

        // команда выбора строки из списка 
        RelayCommand? selectTablPacients;
        public RelayCommand SelectTablPacients
        {
            get
            {
                return selectTablPacients ??
                  (selectTablPacients = new RelayCommand(obj =>
                  {
                      MetodselectPacient();
                  }));
            }
        }

        private void MetodselectPacient()
        {
            if (selectedPacient != null && selectedPacient.id !=0)
            {
                MapOpisViewModel.namePacient = selectedPacient.kodPacient.ToString() + ": " + selectedPacient.name.ToString();
                WindowMain.LikarIntert3.Text = selectedPacient.kodPacient.ToString() + ": " + selectedPacient.name.ToString() + " " + selectedPacient.surname.ToString() + " " + selectedPacient.tel.ToString();

                WindowMain.AccountUsert5.Text = selectedPacient.kodPacient.ToString() + ": " + selectedPacient.name.ToString() + " " + selectedPacient.surname.ToString();
                if (MapOpisViewModel.CallViewProfilLikar == "ProfilPacient") MapOpisViewModel.selectedProfilPacient = selectedPacient;
                if (MapOpisViewModel.CallViewProfilLikar == "PacientProfil") MapOpisViewModel.selectedPacientProfil = selectedPacient;
                selectedPacient = new ModelPacient();
                WindowMen.Close();
            }
        }
    }
}
