using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {

        // ViewModelProfilLikar Справочник докторов
        // клавиша в окне:  Профіль лікаря в кабінеті лікаря

        #region Обработка событий и команд вставки, удаления и редектирования Профіль лікаря
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех карточек описания доктора из БД
        /// через механизм REST.API
        /// </summary>      
        public static MainWindow WindowProfilDoctor = MainWindow.LinkNameWindow("WindowMain");
        public static bool  saveboolAccountLikar = false, boolVisibleMessage = false;
        public static bool editboolProfilLikar = false, addboolProfilLikar = false;
        public static string CallViewProfilLikar = "ProfilLikar";
        public static string _kodDoctor = "";
        public static string pathcontrolerProfilLikar = "/api/ApiControllerDoctor/";
        public static string pathcontrolerMedZakladProfilLikar = "/api/MedicalInstitutionController/";
        public static ModelDoctor selectedProfilLikar;
        private static ModelGridDoctor selectedGridProfilLikar;
        private static ModelGridDoctor selectedGridDoctor;

        public static ObservableCollection<ModelDoctor> ViewProfilLikars { get; set; }
        public static ObservableCollection<ModelGridDoctor> ViewGridProfilLikars { get; set; }
        public static ObservableCollection<ModelDoctor> ViewDoctors { get; set; }
        public ModelDoctor SelectedProfilLikar
        {
            get { return selectedProfilLikar; }
            set { selectedProfilLikar = value; OnPropertyChanged("SelectedProfilLikar"); }
        }

        public ModelGridDoctor SelectedGridProfilLikar
        {
            get { return selectedGridProfilLikar; }
            set { selectedGridProfilLikar = value; OnPropertyChanged("SelectedGridProfilLikar"); }
        }
        public static void ObservableViewProfilLikars(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDoctor>(CmdStroka);
            List<ModelDoctor> res = result.ModelDoctor.ToList();
            ViewDoctors = new ObservableCollection<ModelDoctor>((IEnumerable<ModelDoctor>)res);
            IndexAddEdit = "";
            selectedProfilLikar = ViewDoctors[0];
            MetodLoadGridProfilLikar();
        }


        #endregion

        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника 
        /// </summary>
        /// 
        // загрузка справочника по нажатию клавиши Завантажити
        public void MethodloadProfilLikar()
        {
            MainWindow WindowPacientProfil = MainWindow.LinkNameWindow("WindowMain");
            selectedProfilLikar = new ModelDoctor();
            WindowProfilDoctor.LikarLoadInf.Visibility = Visibility.Hidden;
            WindowProfilDoctor.LikarLoadinterv.Visibility = Visibility.Hidden;
            WindowMain.BorderCabLikar.Visibility = Visibility.Hidden;
            if (boolSetAccountUser == false && loadboolProfilLikar == false && loadboolPacientProfil == false)
            {
                if (RegSetAccountUser() == true)
                {
                    if (ViewDoctors != null)
                    {
                        _kodDoctor = selectedProfilLikar.kodDoctor;
                        SetValueLikarProfil();
                        WindowProfilDoctor.LikarLoadInf.Visibility = Visibility.Hidden;
                        if (WindowProfilDoctor.LikarUrit7.Text.Length > 0) WindowProfilDoctor.FolderDocUri5.Visibility = Visibility.Visible;
                    }
                    if (CallViewProfilLikar == "Admin") MetodSelectRegProfilLikar();
                    if (CallViewProfilLikar == "PacientProfil")
                    {
                        LoadMessageError();
                        return;
                    }
                }
            }
            else
            { 
               if (loadboolPacientProfil == true && boolSetAccountUser == false)
               {
                    PacientProfilMessageError();
                    return;
               }
               if (boolSetAccountUser == true) MetodSelectRegProfilLikar();            
            }
 
        }

        private void MetodSelectRegProfilLikar()
        {
            SelectRegProfilLikar();
            if (selectedProfilLikar.id == 0) return;
            _kodDoctor = ViewModelNsiLikar.selectedLikar.kodDoctor;
            selectedProfilLikar = ViewModelNsiLikar.selectedLikar;
            MetodLoadGridProfilLikar();
            SetValueLikarProfil();
            loadboolProfilLikar = true;
        }

        public void SetValueLikarProfil()
        {

            LoadInfoPacient("лікаря.");
            addboolProfilLikar = true;
            selectedGridProfilLikar = ViewGridProfilLikars[0];
            SelectedGridProfilLikar = selectedGridProfilLikar;
            WindowProfilDoctor.LikarIntert2.Text = selectedGridProfilLikar.name + " " + selectedGridProfilLikar.surname + " " + selectedGridProfilLikar.specialnoct + " " + selectedGridProfilLikar.telefon;
            WindowProfilDoctor.LikarNameInterv.Text = "Лікарське опитування: ";
            WindowProfilDoctor.ReceptionPacient2.Text = selectedGridProfilLikar.name + " " + selectedGridProfilLikar.surname + " " + selectedGridProfilLikar.specialnoct + " " + selectedGridProfilLikar.telefon;

            WindowProfilDoctor.LikarInterviewAvtort7.Text = "";
            ViewReceptionPacients = new ObservableCollection<AdmissionPatient>();
            WindowProfilDoctor.ReceptionPacientTablGrid.ItemsSource = ViewReceptionPacients;
            WindowProfilDoctor.NameMedZaklad.Text = WindowIntevLikar.Likart9.Text;
            WindowProfilDoctor.ReseptionLikar.Text = selectedGridProfilLikar.name + " " + selectedGridProfilLikar.surname + " " + selectedGridProfilLikar.specialnoct + " " + selectedGridProfilLikar.telefon;
            ViewVisitingDays = new ObservableCollection<ModelVisitingDays>();
            WindowProfilDoctor.ReseptionPacientTablGrid.ItemsSource = ViewVisitingDays;
            ColectionInterviewIntevLikars = new ObservableCollection<ModelColectionInterview>();
            WindowProfilDoctor.ColectionIntevLikarTablGrid.ItemsSource = ColectionInterviewIntevLikars;

            boolVisibleMessage = true;
            MethodLoadtableColectionIntevLikar();
            MethodLoadReceptionLikar();
            MethodLoadVisitingDays();
            MethodViewWorkDiagnoz();
            MethodViewLIbDiagnoz();
            MessageWarning Info = MainWindow.LinkNameWindow("WarningMessage");
            if (Info != null) Info.Close();
            boolVisibleMessage = false;
        }

        private void SelectRegProfilLikar()
        {

            WinNsiMedZaklad MedZaklad = new WinNsiMedZaklad();
            MedZaklad.ShowDialog();
            EdrpouMedZaklad = ReceptionLIkarGuest.Likart8.Text.ToString();

            if (EdrpouMedZaklad != "")
            {
                CallViewProfilLikar = "ProfilLikar";
            
                selectedProfilLikar = new ModelDoctor();
                WinNsiLikar NewOrder = new WinNsiLikar();
                NewOrder.ShowDialog();
                CallViewProfilLikar = "";
                if (MapOpisViewModel.nameDoctor != "")
                {
                    if (modelColectionInterview == null) modelColectionInterview = new ModelColectionInterview();

                    modelColectionInterview.nameDoctor = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - MapOpisViewModel.nameDoctor.IndexOf(":") - 1);
                    modelColectionInterview.kodDoctor = MapOpisViewModel.nameDoctor.Substring(0, MapOpisViewModel.nameDoctor.IndexOf(":"));
                    WindowIntevLikar.ReceptionLikar2.Text = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - MapOpisViewModel.nameDoctor.IndexOf(":") - 1);

                    ViewReceptionPacients = new ObservableCollection<AdmissionPatient>();
                    WindowIntevLikar.ReceptionPacientTablGrid.ItemsSource = ViewReceptionPacients;
                    WindowIntevLikar.NameMedZaklad.Text = WindowIntevLikar.Likart9.Text;
                    WindowIntevLikar.ReseptionLikar.Text = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - MapOpisViewModel.nameDoctor.IndexOf(":") - 1); ;
                    ViewVisitingDays = new ObservableCollection<ModelVisitingDays>();
                    WindowIntevLikar.ReseptionPacientTablGrid.ItemsSource = ViewVisitingDays;
                    ColectionInterviewIntevLikars = new ObservableCollection<ModelColectionInterview>();
                    WindowIntevLikar.ColectionIntevLikarTablGrid.ItemsSource = ColectionInterviewIntevLikars;

                }

            }
        }


        public static void MetodLoadGridProfilLikar()
        {
            if (selectedProfilLikar != null)
            {
                if (selectedProfilLikar.id != 0)
                {
                    ViewProfilLikars = new ObservableCollection<ModelDoctor>();
                    ViewProfilLikars.Add(selectedProfilLikar);
                    ViewGridProfilLikars = new ObservableCollection<ModelGridDoctor>();
                    foreach (ModelDoctor modelDoctor in ViewProfilLikars)
                    {
                        selectedGridProfilLikar = new ModelGridDoctor();
                        selectedGridProfilLikar.kodDoctor = modelDoctor.kodDoctor;
                        selectedGridProfilLikar.id = modelDoctor.id;
                        selectedGridProfilLikar.name = modelDoctor.name;
                        selectedGridProfilLikar.surname = modelDoctor.surname;
                        selectedGridProfilLikar.specialnoct = modelDoctor.specialnoct;
                        selectedGridProfilLikar.telefon = modelDoctor.telefon;
                        selectedGridProfilLikar.email = modelDoctor.email;
                        selectedGridProfilLikar.edrpou = modelDoctor.edrpou;
                        selectedGridProfilLikar.uriwebDoctor = modelDoctor.uriwebDoctor;
                        selectedGridProfilLikar.napryamok = modelDoctor.napryamok;
                        if (modelDoctor.edrpou != null)
                        {
                            string json = pathcontrolerMedZakladProfilLikar + modelDoctor.edrpou.ToString()+"/0/0";
                            CallServer.PostServer(pathcontrolerMedZakladProfilLikar, json, "GETID");
                            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            MedicalInstitution Idinsert = JsonConvert.DeserializeObject<MedicalInstitution>(CallServer.ResponseFromServer);
                            if (Idinsert != null)
                            {
                                selectedGridProfilLikar.nameZaklad = Idinsert.name;
                                selectedGridProfilLikar.adrZaklad = Idinsert.adres;
                                selectedGridProfilLikar.pind = Idinsert.postIndex;
                            }
                        }
                        MapOpisViewModel.nameDoctor = selectedGridProfilLikar.kodDoctor.ToString() + ": " + selectedGridProfilLikar.name.ToString() + " " + selectedGridProfilLikar.surname.ToString() + " " + selectedGridProfilLikar.telefon.ToString();
                        ViewGridProfilLikars.Add(selectedGridProfilLikar);
                    }
                }

            }

        }


        // Метод введення нового профілю лікаря до коллекції лікарів
        public void MethodAddNewProfilLIkar()
        {
            selectedProfilLikar = new ModelDoctor();
            CallViewProfilLikar = "ProfilLikar";
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            selectedGridProfilLikar = new ModelGridDoctor();
            ViewDoctors = new ObservableCollection<ModelDoctor>();
            WindowMain.BorderCabLikar.Visibility = Visibility.Hidden;
            if (loadboolProfilLikar == true)
            {
                addboolProfilLikar = false;
                WarningMessageNewProfilLikar();
                return;
            }
            if (addboolProfilLikar == false)
            {
                  
                BoolTrueProfilLikar();
                WindowMain.FoldMedZaklad.Visibility = Visibility.Visible;

            }
            else { BoolFalseProfilLikar(); }

        }

        //// Метод введення нового дозапису в чергу прийомів пацієнтів самим лікарем
        //public void MethodAddNewReceptionPacient()
        //{

        //    LoadProfPacient();

        //}

        public static void BoolTrueProfilLikar()
        {

            editboolProfilLikar = true;
            addboolProfilLikar = true;
            WindowProfilDoctor.Likart10.IsEnabled = true;
            WindowProfilDoctor.Likart10.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.Likart2.IsEnabled = true;
            WindowProfilDoctor.Likart2.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.Likart3.IsEnabled = true;
            WindowProfilDoctor.Likart3.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.Likart5.IsEnabled = true;
            WindowProfilDoctor.Likart5.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.Likart6.IsEnabled = true;
            WindowProfilDoctor.Likart6.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.Likart7.IsEnabled = true;
            WindowProfilDoctor.Likart7.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.LikarNaprt3.IsEnabled = true;
            WindowProfilDoctor.LikarNaprt3.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.LikarUrit7.IsEnabled = true;
            WindowProfilDoctor.LikarUrit7.Background = Brushes.AntiqueWhite;
            WindowProfilDoctor.LikarLoadInf.Content = "Для збреження профілю необхідно натиснути на кнопку 'Зберегти'";
            if (ViewModelRegisterAccountUser.ReestrOnOff == true)WindowProfilDoctor.FoldMedZaklad.Visibility = Visibility.Visible;
            if (WindowProfilDoctor.LikarUrit7.Text.Length > 0)WindowProfilDoctor.FolderDocUri5.Visibility = Visibility.Visible;

        }

        private void BoolFalseProfilLikar()
        {

            editboolProfilLikar = false;
            addboolProfilLikar = false;
            WindowProfilDoctor.Likart10.IsEnabled = false;
            WindowProfilDoctor.Likart10.Background = Brushes.White;
            WindowProfilDoctor.Likart2.IsEnabled = false;
            WindowProfilDoctor.Likart2.Background = Brushes.White;
            WindowProfilDoctor.Likart3.IsEnabled = false;
            WindowProfilDoctor.Likart3.Background = Brushes.White;
            WindowProfilDoctor.Likart5.IsEnabled = false;
            WindowProfilDoctor.Likart5.Background = Brushes.White;
            WindowProfilDoctor.Likart6.IsEnabled = false;
            WindowProfilDoctor.Likart6.Background = Brushes.White;
            WindowProfilDoctor.Likart7.IsEnabled = false;
            WindowProfilDoctor.Likart7.Background = Brushes.White;
            WindowProfilDoctor.LikarNaprt3.IsEnabled = false;
            WindowProfilDoctor.LikarNaprt3.Background = Brushes.White;
            WindowProfilDoctor.LikarUrit7.IsEnabled = false;
            WindowProfilDoctor.LikarUrit7.Background = Brushes.White;
            WindowProfilDoctor.FoldMedZaklad.Visibility = Visibility.Hidden;
            if (ViewModelRegisterAccountUser.ReestrOnOff == false)
            {
                WindowMain.BorderCabLikar.Visibility = Visibility.Visible;
            }
            WindowProfilDoctor.FolderDocUri5.Visibility = Visibility.Hidden;
        }

        // команда  редактировать
        public void MethodEditProfilLikar()
        {
            if (selectedGridProfilLikar != null)
            { 
                 IndexAddEdit = "editCommand";
                if (editboolProfilLikar == false)
                {
                    BoolTrueProfilLikar();
                }
                else
                {
                    BoolFalseProfilLikar();
                    IndexAddEdit = "";
                }           
            }


        }

        public void MethodRemoveProfilLikar()
        {

            
           
            // зчитати профіль пацієнт в колекціїї опитувань
            string json = pathcontrolerColection + "0/" + selectedGridProfilLikar.kodDoctor.ToString()+ "/0";
            CallServer.PostServer(pathcontrolerColection, json, "GETID");
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            ColectionInterview Idinsert = JsonConvert.DeserializeObject<ColectionInterview>(CallServer.ResponseFromServer);
            if (Idinsert != null)
            {
                // проведені опитування заголовок
                json = pathcontrolerColection + "0/0/" + selectedGridProfilLikar.kodDoctor.ToString() ;
                CallServer.PostServer(pathcontrolerColection, json, "DELETE");
                // проведені опитування зміст
                CallServer.PostServer(pathcontroler, pathcontroler + Idinsert.kodComplInterv + "/0", "DELETE");
            } 
            // запис на прийом до лікаря
            json = pathcontrolerReceptionPacient + "0/" + selectedGridProfilLikar.kodDoctor.ToString();
            CallServer.PostServer(pathcontrolerReceptionPacient, json, "DELETE");
            // ОБліковий запис
            json = pathcontrolerAccountUser + "0/" + selectedGridProfilLikar.kodDoctor.ToString();
            CallServer.PostServer(pathcontrolerAccountUser, json, "DELETE");
            // профіль пацієнта
            json = pathcontrolerProfilLikar + selectedGridProfilLikar.id.ToString();
            CallServer.PostServer(pathcontrolerProfilLikar, json, "DELETE");


            selectedGridProfilLikar = new ModelGridDoctor();
            SelectedGridProfilLikar = new ModelGridDoctor();
            WindowProfilDoctor.Likart10.Text = "";
            WindowProfilDoctor.Likart2.Text = "";
            WindowProfilDoctor.Likart3.Text = "";
            WindowProfilDoctor.Likart5.Text = "";
            WindowProfilDoctor.Likart6.Text = "";
            WindowProfilDoctor.Likart7.Text = "";
            WindowProfilDoctor.Likart4.Text = "";
            WindowProfilDoctor.Likart9.Text = "";
            WindowProfilDoctor.LikarNaprt3.Text = "";
            WindowProfilDoctor.LikarUrit7.Text = "";
            ExitCabinetLikar();
            IndexAddEdit = "";
        }

        // команда сохранить редактирование
        public void MethodSaveProfilLikar()
        {

            if (WindowProfilDoctor.Likart10.Text.Length != 0 && WindowProfilDoctor.Likart2.Text.Length != 0
                 && WindowProfilDoctor.Likart4.Text.Length != 0 && WindowProfilDoctor.Likart9.Text.Length != 0)
            {
                if (WindowProfilDoctor.Likart6.Text.Length > 9)
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine + "Порушено формат номеру телефону." + Environment.NewLine + "Кількість цифр у номері телефону перевищує дев'ять."
                        + Environment.NewLine + "Необхідно зменьшити кількість цифр у номері телефону.";
                    MapOpisViewModel.SelectedFalseLogin(8);
                    return;
                }
                
                ViewModelRegisterAccountUser.ReestrOnOff = true;
                SelectProfilLikar();

                if (IndexAddEdit == "addCommand")
                {
                    string json = JsonConvert.SerializeObject(selectedProfilLikar);
                    CallServer.PostServer(pathcontrolerProfilLikar, json, "POST");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    ModelDoctor Idinsert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                    if (ViewDoctors == null)
                    {
                        ViewDoctors = new ObservableCollection<ModelDoctor>();
                        ViewDoctors.Add(Idinsert);
                    }
                    else { ViewDoctors.Insert(ViewDoctors.Count, Idinsert); }
                    selectedProfilLikar = Idinsert;

                }
                else
                {
                    string json = JsonConvert.SerializeObject(selectedProfilLikar);
                    CallServer.PostServer(pathcontrolerProfilLikar, json, "PUT");
                }
                _kodDoctor = selectedProfilLikar.kodDoctor;
                MetodLoadGridProfilLikar();
                SelectedGridProfilLikar = selectedGridProfilLikar;
                WindowProfilDoctor.LikarNameInterv.Text = "Гіпотеза ";
                WindowProfilDoctor.ReceptionPacient2.Text = selectedGridProfilLikar.name + " " + selectedGridProfilLikar.surname + " " + selectedGridProfilLikar.specialnoct + " " + selectedGridProfilLikar.telefon;
                WindowProfilDoctor.LikarLoadInf.Visibility = Visibility.Hidden;
            }
            else
            { 
               if (WindowProfilDoctor.Likart4.Text.Length == 0 && WindowProfilDoctor.Likart9.Text.Length == 0)
                {

                    MainWindow.MessageError = "Увага!" + Environment.NewLine +" Не вказано медзаклад де працює лікар.";
                    MapOpisViewModel.SelectedFalseLogin(10);
                }            
            }
 
            IndexAddEdit = "";
            BoolFalseProfilLikar();
        }
        public void SelectProfilLikar()
        {

            selectedProfilLikar.name = WindowProfilDoctor.Likart10.Text.ToString();
            selectedProfilLikar.surname = WindowProfilDoctor.Likart2.Text.ToString();
            selectedProfilLikar.specialnoct = WindowProfilDoctor.Likart3.Text.ToString();
            selectedProfilLikar.edrpou = WindowProfilDoctor.Likart8.Text.ToString();
            selectedProfilLikar.telefon = WindowProfilDoctor.Likart6.Text.ToString();
            selectedProfilLikar.email = WindowProfilDoctor.Likart7.Text.ToString();
            selectedProfilLikar.napryamok = WindowProfilDoctor.LikarNaprt3.Text.ToString();
            selectedProfilLikar.uriwebDoctor = WindowProfilDoctor.LikarUrit7.Text.ToString();

            if (ViewModelRegisterAccountUser.ReestrOnOff == true)
            {
                NewProfilLikar();
                selectedProfilLikar.id = 0;
                NewAccountRecords();

            }


            selectedGridDoctor = new ModelGridDoctor();
            selectedGridDoctor.kodDoctor = selectedProfilLikar.kodDoctor;
            selectedGridDoctor.id = selectedProfilLikar.id; 
            selectedGridDoctor.name = selectedProfilLikar.name;
            selectedGridDoctor.surname = selectedProfilLikar.surname;
            selectedGridDoctor.specialnoct = selectedProfilLikar.specialnoct;
            selectedGridDoctor.telefon = selectedProfilLikar.telefon;
            selectedGridDoctor.email = selectedProfilLikar.email;
            selectedGridDoctor.edrpou = selectedProfilLikar.edrpou;
            selectedGridDoctor.nameZaklad = WindowProfilDoctor.Likart9.Text.ToString();
            selectedGridDoctor.pind = WindowProfilDoctor.Likart5.Text.ToString();
            selectedGridDoctor.adrZaklad = WindowProfilDoctor.Likart4.Text.ToString();
            selectedGridDoctor.uriwebDoctor = selectedProfilLikar.uriwebDoctor;
            selectedGridDoctor.napryamok = selectedProfilLikar.napryamok;
        }

        private void NewProfilLikar()
        {
            CallServer.PostServer(pathcontrolerProfilLikar, pathcontrolerProfilLikar , "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) BeginProfilLikar();
            else 
            {
                ObservableProfilLikars(CmdStroka);
                if (ViewDoctors.Count == 0) BeginProfilLikar();
                else
                {
                    int _keyDoctorindex = 0, setindex = 0;
                    _keyDoctorindex = Convert.ToInt32(ViewDoctors[0].kodDoctor.Substring(ViewDoctors[0].kodDoctor.LastIndexOf(".") + 1, ViewDoctors[0].kodDoctor.Length - (ViewDoctors[0].kodDoctor.LastIndexOf(".") + 1)));
                    for (int i = 0; i < ViewDoctors.Count; i++)
                    {
                        setindex = Convert.ToInt32(ViewDoctors[i].kodDoctor.Substring(ViewDoctors[i].kodDoctor.LastIndexOf(".") + 1, ViewDoctors[i].kodDoctor.Length - (ViewDoctors[i].kodDoctor.LastIndexOf(".") + 1)));
                        if (_keyDoctorindex < setindex) _keyDoctorindex = setindex;
                    }
                    _keyDoctorindex++;
                    string _repl = "000000000";
                    selectedProfilLikar.kodDoctor = "DTR." + _repl.Substring(0, _repl.Length - _keyDoctorindex.ToString().Length) + _keyDoctorindex.ToString();
                }          
            }
        }

        private void BeginProfilLikar()
        { 
            selectedProfilLikar.kodDoctor = "DTR.000000001";
        }

        private void ObservableProfilLikars(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDoctor>(CmdStroka);
            List<ModelDoctor> res = result.ModelDoctor.ToList();
            ViewDoctors = new ObservableCollection<ModelDoctor>((IEnumerable<ModelDoctor>)res);
        }

        public static void NewAccountRecords()
        {
            saveboolAccountLikar = false;
            WinAccountRecords NewOrder = new WinAccountRecords();
            NewOrder.ShowDialog();
        }

        // команда печати
        public void MethodPrintProfilLikar()
        { 
            if (selectedGridProfilLikar != null)
            {
                MessageBox.Show("Ім'я та прізвище лікаря :" + selectedGridProfilLikar.name.ToString() + " " + selectedGridDoctor.surname.ToString());
            }       
        }

        
              RelayCommand? addMedzaklad;
        public RelayCommand AddMedzaklad
        {
            get
            {
                return addMedzaklad ??
                  (addMedzaklad = new RelayCommand(obj =>
                  {
                  WinNsiMedZaklad MedZaklad = new WinNsiMedZaklad();
                  MedZaklad.ShowDialog();
                  EdrpouMedZaklad = ReceptionLIkarGuest.Likart8.Text.ToString();
                  if (EdrpouMedZaklad == "") return;

                      selectedGridProfilLikar.edrpou = EdrpouMedZaklad;
                      selectedGridProfilLikar.adrZaklad = WindowProfilDoctor.Likart4.Text;
                      selectedGridProfilLikar.nameZaklad = WindowProfilDoctor.Likart9.Text;
                      


                  }));
            }
        }


        // команда 
        RelayCommand? exitCommandLikar;
        public RelayCommand ExitCommandLikar
        {
            get
            {
                return exitCommandLikar ??
                  (exitCommandLikar = new RelayCommand(obj =>
                  {
                      ExitCabinetLikar();
                  }));
            }
        }

        public void ExitCabinetLikar()
        {
            _kodDoctor = "";
            _pacientProfil = "";
            SelectedGridProfilLikar = new ModelGridDoctor();
            selectedGridProfilLikar = new ModelGridDoctor();

            loadboolPacientProfil = false;
            loadboolProfilLikar = false;
            selectedProfilLikar = new ModelDoctor();
            SelectedProfilLikar = new ModelDoctor();
            SelectedGridProfilLikar = new ModelGridDoctor();
            selectedPacientProfil = new ModelPacient();
            SelectedPacientProfil = new ModelPacient();
            SelectedPacientMapAnaliz = new PacientMapAnaliz();
            ViewPacientProfils = new ObservableCollection<ModelPacient>();
            ViewPacientMapAnalizs = new ObservableCollection<PacientMapAnaliz>();
            ViewProfilLikars = new ObservableCollection<ModelDoctor>();
            WindowAccountUser.StatusHealthTablGrid.ItemsSource = null;
            WindowAccountUser.StatusHealth3.Text = "";
            SelectedColectionIntevLikar = new ModelColectionInterview();
            WindowAccountUser.ColectionIntevLikarTablGrid.ItemsSource = null;
            SelectedReceptionPacient = new ModelReceptionPatient();
            WindowAccountUser.ReceptionPacientTablGrid.ItemsSource = null;
            SelectedColectionIntevPacient = new ModelColectionInterview();
            WindowAccountUser.ColectionIntevPacientTablGrid.ItemsSource = null;
            SelectedColectionReceptionPatient = new ModelColectionInterview();
            WindowAccountUser.ReceptionLikarTablGrid.ItemsSource = null;
            WindowMain.StackPanelCabPacient.Visibility = Visibility.Visible;
            BoolFalseProfilLikar();
            BoolFalsePacientProfil();
        }

        // команда 
        RelayCommand? readListMedZaklad;
        public RelayCommand ReadListMedZaklad
        {
            get
            {
                return readListMedZaklad ??
                  (readListMedZaklad = new RelayCommand(obj =>
                  {

                      MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
                      WinNsiMedZaklad NewOrder = new WinNsiMedZaklad();
                      NewOrder.ShowDialog();
                      selectedProfilLikar.edrpou = WindowMain.Likart8.Text; 
                  }));
            }
        }

        // команда загрузки сайта
        private RelayCommand? gridProfilWebUriLikar;
        public RelayCommand GridProfilWebUriLikar
        {
            get
            {
                return gridProfilWebUriLikar ??
                  (gridProfilWebUriLikar = new RelayCommand(obj =>
                  {

                      if (selectedGridProfilLikar.uriwebDoctor != "")
                      {
                          string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                          string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                          string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                          Process Rungoogle = new Process();
                          Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                          Rungoogle.StartInfo.Arguments = selectedGridProfilLikar.uriwebDoctor;
                          //Rungoogle.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
                          Rungoogle.StartInfo.UseShellExecute = false;
                          Rungoogle.EnableRaisingEvents = true;
                          Rungoogle.Start();
                      }
                  }));
            }
        }

        #endregion
    }

}


