﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
using System.Diagnostics;

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{

    public partial class ListModelInterview
    {

        [JsonProperty("list")]
        public ModelInterview[] ModelInterview { get; set; }

    }
    public class ModelInterview : BaseViewModel
    {


        // словарь протоколов интревью
        private int Id;
        private string NametInterview;
        private string KodProtokola;
        private string DetailsInterview;
        private string OpistInterview;
        private string UriInterview;
        private string IdUser;

        public ModelInterview(int Id = 0, int Numberstr = 0, string KodProtokola = "", string DetailsInterview = "",
            string NametInterview = "", string OpistInterview = "", string UriInterview = "", string IdUser = "")
        {
            this.Id = Id;
            this.NametInterview = NametInterview;
            this.KodProtokola = KodProtokola;
            this.DetailsInterview = DetailsInterview;
            this.OpistInterview = OpistInterview;
            this.UriInterview = UriInterview;
            this.IdUser = IdUser;

        }
        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodProtokola")]
        public string kodProtokola
        {
            get { return KodProtokola; }
            set { KodProtokola = value; OnPropertyChanged("kodProtokola"); }
        }

        [JsonProperty("detailsInterview")]
        public string detailsInterview
        {
            get { return DetailsInterview; }
            set { DetailsInterview = value; OnPropertyChanged("detailsInterview"); }
        }


        [JsonProperty("nametInterview")]
        public string nametInterview
        { get { return NametInterview; } set { NametInterview = value; OnPropertyChanged("nametInterview"); } }

        [JsonProperty("opistInterview")]
        public string opistInterview
        {
            get { return OpistInterview; }
            set { OpistInterview = value; OnPropertyChanged("opistInterview"); }
        }

        [JsonProperty("uriInterview")]
        public string uriInterview
        {
            get { return UriInterview; }
            set { UriInterview = value; OnPropertyChanged("uriInterview"); }
        }

        [JsonProperty("idUser")]
        public string idUser
        {
            get { return IdUser; }
            set { IdUser = value; OnPropertyChanged("idUser"); }
        }
    }


    public partial class ListModelResultInterview
    {

        [JsonProperty("list")]
        public ModelResultInterview[] ModelResultInterview { get; set; }

    }

    public class ModelResultInterview : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }


        private string NametInterview;
        private string KodProtokola;
        private string DetailsInterview;
        private string OpistInterview;
        private string UriInterview;
        private string DateInterview;
        private string KodComplInterv;
        private string NameDiagnoza;
        private string NameRecommendation;
        public ModelResultInterview(string KodProtokola="",string DetailsInterview="",string NametInterview="",
            string OpistInterview="", string UriInterview="", string DateInterview="",string KodComplInterv="",
            string NameDiagnoza="",string NameRecommendation="")
        {
            this.DateInterview = DateInterview;
            this.DetailsInterview = DetailsInterview;
            this.UriInterview = UriInterview;
            this.KodProtokola = KodProtokola;
            this.NametInterview = NametInterview;
            this.OpistInterview = OpistInterview;
            this.KodComplInterv = KodComplInterv;
            this.NameDiagnoza = NameDiagnoza;
            this.NameRecommendation = NameRecommendation;
        }
        public string kodProtokola
        {
            get { return KodProtokola; }
            set { KodProtokola = value; OnPropertyChanged("kodProtokola"); }
        }
        public string detailsInterview
        {
            get { return DetailsInterview; }
            set { DetailsInterview = value; OnPropertyChanged("detailsInterview"); }
        }
        public string nametInterview
        {
            get { return NametInterview; }
            set { NametInterview = value; OnPropertyChanged("nametInterview"); }
        }
        public string opistInterview
        {
            get { return OpistInterview; }
            set { OpistInterview = value; OnPropertyChanged("opistInterview"); }
        }
        public string uriInterview
        {
            get { return UriInterview; }
            set { UriInterview = value; OnPropertyChanged("uriInterview"); }
        }
        public string nameDiagnoza
        {
            get { return NameDiagnoza; }
            set { NameDiagnoza = value; OnPropertyChanged("nameDiagnoza"); }
        }

        public string nameRecommendation
        {
            get { return NameRecommendation; }
            set { NameRecommendation = value; OnPropertyChanged("nameRecommendation"); }
        }
        public string dateInterview
        {
            get { return DateInterview; }
            set { DateInterview = value; OnPropertyChanged("dateInterview"); }
        }
        public string kodComplInterv
        {
            get { return KodComplInterv; }
            set { KodComplInterv = value; OnPropertyChanged("kodComplInterv"); }
        }

        // команда закрытия окна
        RelayCommand? closeResult;
        public RelayCommand CloseResult
        {
            get
            {
                return closeResult ??
                  (closeResult = new RelayCommand(obj =>
                  {
                      WinResultInterview WindowResult = MainWindow.LinkMainWindow("WinResultInterview");
                      WindowResult.Close();
                  }));
            }
        }

        // команда продолжения опроса
        RelayCommand? continueInterview;
        public RelayCommand ContinueInterview
        {
            get
            {
                return continueInterview ??
                  (continueInterview = new RelayCommand(obj =>
                  {
                      WinResultInterview WindowResult = MainWindow.LinkMainWindow("WinResultInterview");
                      MapOpisViewModel.ContinueCompletedInterview();
                      WindowResult.Close();
                  }));
            }
        }

        // команда сохранения результата опроса
        RelayCommand? saveIntervDiagnoz;
        public RelayCommand SaveIntervDiagnoz
        {
            get
            {
                return saveIntervDiagnoz ??
                  (saveIntervDiagnoz = new RelayCommand(obj =>
                  {
                      WinResultInterview WindowResult = MainWindow.LinkMainWindow("WinResultInterview");
                      CallServer.PostServer(MapOpisViewModel.pathcontrolerContent, MapOpisViewModel.pathcontrolerContent + MapOpisViewModel.modelColectionInterview.kodProtokola, "GETID");
                      string CmdStroka = CallServer.ServerReturn();
                      if (CmdStroka.Contains("[]")) return;
                      else ViewModelAnalogDiagnoz.ObservableContentInterv(CmdStroka);

                      MapOpisViewModel.IndexAddEdit = "addCommand";
                      ViewModelAnalogDiagnoz.SaveInterview();
                      MainWindow.MessageError = "Увага! вибраний вами попередній діагноз " + Environment.NewLine +
                       " збережений у реєстрі проведених опитуваннь. Для  його перегляду " + Environment.NewLine +
                       "вам необхідно натиснути закладку 'Перегляд проведених опитуваннь' та  на кнопку 'Завантажити'.";
                      MapOpisViewModel.SelectedFalseLogin(10);
                      WindowResult.Close();

                  }));
            }
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
                      MapOpisViewModel.IndexAddEdit = "";
                      MapOpisViewModel.ModelCall = "ModelColectionInterview";
                      MapOpisViewModel.GetidkodProtokola = MapOpisViewModel.modelColectionInterview.kodProtokola; //kodComplInterv + "/0"

                      WinCreatIntreview NewOrder = new WinCreatIntreview();
                      NewOrder.Left = 600;
                      NewOrder.Top = 130;
                      NewOrder.ShowDialog();

                  }));
            }
        }

        // команда просмотра содержимого интервью
        private RelayCommand? runGoogleUri;
        public RelayCommand RunGoogleUri
        {
            get
            {
                return runGoogleUri ??
                  (runGoogleUri = new RelayCommand(obj =>
                  {
                      string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                      string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                      string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                      Process Rungoogle = new Process();
                      Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                      Rungoogle.StartInfo.Arguments = ViewModelResultInterview.selectItogInterview.uriInterview;
                      //Rungoogle.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                      Rungoogle.StartInfo.UseShellExecute = false;
                      Rungoogle.EnableRaisingEvents = true;
                      Rungoogle.Start();
                  }));
            }
        }

        // команда закрытия окна
        private RelayCommand? reseptionLikar;
        public RelayCommand ReseptionLikar
        {
            get
            {
                return reseptionLikar ??
                  (reseptionLikar = new RelayCommand(obj =>
                  {

                      MainWindow WindowIntevLikar = MainWindow.LinkNameWindow("WindowMain");
                      if (MapOpisViewModel.ViewReceptionPatients == null) MapOpisViewModel.ViewReceptionPatients = new ObservableCollection<ModelColectionInterview>();
                      MapOpisViewModel.ViewReceptionPatients.Add(MapOpisViewModel.modelColectionInterview);

                      switch (MapOpisViewModel.ActCompletedInterview)
                      {
                          case "Likar":

                              WindowIntevLikar.ReceptionPacient4.IsEnabled = true;
                              WindowIntevLikar.ReceptionPacient4.Background = Brushes.AntiqueWhite;
                              WindowIntevLikar.ReceptionPacient7.IsEnabled = true;
                              WindowIntevLikar.ReceptionPacient7.Background = Brushes.AntiqueWhite;
                              WindowIntevLikar.ReceptionPacientFoldProfil.Visibility = Visibility.Visible;
                              break;
                          case "Pacient":
                              WindowIntevLikar.ReceptionLikar4.IsEnabled = true;
                              WindowIntevLikar.ReceptionLikar4.Background = Brushes.AntiqueWhite;
                              WindowIntevLikar.ReceptionLikar7.IsEnabled = true;
                              WindowIntevLikar.ReceptionLikar7.Background = Brushes.AntiqueWhite;
                              WindowIntevLikar.ReceptionLikarFolderLikar.Visibility = Visibility.Visible;
                              WindowIntevLikar.ReceptionLikarCompInterview.Visibility = Visibility.Visible;
                              WindowIntevLikar.ReceptionLikarFoldInterv.Visibility = Visibility.Visible;
                              WindowIntevLikar.ReceptionLikarFolderTime.Visibility = Visibility.Visible;
                              WindowIntevLikar.ReceptionLikarTablGrid.ItemsSource = MapOpisViewModel.ViewReceptionPatients;
                              WindowIntevLikar.ReceptionLikarLoadinterv.Content = "Ваші дії:  -вибрати лікаря натиснув" + Environment.NewLine + " на малюнок папки; -ввести дату, час прийому та зміст звернення;-натиснути кнопку 'Зберегти'. ";
                              WindowIntevLikar.ReceptionLikarLoadinterv.Width = 630;
                              WindowIntevLikar.ReceptionLikarLoadinterv.Height = 70;
                              WindowIntevLikar.ReceptionLikarLoadinterv.HorizontalAlignment = HorizontalAlignment.Left;
                              WindowIntevLikar.ReceptionLikarLoadinterv.FontSize = 12;
                              WindowIntevLikar.ReceptionLikarLoadinterv.FontWeight = FontWeights.Black;
                              break;
                          case "Guest":
                              MapOpisViewModel.modelColectionInterview.namePacient = "";
                              MapOpisViewModel.addReceptionLIkarGuest = true;
                              WindowIntevLikar.ReceptionLikarFolderLikarGuest.Visibility = Visibility.Visible;
                              WindowIntevLikar.ReceptionLikarFolderGuestTime.Visibility = Visibility.Visible;
                              WindowIntevLikar.ReceptionLikarGuestFoldInterv.Visibility = Visibility.Visible;
                              WindowIntevLikar.ReceptionLikarGuestCompInterview.Visibility = Visibility.Visible;
                              WindowIntevLikar.ReseptionZapisLikar.Text = "Ваші дії: -вибрати лікаря натиснув на малюнок папки; -ввести дату, час прийому та зміст звернення; -натиснути кнопку 'Зберегти'. ";
                              WindowIntevLikar.ReceptionLikarGuest3.IsEnabled = true;
                              WindowIntevLikar.ReceptionLikarGuest3.Background = Brushes.AntiqueWhite;
                              WindowIntevLikar.ReceptionLikarGuest3.Text = "";
                              WindowIntevLikar.ReceptionLikarGuest4.IsEnabled = true;
                              WindowIntevLikar.ReceptionLikarGuest4.Background = Brushes.AntiqueWhite;
                              WindowIntevLikar.ReceptionLikarGuest7.IsEnabled = true;
                              WindowIntevLikar.ReceptionLikarGuest7.Background = Brushes.AntiqueWhite;
                              break;
                      }
 

                      if (MapOpisViewModel.ViewRegistrAppoints == null) MapOpisViewModel.ViewRegistrAppoints = new ObservableCollection<ModelRegistrationAppointment>();
                      MapOpisViewModel.selectRegistrationAppointment = new ModelRegistrationAppointment();
                      MapOpisViewModel.selectRegistrationAppointment.kodComplInterv = MapOpisViewModel.modelColectionInterview.kodComplInterv;
                      MapOpisViewModel.selectRegistrationAppointment.kodPacient = MapOpisViewModel.modelColectionInterview.kodPacient;
                      MapOpisViewModel.selectRegistrationAppointment.dateInterview = MapOpisViewModel.modelColectionInterview.dateInterview; ;
                      MapOpisViewModel.ViewRegistrAppoints.Add(MapOpisViewModel.selectRegistrationAppointment);

                      WinResultInterview WindowResult = MainWindow.LinkMainWindow("WinResultInterview");
                      WindowResult.Close();
                      AddResultInterviewreseptionLikar();
                  }));
            }
        }

        // команда выбора профильного мед. учреждения
        RelayCommand? profilMedical;
        public RelayCommand ProfilMedical
        {
            get
            {
                return profilMedical ??
                  (profilMedical = new RelayCommand(obj =>
                  {
                      MainWindow WindowIntevLikar = MainWindow.LinkNameWindow("WindowMain");
                      WinNsiMedZaklad MedZaklad = new WinNsiMedZaklad();
                      MedZaklad.ShowDialog();
                      MapOpisViewModel.EdrpouMedZaklad = WindowIntevLikar.Likart8.Text.ToString();
                      if (MapOpisViewModel.EdrpouMedZaklad.Length > 0)
                      {
                          WinNsiLikar NewOrder = new WinNsiLikar();
                          NewOrder.ShowDialog();
                          if (MapOpisViewModel.nameDoctor.Length > 0)
                          {
                              MapOpisViewModel.modelColectionInterview.nameDoctor = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":"), MapOpisViewModel.nameDoctor.Length - (MapOpisViewModel.nameDoctor.IndexOf(":") + 1));

                          }
                      }
                  }));
            }
        }

        public void AddResultInterviewreseptionLikar()
        {
            switch (MapOpisViewModel.ActCompletedInterview)
            {
                case "Likar":
                    MainWindow.MessageError = "Висновки за резльтатами проведеного опитування" + Environment.NewLine +
                    "переміщені в закладку 'Розклад прийому пацієнтів'." + Environment.NewLine + "Для подальшого їх використання натисніть на закладку 'Розклад прийому пацієнтів'";
                    break;
                default:
                    MainWindow.MessageError = "Висновки за резльтатами проведеного опитування" + Environment.NewLine +
                    "переміщені в закладку 'Запис на прийом до лікаря'." + Environment.NewLine + "Для подальшого їх використання натисніть на закладку 'Запис на прийом до лікаря'";
                    break;
            }
            MapOpisViewModel.SelectedWirning();
        }


        // команда закрытия окна
        private RelayCommand? printDiagnoz;
        public RelayCommand PrintDiagnoz
        {
            get
            {
                return printDiagnoz ??
                  (printDiagnoz = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.ModelCall = "ModelColectionInterview";
                      MapOpisViewModel.GetidkodProtokola = MapOpisViewModel.modelColectionInterview.kodComplInterv + "/0";
                      MapOpisViewModel.PrintDiagnoz();

                  }));
            }
        }

 
    }
 }
