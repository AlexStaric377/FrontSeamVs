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
using System.Text.RegularExpressions;
using System.Diagnostics;

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int IdItemSelected = 0;
        public static string DiagnozRecomendaciya = "", NameDiagnoz = "", NameRecomendaciya ="", OpistInterview = "", UriInterview ="";
        public static bool endwhileselected = false, OnOffStartGuest=false, ViewAnalogDiagnoz=false, PrintCompletedInterview=false, SaveAnalogDiagnoz= false,
            StopDialog = false, EndDialogdali = false, boolSetAccountUser = false, loadboolProfilLikar = false, loadboolPacientProfil = false;
        public static string ActCompletedInterview = "null", ActCreatInterview = "", IndikatorSelected = "", selectedComplaintname = "", selectFeature="", selectGrDetailing="", selectQualification="";
        public static string InputContent = "", PacientContent="", LikarContent="", selectIcdGrDiagnoz = "";
        public static MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
        public static int NumberstrokaGuest = 0, IdItemGuestInterv = 0;
        public static string Controlleroutfile = "/api/UnLoadController/", upLoadstroka = "", RegIdUser = "", RegUserStatus = "";
        public static int _ControlTableItem = 0, _ControlGuest = 0, _ControlPacient = 0, _ControlLikar = 0, _ControlAdmin = 0;
        private bool endwhile = false;
        public static string IndexAddEdit ="", GetidkodProtokola="";
        public static string pathcontrolerContent = "/api/ContentInterviewController/";
        public static string pathcontroler = "/api/CompletedInterviewController/";
        public static string pathcontrolerColection = "/api/ColectionInterviewController/";
        public static string pathcontrolerInterview = "/api/InterviewController/";
        public static ModelCompletedInterview selectedGuestInterv;
        public static ModelCompletedInterview selectedCompletedInterv;
        public static ModelColectionInterview modelColectionInterview = new ModelColectionInterview();
        public static ModelPacient selectedProfilPacient;
        public static ColectionInterview colectionInterview;
        public static ObservableCollection<ModelInterview> AnalogInterviews = new ObservableCollection<ModelInterview>();

        public static ObservableCollection<ModelCompletedInterview> GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
        public static ObservableCollection<ModelCompletedInterview> TmpGuestIntervs = new ObservableCollection<ModelCompletedInterview>();
        public static ObservableCollection<ModelDetailing> listgrdetaling = new ObservableCollection<ModelDetailing>();
        public ModelCompletedInterview SelectedGuestInterv
        { get { return selectedGuestInterv; } set { selectedGuestInterv = value; OnPropertyChanged("SelectedGuestInterv"); } }

        public ModelPacient SelectedProfilPacient
        { get { return selectedProfilPacient; } set { selectedProfilPacient = value; OnPropertyChanged("SelectedProfilPacient"); } }


        public MapOpisViewModel()
        {
            //WinWelcome NewResult = new WinWelcome();
            //NewResult.Left = 200;
            //NewResult.Top = 100;
            //NewResult.ShowDialog();
        }
        // команда вывзова окна со списком жалоб для выбора строки  и записи в интервью
        public void MethodStartInterview()
        { 
            WindowMain.NameInterv.Text = "Загальне опитування: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();

            WindowMain.InputNameInterview.Visibility = Visibility.Hidden;
            WindowMain.StackPanelGuest.Visibility = Visibility.Hidden;
            InputContent = InputContent == ""? WindowMain.InputNameInterview.Content.ToString(): InputContent;
            IndexAddEdit = "addCommand";
            EndDialogdali = StopDialog = false;
            ActCompletedInterview = "Guest";
            OnOffStartGuest = false;
            modelColectionInterview = new ModelColectionInterview();
            AutoSelectedInterview();
            EndInterview();
        }

        public void EndInterview()
        {
          
            if (GuestIntervs.Count != 0 )
            {
                if (MapOpisViewModel.StopDialog == true)
                {
                    StopDialogtrue();
                    return;
                }

                if (EndDialogdali == false) SuccessEndInterview();
                if (MapOpisViewModel.StopDialog == true)
                {
                    StopDialogtrue();
                    return;
                }
                MetodSaveInterview();
                WindowMain.NameInterv.Text = "";
            }
        }

        private void StopDialogtrue()
        {
            GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
            Selectedswitch();
        }

        // команда редактирования интервью которое создал  пациент
        public void MethodEditCompletedInterview()
        { 
            if (GuestIntervs.Count != 0)
            { 
                IndexAddEdit = "editCommand";
                ActCompletedInterview = "Guest";
                WindowMain.InputNameInterview.Visibility = Visibility.Hidden;
                WindowMain.StackPanelGuest.Visibility = Visibility.Hidden;
                EditSelectedInterview();
                SuccessEndEditInterview();
            }       
        
        }
        // команда сохранения резулдьтата опроса 
        public void MethodSaveCompletedGuestInterview()
        {
            SetContent();
            WindowMain.InputNameInterview.Visibility = Visibility.Hidden;
            WindowMain.StackPanelGuest.Visibility = Visibility.Hidden;
            MetodSaveInterview();
        }

        // команда удаления строки интервью
        public void MethodRemoveCompletedGuestInterview()
        {

            if (GuestIntervs.Count != 0)
            {
                int IdItemSelected = WindowMain.TablGuestInterviews.SelectedIndex;
                RemovStrIntervs(IdItemSelected);
                WindowMain.StackPanelGuest.Visibility = Visibility.Hidden;
            }
        }

        // команда печати
        public void MethodPrintCompletedInterviewGuest()
        {
            PrintCompletedInterview = true;
            PrintDiagnoz();
        }

        // Дозапись или корретировка проведенного интервью
        public void EditSelectedInterview()
        {

            SelectedswitchEdit();
            if (selectedGuestInterv != null && IdItemGuestInterv > 0)
            {

                switch (selectedGuestInterv.kodDetailing.Length)
                {
                    case 5:
                        OpenNsiFeature();
                        WindowMain.TablLikarInterviews.SelectedItem = null;
                        break;
                    case 9:
                        OpenNsiDetailing();
                        WindowMain.TablLikarInterviews.SelectedItem = null;
                        break;
                    case > 9:
                        OpenNsiGrDetailing();
                        WindowMain.TablLikarInterviews.SelectedItem = null;
                        break;
                }
            }
            else
            {
                IdItemGuestInterv = GuestIntervs.Count;
                OpenNsiComplaint();
            }
        }

        public static void SelectedswitchEdit()
        {
            switch (ActCompletedInterview)
            {
                case "Guest":
                    IdItemGuestInterv = WindowMain.TablGuestInterviews.SelectedIndex;
                    break;
                case "Pacient":
                    IdItemGuestInterv = WindowMain.TablPacientInterviews.SelectedIndex;
                    break;
                case "Likar":
                    IdItemGuestInterv = WindowMain.TablLikarInterviews.SelectedIndex;
                    break;
            }
            IdItemGuestInterv++;
        }

        public void MetodSaveInterview()
        {
            if (GuestIntervs != null)
            {
                if (GuestIntervs.Count != 0)
                { 
                    MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
                    SelectNewKodComplInteriew();
                    modelColectionInterview.detailsInterview = "";
                    NumberstrokaGuest = 0;
                    // Очистка коллекции от двойных  кодов
                    if (modelColectionInterview.kodProtokola != "")
                    {
                        string json = pathcontroler + modelColectionInterview.kodProtokola + "/0";
                        CallServer.PostServer(pathcontroler, json, "DELETE");
                    }

                    // ОБращение к серверу добавляем запись в соответствии с сформированным списком
                    foreach (ModelCompletedInterview modelCompletedInterview in GuestIntervs.OrderBy(x => x.kodDetailing))
                    {
                        modelColectionInterview.detailsInterview = modelColectionInterview.detailsInterview.Length == 0
                                        ? modelCompletedInterview.kodDetailing +";" : modelColectionInterview.detailsInterview + modelCompletedInterview.kodDetailing + ";";
                    }
            
 
                    DiagnozRecomendaciya = modelColectionInterview.detailsInterview;
                    SelectedDiagnozRecom();
                    if (ViewAnalogDiagnoz == true)
                    { 
                        foreach (ModelCompletedInterview modelCompletedInterview in GuestIntervs)
                        {
                
                            modelCompletedInterview.id = 0;
                            modelCompletedInterview.numberstr = NumberstrokaGuest++;
                            modelCompletedInterview.kodComplInterv = modelColectionInterview.kodComplInterv;
                            string json = JsonConvert.SerializeObject(modelCompletedInterview);
                            CallServer.PostServer(pathcontroler, json, "POST");
                        }
                        

                        AddInterviewProtokol();
                        TmpGuestIntervs = GuestIntervs;
                    }
 
                    OnOffStartGuest = true;
                    WindowMain.TablGuestInterviews.ItemsSource = null;
                    WindowMain.TablLikarInterviews.ItemsSource = null;
                    WindowMain.TablPacientInterviews.ItemsSource = null;
                    GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
               
                }
           
            }
            WindowMain.NameInterv.Text = "";

        }

        
        private void RemovStrIntervs(int IdItemSelected)
        {
            if (selectedGuestInterv != null)
            {
                switch (selectedGuestInterv.kodDetailing.Length)
                {
                    case 0:
                        ClearGuestIntervs(0, IdItemSelected);
                        break;
                    case 5:
                        ClearGuestIntervs(5, IdItemSelected);
                        break;
                    case 9:
                        ClearGuestIntervs(9, IdItemSelected);
                        break;
                    case > 9:
                        GuestIntervs.Remove(selectedGuestInterv);
                        break;
                }
                Selectedswitch();
            }

        }
        private void ClearGuestIntervs(int Lenghkod = 0, int IdItemSelected = 0)
        {
            endwhile = false;
            ModelCompletedInterview modelContentInterv = new ModelCompletedInterview();
            while (endwhile == false)
            {
                if (IdItemSelected == 0)
                {
                    GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
                    endwhile = true;
                    break;
                }

                string remkodDetailing = "";
                int indexrem = 0, Lengthkodrem = 0;
                int CountInterv = GuestIntervs.Count;
                for (int index =0; index<=CountInterv; index++)
                {
                    
                    if (IdItemSelected <= index)
                    {
                        
                        if (IdItemSelected == index)
                        {
                            indexrem = index;
                            Lengthkodrem = GuestIntervs[index].kodDetailing.Length;
                            remkodDetailing = GuestIntervs[index].kodDetailing;
                        }
                        if (GuestIntervs.Count == indexrem) break;
                        modelContentInterv = GuestIntervs[indexrem];
                        if ((modelContentInterv.kodDetailing.Length < Lengthkodrem) || (remkodDetailing != modelContentInterv.kodDetailing && modelContentInterv.kodDetailing.Length == Lengthkodrem))
                        {
                           break;
                        }

                        GuestIntervs.Remove(modelContentInterv);
                    }
                }
                endwhile = true;
            }
            TmpGuestIntervs = GuestIntervs;
             
        }

        // Дальнейший автовыбор характеристик жалоб на основании ранее установленных.
        // Автоматическое продолжение интервью

        public  void AutoSelectedInterview()
        {
            bool endwhileFeature = false;
            OpenNsiComplaint();
            if (GuestIntervs.Count == 0)
            {
                WindowMain.InputNameProfilLikar.Visibility = Visibility.Visible;
                //WindowMain.StackPanelLikar.Visibility = Visibility.Visible;
                return;
            }
            //MessageDialogFeature();
            int countFeature = GuestIntervs.Count;
            while (endwhileFeature == false)
            {
                int shag = 1, key = 0;

                foreach (ModelCompletedInterview modelContentInterv in GuestIntervs)
                {
                    selectedGuestInterv = modelContentInterv;
                    switch (IndikatorSelected)
                    {
                        case "NsiComplaint":
                            key = 5;
                            break;
                        case "NsiFeature":
                            key = 9;
                            break;
                    }
                    AutoSelectedFeature(selectedGuestInterv, shag, countFeature, key);
                    shag++;
                }
                if (IndikatorSelected == "NsiDetailing") endwhileFeature = true;
                switch (IndikatorSelected)
                {
                    case "NsiComplaint":
                        IndikatorSelected = "NsiFeature";
                        break;
                    case "NsiFeature":
                        IndikatorSelected = "NsiDetailing";
                        break;
                }
                //if (countFeature == GuestIntervs.Count && endwhileFeature == false)
                //{
                //    MessageEndDialog();
                //    EndDialogdali = endwhileFeature = true;
                //}


                if (countFeature == GuestIntervs.Count && endwhileFeature == false)
                {

                    MessageEndDialog();
                    endwhileFeature = true;
                    if (MapOpisViewModel.StopDialog == true)
                    {
                        GuestIntervs = new ObservableCollection<ModelCompletedInterview>();
                        Selectedswitch();
                        return;
                    }
                    if (GuestIntervs.Count != 0) MetodSaveInterview(); 
                    WindowMain.NameInterv.Text = "";
                    StopDialog = true;
                }
                countFeature = GuestIntervs.Count;
            }

        }

        public static void AutoSelectedFeature(ModelCompletedInterview modelContentInterv, int shag, int countFeature, int key)
        {
            if (modelContentInterv.kodDetailing.Length == key)  // заменить цифру или показатель на независимый показатель
            {
                IdItemGuestInterv = GuestIntervs.Count - countFeature + shag;
                if (IndikatorSelected == "NsiComplaint") OpenNsiFeature();
                if (IndikatorSelected == "NsiFeature") OpenNsiDetailing();
            }
        }

        public static void OpenNsiComplaint()
        {
            
            NsiComplaint NewOrder = new NsiComplaint();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-150;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350; //350;
            NewOrder.ShowDialog();
            IndikatorSelected = "NsiComplaint";
            Selectedswitch();
 
        }

        public static void OpenNsiFeature()
        {
            selectedComplaintname = GuestIntervs[IdItemGuestInterv-1].detailsInterview;
            WinNsiFeature NewOrder = new WinNsiFeature();
            NewOrder.Left = (MainWindow.ScreenWidth / 2)-190;
            NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewOrder.ShowDialog();
        }

        public static void OpenNsiDetailing()
        {

            selectFeature = GuestIntervs[IdItemGuestInterv - 1].detailsInterview;
            string pathcontroller = "/api/DetailingController/";
            string jason = pathcontroller + "0/" + selectedGuestInterv.kodDetailing + "/0";
            CallServer.PostServer(pathcontroller, jason, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]") == false)
            {

                ViewModelNsiDetailing.ObservableNsiModelFeatures(CmdStroka);
                LoadNsiGrDetailing();
                if (ViewModelNsiDetailing.NsiModelDetailings.Count() > 0)
                {
                    NsiDetailing NewNsi = new NsiDetailing();
                    NewNsi.Left = (MainWindow.ScreenWidth / 2) - 190;
                    NewNsi.Top = (MainWindow.ScreenHeight / 2) - 350;
                    NewNsi.ShowDialog();

                }
                ViewModelNsiDetailing.NsiModelDetailings = null;

            }
       
        }

        public static void LoadNsiGrDetailing()
        {
            listgrdetaling = new ObservableCollection<ModelDetailing>();
            foreach (ModelDetailing modelDetailing in ViewModelNsiDetailing.NsiModelDetailings)
            {
                if (modelDetailing.keyGrDetailing != null && modelDetailing.keyGrDetailing != "")
                {
                    listgrdetaling.Add(modelDetailing);
                }
            }
            if (listgrdetaling.Count > 0)
            {
                foreach (ModelDetailing modelDetailing in listgrdetaling)
                {
                    ViewModelNsiDetailing.NsiModelDetailings.Remove(modelDetailing);

                    ViewModelNsiDetailing.selectedDetailing = modelDetailing;
                    MapOpisViewModel.selectGrDetailing = selectFeature + " " + modelDetailing.nameDetailing.ToString().ToUpper();
                    WinNsiGrDetailing NewOrder = new WinNsiGrDetailing();
                    NewOrder.Left = (MainWindow.ScreenWidth / 2) - 150;
                    NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350; //350;
                    NewOrder.ShowDialog();
                }
            }

        }

        public static void OpenNsiGrDetailing()
        {

            WinNsiGrDetailing NewNsi = new WinNsiGrDetailing();
            NewNsi.Left = (MainWindow.ScreenWidth / 2) - 150;
            NewNsi.Top = (MainWindow.ScreenHeight / 2) - 350;
            NewNsi.ShowDialog();
        }

        public static void Selectedswitch()
        {
            switch (ActCompletedInterview)
            {
                case "Guest":
                    WindowMain.TablGuestInterviews.ItemsSource = GuestIntervs;
                    break;
                case "Pacient":
                    WindowMain.TablPacientInterviews.ItemsSource = GuestIntervs;
                    break;
                case "Likar":
                    WindowMain.TablLikarInterviews.ItemsSource = GuestIntervs;
                    break;
            }

        }


        // метод дозаписи выбранной строки жалобы в общий контекст интервью.
        public static void SelectContentCompleted()
        {
            int indexcontent = 0;
            bool booladdContent = false, addcontent = false;

            TmpGuestIntervs = new ObservableCollection<ModelCompletedInterview>();
            foreach (ModelCompletedInterview ModelCompletedInterview in GuestIntervs) //.OrderBy(x => x.kodDetailing)
            {
                indexcontent++;
                if (IdItemGuestInterv == indexcontent && selectedGuestInterv != null && addcontent==false) booladdContent = true;
                TmpGuestIntervs.Add(ModelCompletedInterview);
                if (booladdContent == true)
                { 
                    AddselectedColection();
                    addcontent = true;
                    booladdContent = false;
                } 
            }
            if (GuestIntervs.Count == TmpGuestIntervs.Count) AddselectedColection();
            GuestIntervs = TmpGuestIntervs;
            Selectedswitch();

        }

        private static void AddselectedColection()
        {
            ModelCompletedInterview selectedaddContent = new ModelCompletedInterview();
            selectedaddContent.kodDetailing = nameFeature3.Substring(0, nameFeature3.IndexOf(":"));
            selectedaddContent.detailsInterview = nameFeature3.Substring(nameFeature3.IndexOf(":") + 1, nameFeature3.Length - (nameFeature3.IndexOf(":") + 1));
            TmpGuestIntervs.Add(selectedaddContent);
            IdItemGuestInterv ++;

        }

        public static void SelectNewKodComplInteriew()
        {
            string indexcmp = "CMP.000000000001";
            CallServer.PostServer(pathcontroler, pathcontroler + "0/0", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]") == false)
            {
                var result = JsonConvert.DeserializeObject<ListModelCompletedInterview>(CmdStroka);
                if (result.ModelCompletedInterview[0].kodComplInterv != null)
                { 
                    List<ModelCompletedInterview> res = result.ModelCompletedInterview.ToList();
                    int indexdia = Convert.ToInt32(res[0].kodComplInterv.Substring(res[0].kodComplInterv.LastIndexOf(".") + 1, res[0].kodComplInterv.Length - (res[0].kodComplInterv.LastIndexOf(".") + 1)));
                    string _repl = "000000000000";
                    indexcmp = "CMP." + _repl.Substring(0, _repl.Length - indexdia.ToString().Length) + (indexdia + 1).ToString();                    
                }
            }
            modelColectionInterview.kodComplInterv = indexcmp;
        }



        // метод дозаписи данных формируемого интервью в таблицу проведенных интервью
        public static void AddInterviewProtokol()
        {
            modelColectionInterview.dateInterview = DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString();
            switch (ActCompletedInterview)
            {
                case "Likar":
                    modelColectionInterview.kodDoctor= _kodDoctor;
                    modelColectionInterview.kodPacient = "";
                    if(selectedProfilPacient!=null) modelColectionInterview.kodPacient = selectedProfilPacient.kodPacient;
                    modelColectionInterview.nameInterview = WindowMain.LikarNameInterv.Text.ToString();
                    break;
                case "Pacient":
                    modelColectionInterview.kodDoctor = "";
                    modelColectionInterview.kodPacient = _pacientProfil;
                    modelColectionInterview.nameInterview = WindowMain.PacentNameInterv.Text.ToString();
                    break;
                case "Guest":
                    modelColectionInterview.kodDoctor = "";
                    modelColectionInterview.kodPacient = "Гість";
                    modelColectionInterview.namePacient = "Гість";
                    modelColectionInterview.nameInterview = WindowMain.NameInterv.Text.ToString();
                    break;
                    
            }
            CopycolectionInterview();
            SaveInterviewProtokol();
        }

        public static void CopycolectionInterview()
        { 
            colectionInterview = new ColectionInterview();
            colectionInterview.id = modelColectionInterview.id;
            colectionInterview.kodProtokola = modelColectionInterview.kodProtokola;
            colectionInterview.kodPacient = modelColectionInterview.kodPacient;
            colectionInterview.kodDoctor = modelColectionInterview.kodDoctor;
            colectionInterview.kodComplInterv = modelColectionInterview.kodComplInterv;
            colectionInterview.dateInterview = modelColectionInterview.dateInterview;
            colectionInterview.detailsInterview = modelColectionInterview.detailsInterview;
            colectionInterview.nameInterview = modelColectionInterview.nameInterview;
            colectionInterview.resultDiagnoz = modelColectionInterview.resultDiagnoz;
            colectionInterview.dateDoctor = modelColectionInterview.dateDoctor;

        }
        public static void SaveInterviewProtokol()
        {
            string Method = colectionInterview.id !=  0 ? "PUT" : "POST";
            var json = JsonConvert.SerializeObject(colectionInterview);
            CallServer.PostServer(pathcontrolerColection, json, Method);
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.FalseServerGet();
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                colectionInterview = JsonConvert.DeserializeObject<ColectionInterview>(CallServer.ResponseFromServer);
                modelColectionInterview.id = colectionInterview.id;
            }
        }

        // команда старт создать интервью пацента
        public void MethodStartInteviewPacient()
        { 
            WindowMain.PacentNameInterv.Text = "Опитування пацієнта: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(); 

            if (_pacientProfil == "")
            {
                WarningMessageOfProfilPacient();
                return;
            }
            PacientContent = PacientContent == "" ? WindowMain.PacientKabinetInterv.Content.ToString(): PacientContent;
            WindowMain.PacientKabinetInterv.Visibility = Visibility.Hidden;
            WindowMain.StackPanelPacient.Visibility = Visibility.Hidden;
            EndDialogdali = StopDialog = false;
            IndexAddEdit = "addCommand";
            ActCompletedInterview = "Pacient";
            modelColectionInterview = new ModelColectionInterview();
            modelColectionInterview.namePacient = selectedPacientProfil.name + selectedPacientProfil.surname;
            modelColectionInterview.kodPacient = selectedPacientProfil.kodPacient;
            AutoSelectedInterview();
            EndInterview();


        }

        public void MethodEditInteviewPacient()
        {
            if (GuestIntervs.Count != 0)
            {
                IndexAddEdit = "editCommand";
                ActCompletedInterview = "Pacient";
                WindowMain.StackPanelPacient.Visibility = Visibility.Hidden;
                WindowMain.PacientKabinetInterv.Visibility = Visibility.Hidden;
                EditSelectedInterview();
                SuccessEndEditInterview();
            }

        }


        // команда сохранить интервью пацента
        public void MethodSaveInterviewPacient()
        {
            if (GuestIntervs.Count == 0) return;
            SetContent();
            if (WindowMain.StackPanelPacient.Visibility == Visibility.Visible)
            {
                WindowMain.StackPanelPacient.Visibility = Visibility.Hidden;
                WindowMain.PacientKabinetInterv.Visibility = Visibility.Hidden;
            }

            MetodSaveInterview();
            WindowMain.PacentNameInterv.Text = "";        
        }

        // команда удалить интервью пацента
        public void MethodRemoveCompletedPacient()
        { 
            int IdItemSelected = WindowMain.TablPacientInterviews.SelectedIndex;
            RemovStrIntervs(IdItemSelected);
            WindowMain.StackPanelPacient.Visibility = Visibility.Hidden;
        }

        public void MethodPrintCompletedInterviewPacient()
        {
        }

        // команда старт создать  интервью пацента которое проводит врач 
        public void MethodStartInterviewLikar()
        { 
            WindowMain.LikarNameInterv.Text = "Лікарське опитування: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            if (_kodDoctor == "")
            {
                WarningMessageOfProfilLikar();
                return;
            }
            IndexAddEdit = "addCommand";
            EndDialogdali = StopDialog = false;
            ActCompletedInterview = "Likar";
            WindowMain.InputNameProfilLikar.Visibility = Visibility.Hidden;
            WindowMain.StackPanelLikar.Visibility = Visibility.Hidden;
            LikarContent = LikarContent == "" ? WindowMain.InputNameProfilLikar.Content.ToString(): LikarContent;
            modelColectionInterview = new ModelColectionInterview();
            modelColectionInterview.nameDoctor = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - MapOpisViewModel.nameDoctor.IndexOf(":") - 1);
            modelColectionInterview.kodDoctor = _kodDoctor;

            LoadProfPacient();
            SetProfilPacient();
            if (_pacientProfil.Length != 0)
            { 
                AutoSelectedInterview();
                EndInterview();
            }
        }

        private void LoadProfPacient()
        {

            CallViewProfilLikar = "ProfilPacient";
            selectedProfilPacient = new ModelPacient();
            WinNsiPacient NewOrder = new WinNsiPacient();
            NewOrder.ShowDialog();
            CallViewProfilLikar = "";
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]"))
            {
                WarningMessageOfProfilPacient();
                return;
            }
            
        }

        public void SetProfilPacient()
        { 
        
           if (selectedProfilPacient != null)
            { 
                 _pacientProfil = selectedProfilPacient.kodPacient;
                SelectedProfilPacient = selectedProfilPacient;
                modelColectionInterview.namePacient = selectedProfilPacient.name + selectedProfilPacient.surname;
                modelColectionInterview.kodPacient = selectedProfilPacient.kodPacient;
            }
            WindowMain.LikarIntert3.Text = "";
        
        }

        // команда редактироваия интервью которое создал врач
        public void MethodEditInterviewLikar()
        { 
           if (GuestIntervs.Count != 0)
           {
                IndexAddEdit = "editCommand";
                ActCompletedInterview = "Likar";
                WindowMain.InputNameProfilLikar.Visibility = Visibility.Hidden;
                WindowMain.StackPanelLikar.Visibility = Visibility.Hidden;
                EditSelectedInterview();
                SuccessEndEditInterview();

            }       
        }

        // команда сохранения интервью пацента
        public void MethodSaveInterviewLikar()
        {
            if (GuestIntervs.Count == 0) return;
            SetContent();
            if (WindowMain.BorderCabLikar.Visibility == Visibility.Visible)
            {
                WindowMain.InputNameProfilLikar.Visibility = Visibility.Hidden;
                WindowMain.StackPanelLikar.Visibility = Visibility.Hidden;
            }
            MetodSaveInterview();
            WindowMain.LikarNameInterv.Text = "";        
        }

        // команда удаления интервью пацента
        public void MethodRemoveCompletedInterviewLikar()
        {
            if (GuestIntervs == null) return;
            int IdItemSelected = WindowMain.TablLikarInterviews.SelectedIndex;
            RemovStrIntervs(IdItemSelected);
            WindowMain.StackPanelLikar.Visibility = Visibility.Hidden;
        }
        // команда печати интервью пацента       
        public void MethodPrintCompletedInterviewLikar()
        {
            PrintCompletedInterview = true;
            MapOpisViewModel.PrintDiagnoz();
        }

        public static void SuccessEndEditInterview()
        {
            MainWindow.MessageError = "Для збереження змін і завершення опитування" + Environment.NewLine + 
            "необхідно натиснути на кнопку <Зберегти>.";
            SelectedFalseLogin(4);
        }
        public static void SuccessEndInterview()
        {

            MainWindow.MessageError = "Опитування успішно закінчено." + Environment.NewLine + "Почекайте будьласка." + Environment.NewLine +
            "Здійснюється формування попередньої діагностичної гіпотези " + Environment.NewLine + "та відповідних рекомендацій щодо подальших дій";
            SelectedFalseLogin(4);

        }
        public void SelectedDiagnozRecom()
        {
            //MainWindow.MessageError = "Почекайте будьласка." + Environment.NewLine +
            // "Здійснюється формування попереднього діагнозу " + Environment.NewLine + "та відповідних рекомендацій щодо подальших дій";
            //SelectedFalseLogin();

            var json = pathcontrolerInterview + "0/" + DiagnozRecomendaciya + "/0/0";
            CallServer.PostServer(pathcontrolerInterview, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]"))
            { 
                SelectedFalseDiagnoz();
            } 
            else
            {
                SelectInterviewDiagnoz();
                TrueNaimDiagnoz();
            }

          
        }

        public void SelectInterviewDiagnoz()
        {
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            ModelInterview Idinsert = JsonConvert.DeserializeObject<ModelInterview>(CallServer.ResponseFromServer);
            // Дозапись найденого протокола в коллекции протоколов интервью
            modelColectionInterview.kodProtokola = Idinsert.kodProtokola;
            if (colectionInterview == null) colectionInterview = new ColectionInterview();
            colectionInterview.kodProtokola = Idinsert.kodProtokola;
            string json = JsonConvert.SerializeObject(colectionInterview);
            CallServer.PostServer(pathcontrolerColection, json, "PUT");

            LoadDiagnozRecomen(Idinsert.kodProtokola);

        }

        public static void LoadDiagnozRecomen(string KodProtokola)
        {
            string json = Protocolcontroller + "0/" + KodProtokola + "/0";
            CallServer.PostServer(Protocolcontroller, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) return;
            else
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDependency Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
              
                if (Insert != null)
                {
                    json = Diagnozcontroller + Insert.kodDiagnoz.ToString() + "/0/0";
                    CallServer.PostServer(Diagnozcontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        NameDiagnoz = Insert1.nameDiagnoza;

                        json = ViewModelNsiLikar.controlerLikarGrDiagnoz + "0/" + Insert1.icdGrDiagnoz.ToString() + "/0";
                        CallServer.PostServer(ViewModelNsiLikar.controlerLikarGrDiagnoz, json, "GETID");
                        if (CallServer.ResponseFromServer.Contains("[]") == false)
                        {
                            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            ModelGrupDiagnoz insertGrDiagnoz = JsonConvert.DeserializeObject<ModelGrupDiagnoz>(CallServer.ResponseFromServer);
                            selectIcdGrDiagnoz = insertGrDiagnoz.nameGrDiagnoz;
                        }
                    }

                    json = MapOpisViewModel.Recomencontroller + Insert.kodRecommend.ToString() + "/0";
                    CallServer.PostServer(Recomencontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelRecommendation Insert2 = JsonConvert.DeserializeObject<ModelRecommendation>(CallServer.ResponseFromServer);
                        NameRecomendaciya = Insert2.contentRecommendation;

                    }
                }
            }

        }

        public void TrueNaimDiagnoz()
        {
            if (NameDiagnoz.Length != 0)
            {
                IndexAddEdit = "editCommand";
                GetidkodProtokola = modelColectionInterview.kodComplInterv + "/0";
                SelectReception();
                WinResultInterview NewResult = new WinResultInterview();
                NewResult.ShowDialog();
                
                switch (ActCompletedInterview)
                {
                    case "Likar":
                        WindowMain.StackPanelLikar.Visibility = Visibility.Visible;
                        WindowMain.InputNameProfilLikar.Visibility = Visibility.Visible;
                        break;
                    case "Pacient":
                        WindowMain.StackPanelPacient.Visibility = Visibility.Visible;
                        WindowMain.PacientKabinetInterv.Visibility = Visibility.Visible;
                        break;
                    case "Guest":
                        WindowMain.StackPanelGuest.Visibility = Visibility.Visible;
                        WindowMain.InputNameInterview.Visibility = Visibility.Visible;
                       
                        break;
                }
            }

        }

        public void SelectReception()
        { 
            switch (ActCompletedInterview)
            {
                case "Likar":

                    MethodLoadReceptionLikar();
                    ModelReceptionPatient modelReceptionPatient = new ModelReceptionPatient();
                    modelReceptionPatient.dateInterview = modelColectionInterview.dateInterview;
                    modelReceptionPatient.dateVizita = modelColectionInterview.dateInterview;
                    modelReceptionPatient.nameDiagnoz = modelColectionInterview.nameDiagnoz;
                    modelReceptionPatient.namePacient = modelColectionInterview.namePacient;
                    modelReceptionPatient.nameRecomen = modelColectionInterview.nameRecomen;
                    modelReceptionPatient.kodProtokola = modelColectionInterview.kodProtokola;
                    modelReceptionPatient.kodPacient = modelColectionInterview.kodPacient;
                    modelReceptionPatient.kodComplInterv = modelColectionInterview.kodComplInterv;
                    modelReceptionPatient.topictVizita = modelColectionInterview.resultDiagnoz;
                    if (ViewModelReceptionPatients == null) ViewModelReceptionPatients = new ObservableCollection<ModelReceptionPatient>();
                    ViewModelReceptionPatients.Add(modelReceptionPatient);
                    WindowReceptionPacient.ReceptionPacientTablGrid.ItemsSource = ViewModelReceptionPatients;
                    SelectedReceptionPacient = modelReceptionPatient;

                    if (selectedReceptionPacient == null) selectedReceptionPacient = new AdmissionPatient();
                    selectedReceptionPacient.kodPacient = modelColectionInterview.kodPacient;
                    selectedReceptionPacient.kodDoctor = modelColectionInterview.kodDoctor;
                    selectedReceptionPacient.kodComplInterv = modelColectionInterview.kodComplInterv;
                    selectedReceptionPacient.kodProtokola = modelColectionInterview.kodProtokola;
                    selectedReceptionPacient.topictVizita = modelColectionInterview.resultDiagnoz;
                    selectedReceptionPacient.dateInterview = modelColectionInterview.dateInterview;
                    if (ViewReceptionPacients == null) ViewReceptionPacients = new ObservableCollection<AdmissionPatient>();
                    ViewReceptionPacients.Add(selectedReceptionPacient);
                    IndexAddEdit = "addCommand";

                    break;
                case "Pacient":

                    // записать заявку на прием к врачу
                    SelectedColectionIntevLikar = modelColectionInterview;

                    // дописать в проведенные интервью
                    if (ColectionInterviewIntevPacients == null) ColectionInterviewIntevPacients = new ObservableCollection<ModelColectionInterview>(); ;
                    ColectionInterviewIntevPacients.Add(modelColectionInterview);
                    SelectedColectionIntevPacient = modelColectionInterview;
                    SelectedColectionReceptionPatient = modelColectionInterview;
                    break;
                case "Guest":

                    SelectReceptionLIkarGuest = modelColectionInterview;
                    break;
            }
            SelectIcdGrDiagnoz();
            ChangeContentCompletedInterview();
        }

        public void ChangeContentCompletedInterview()
        {
            // Очистка коллекции от двойных  кодов
            string json = pathcontroler + modelColectionInterview.kodComplInterv + "/0";
            CallServer.PostServer(pathcontroler, json, "DELETE");

            CallServer.PostServer(pathcontrolerContent, pathcontrolerContent + modelColectionInterview.kodProtokola, "GETID");
            string CmdStroka = CallServer.ServerReturn();

            if (CmdStroka.Contains("[]")) ViewModelCreatInterview.selectedContentInterv = new ModelContentInterv();
            else ViewModelCreatInterview.ObservableContentInterv(CmdStroka);

            foreach (ModelContentInterv modelContentInterv in ViewModelCreatInterview.ContentIntervs)
            {
                ModelCompletedInterview modelCompletedInterview = new ModelCompletedInterview();
                modelCompletedInterview.id = 0;
                modelCompletedInterview.numberstr = modelContentInterv.numberstr;
                modelCompletedInterview.kodComplInterv = modelColectionInterview.kodComplInterv;
                modelCompletedInterview.kodDetailing = modelContentInterv.kodDetailing;
                modelCompletedInterview.detailsInterview = modelContentInterv.detailsInterview;

                json = JsonConvert.SerializeObject(modelCompletedInterview);
                CallServer.PostServer(pathcontroler, json, "POST");
            }

        }
        public void SelectIcdGrDiagnoz()
        {
            string json = Protocolcontroller + "0/" + modelColectionInterview.kodProtokola + "/0";
            CallServer.PostServer(MapOpisViewModel.Protocolcontroller, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) return;
            else
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelDependency Insert = JsonConvert.DeserializeObject<ModelDependency>(CallServer.ResponseFromServer);
                if (Insert != null)
                {
                    json = Diagnozcontroller + Insert.kodDiagnoz.ToString() + "/0/0";
                    CallServer.PostServer(Diagnozcontroller, json, "GETID");
                    if (CallServer.ResponseFromServer.Contains("[]") == false)
                    {
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelDiagnoz Insert1 = JsonConvert.DeserializeObject<ModelDiagnoz>(CallServer.ResponseFromServer);
                        selectIcdGrDiagnoz = Insert1.icdGrDiagnoz.ToString();
                        json = ViewModelNsiLikar.controlerLikarGrDiagnoz + "0/" + Insert1.icdGrDiagnoz.ToString() + "/0";
                        CallServer.PostServer(ViewModelNsiLikar.controlerLikarGrDiagnoz, json, "GETID");
                        if (CallServer.ResponseFromServer.Contains("[]") == false)
                        {
                            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            ModelGrupDiagnoz insertGrDiagnoz = JsonConvert.DeserializeObject<ModelGrupDiagnoz>(CallServer.ResponseFromServer);
                            selectIcdGrDiagnoz = insertGrDiagnoz.icdGrDiagnoz;
                        }
                    }
                }
            }
        }
        public  void SelectedFalseDiagnoz()
        {

            string json = "", CmdStroka = "";
            while (DiagnozRecomendaciya.Contains(";") == true)
            {
                json = pathcontrolerInterview + "0/" + DiagnozRecomendaciya + "/-1/0";
                CallServer.PostServer(pathcontrolerInterview, json, "GETID");
                CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]") == false) { break; }
                DiagnozRecomendaciya = DiagnozRecomendaciya.Substring(0, DiagnozRecomendaciya.Length - 1);
                DiagnozRecomendaciya = DiagnozRecomendaciya.Substring(0, DiagnozRecomendaciya.LastIndexOf(";") + 1);
            }

            if (CmdStroka.Contains("[]") == true)
            {
                MainWindow.MessageError = "Увага!" + Environment.NewLine +
                "за результатами проведеного опитування відповідний діагноз відсутній.";
                MapOpisViewModel.SelectedFalseLogin(4);
                return;
            }
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "За результатами вашого опитування в базі знань знайдені " + Environment.NewLine +
            "визначення попереднього діагнозу. " ; 
            WinDeleteData NewOrder = new WinDeleteData(MainWindow.MessageError);
            NewOrder.Height = NewOrder.Height + 50;
            NewOrder.grid2.Height = NewOrder.grid2.Height + 20;
            NewOrder.MessageText.Height= NewOrder.MessageText.Height + 10;
            NewOrder.BorderYes.Margin = new Thickness(20, 0, 0, 0);
            NewOrder.BorderYes.Width = NewOrder.BorderYes.Width + 40;
            NewOrder.Yes.Width = NewOrder.Yes.Width + 40;
            NewOrder.Yes.Height = NewOrder.Yes.Height + 6;
            NewOrder.Yes.FontSize = 15;
            NewOrder.Yes.Content = "Переглянути";
            NewOrder.BorderNo.Margin = new Thickness(0, 0, 20, 0);
            NewOrder.BorderNo.Width = NewOrder.BorderNo.Width + 120;
            NewOrder.No.Width = NewOrder.No.Width + 120;
            NewOrder.No.Height = NewOrder.No.Height + 6;
            NewOrder.No.FontSize = 15;
            NewOrder.No.Content = "Продовжити опитування";
            NewOrder.Left = 250;
            NewOrder.Top = 100;
            NewOrder.ShowDialog();
            if (MapOpisViewModel.DeleteOnOff == true)
            {
                var result = JsonConvert.DeserializeObject<ListModelInterview>(CmdStroka);
                List<ModelInterview> res = result.ModelInterview.ToList();
                AnalogInterviews = new ObservableCollection<ModelInterview>((IEnumerable<ModelInterview>)res);
                WinAnalogDiagnoz NewResult = new WinAnalogDiagnoz();
                NewResult.ShowDialog();

                //if (ViewAnalogDiagnoz == true)SelectReception();
                if (SaveAnalogDiagnoz == true || ViewAnalogDiagnoz == true)
                {
                    SetContent();
                    switch (ActCompletedInterview)
                    {
                        case "Likar":
                            WindowMain.StackPanelLikar.Visibility = Visibility.Visible;
                            WindowMain.InputNameProfilLikar.Visibility = Visibility.Visible;
                            break;
                        case "Pacient":
                            WindowMain.StackPanelPacient.Visibility = Visibility.Visible;
                            WindowMain.PacientKabinetInterv.Visibility = Visibility.Visible;
                            break;
                        case "Guest":
                            WindowMain.InputNameInterview.Visibility = Visibility.Visible;
                            WindowMain.StackPanelGuest.Visibility = Visibility.Visible;
                            break;
                    }
                }
            }
            else
            {
                ContinueCompletedInterview();

            }
        }

        public static void SetContent()
        {
            switch (ActCompletedInterview)
            {
                case "Likar":
                    if (LikarContent.Length > 0) WindowMain.InputNameProfilLikar.Content = LikarContent;
                    WindowMain.InputNameProfilLikar.Height = 282;
                    WindowMain.InputNameProfilLikar.Foreground = Brushes.Black;
                    WindowMain.StackPanelLikar.Height = 300;
                    WindowMain.StackPanelLikar.Margin = new Thickness(0, 0, 0, 0);
                    WindowMain.BorderLikar.Height = 300;
                    WindowMain.BorderLikar.Margin = new Thickness(0, 0, 0, 0);
  
                    break;
                case "Pacient":
                    if (PacientContent.Length > 0) WindowMain.PacientKabinetInterv.Content = PacientContent;
                    WindowMain.PacientKabinetInterv.Height = 282;
                    WindowMain.PacientKabinetInterv.Foreground = Brushes.Black;
                    WindowMain.StackPanelPacient.Height = 300;
                    WindowMain.StackPanelPacient.Margin = new Thickness(0, 0, 0, 0);
                    WindowMain.BorderPacient.Height = 300;
                    WindowMain.BorderPacient.Margin = new Thickness(0, 0, 0, 0);
                    break;
                case "Guest":
                    if (InputContent.Length > 0) WindowMain.InputNameInterview.Content = InputContent;
                    WindowMain.InputNameInterview.Height = 282;
                    WindowMain.InputNameInterview.Foreground = Brushes.Black;
                    WindowMain.StackPanelGuest.Height = 300;
                    WindowMain.StackPanelGuest.Margin = new Thickness(0, 0, 0, 0);
                    WindowMain.BorderGuest.Height = 300;//182
                    WindowMain.BorderGuest.Margin = new Thickness(0, 0, 0, 0);
                    break;
            }

        }

        public static void ContinueCompletedInterview()
        {
            GuestIntervs = TmpGuestIntervs;
            string ContentMasage = Environment.NewLine + "Для продовження деталізації опитування та редагування необхідно" + Environment.NewLine +
                    "     натиснути кнопку 'Додати' або  встановити курсор миши " + Environment.NewLine + " на обраний рядок опитування та натиснути кнопку 'Змінити'. ";
            switch (ActCompletedInterview)
            {
                case "Likar":
                    WindowMain.TablLikarInterviews.ItemsSource = GuestIntervs;
                    WindowMain.InputNameProfilLikar.Content = ContentMasage;
                    WindowMain.InputNameProfilLikar.Height = 100;
                    WindowMain.InputNameProfilLikar.Foreground = Brushes.Blue;
                    WindowMain.StackPanelLikar.Height = 100;
                    WindowMain.StackPanelLikar.Margin = new Thickness(0, 0, 0, -180);
                    WindowMain.BorderLikar.Height = 100;
                    WindowMain.BorderLikar.Margin = new Thickness(0, 0, 0, -180);
                    WindowMain.StackPanelLikar.Visibility = Visibility.Visible;
                    WindowMain.InputNameProfilLikar.Visibility = Visibility.Visible;

                    break;
                case "Pacient":
                    WindowMain.TablPacientInterviews.ItemsSource = GuestIntervs;
                    WindowMain.PacientKabinetInterv.Content = ContentMasage;
                    WindowMain.PacientKabinetInterv.Height = 100;
                    WindowMain.PacientKabinetInterv.Foreground = Brushes.Blue;
                    WindowMain.StackPanelPacient.Height = 100;
                    WindowMain.StackPanelPacient.Margin = new Thickness(0, 0, 0, -220);
                    WindowMain.BorderPacient.Height = 100;
                    WindowMain.BorderPacient.Margin = new Thickness(0, 0, 0, -220);
                    WindowMain.StackPanelPacient.Visibility = Visibility.Visible;
                    WindowMain.PacientKabinetInterv.Visibility = Visibility.Visible;

                    break;
                case "Guest":
                    WindowMain.TablGuestInterviews.ItemsSource = GuestIntervs;
                    WindowMain.InputNameInterview.Content = ContentMasage;
                    WindowMain.InputNameInterview.Height = 100;
                    WindowMain.InputNameInterview.Foreground = Brushes.Blue;
                    WindowMain.StackPanelGuest.Height = 100;
                    WindowMain.StackPanelGuest.Margin = new Thickness(0, 0, 0, -220);
                    WindowMain.BorderGuest.Height = 100;
                    WindowMain.BorderGuest.Margin = new Thickness(0, 0, 0, -220);
                    WindowMain.StackPanelGuest.Visibility = Visibility.Visible;
                    WindowMain.InputNameInterview.Visibility = Visibility.Visible;
                    break;
            }

        }

        public static void UnloadCmdStroka(string model = "", string CmdStroka = "")
        {

            CallServer.PostServer(Controlleroutfile, Controlleroutfile + model + CmdStroka + "/0", "GETID");
            if (CmdStroka == "0") upLoadstroka = CallServer.ResponseFromServer;
            CmdStroka = CallServer.ResponseFromServer;

        }

    }
}

