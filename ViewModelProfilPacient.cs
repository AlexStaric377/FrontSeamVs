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
    public partial class MapOpisViewModel : BaseViewModel
    {

        // ViewModelPacient справочник пациентов
        // клавиша в окне: Кабинет пацієнта  

        #region Обработка событий и команд вставки, удаления и редектирования справочника "пациентов"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех паціентів из БД
        /// через механизм REST.API
        /// </summary> 
        public static MainWindow WindowProfilPacient = MainWindow.LinkNameWindow("WindowMain");
        public static bool editboolPacientProfil = false, addboolPacientProfil = false;
        private string edittextPacientProfil = "";
        private static string pathcontrolerPacientProfil = "/api/PacientController/";
        public static string pathcontrolerPacient = "/api/PacientController/", controlerLifePacient = "/api/LifePacientController/"
            , controlerLifeDoctor = "/api/LifeDoctorController/";
        public static ModelPacient selectedPacientProfil;
        public static string _pacientProfil = "", _pacientName="", _pacientGender = "", RegStatusUser= "Пацїєнт";

        public static List<string> UnitCombProfil { get; set; } = new List<string> { "чол.", "жін." };
        public static ObservableCollection<ModelPacient> ViewPacientProfils { get; set; }
        public ModelPacient SelectedPacientProfil
        {
            get { return selectedPacientProfil; }
            set { selectedPacientProfil = value; OnPropertyChanged("SelectedPacientProfil"); }
        }




        public static void ObservableViewPacientProfil(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelPacient>(CmdStroka);
            List<ModelPacient> res = result.ViewPacient.ToList();
            ViewPacientProfils = new ObservableCollection<ModelPacient>((IEnumerable<ModelPacient>)res);

         

        }

        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "детализация характера"
        /// </summary>
        /// 
      
       // загрузка справочника по нажатию клавиши Завантажити
        private void MethodLoadPacientProfil()
        {
            WindowMain.BorderCabPacient.Visibility = Visibility.Hidden;
            WindowMain.LoadProfil.Visibility = Visibility.Hidden;
            if (boolSetAccountUser == false && loadboolProfilLikar == false && loadboolPacientProfil == false)
            {
                if (RegSetAccountUser() == true)
                {
                    if (ViewPacientProfils != null)
                    {
                        if (ViewPacientProfils.Count > 0)
                        {
                            WindowProfilPacient.LoadProfil.Visibility = Visibility.Hidden;
                            selectedPacientProfil = ViewPacientProfils[0];
                            SetValuePacientProfil();
                        }
                    }

                    if (CallViewProfilLikar == "Admin") SelectRegPacientProfil();
                    if (CallViewProfilLikar == "ProfilLikar")
                    {
                        LoadMessageErrorProfilLikar();
                        return;
                    }
                }
            }
            else
            {
                if (loadboolProfilLikar == true && boolSetAccountUser == false)
                {
                    ProfilLikarMessageError();
                    return;
                }
                if (boolSetAccountUser == true) SelectRegPacientProfil();           
            }
 

        }
        // команда добавления нового объекта
        private void MethodaddcomPacientProfil()
        {
            if (loadboolPacientProfil == true)
            { 
                WarningMessageNewProfilPacient();
                return;
            } 
            
            CallViewProfilLikar = "PacientProfil";
            WindowMain.BorderCabPacient.Visibility = Visibility.Hidden;
            WindowMain.LoadProfil.Visibility = Visibility.Hidden;
            ViewPacientProfils = new ObservableCollection<ModelPacient>();
            selectedPacientProfil = new ModelPacient();
            SelectedPacientProfil = new ModelPacient();
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            if (addboolPacientProfil == true)
            {
                BoolFalsePacientProfil();
                WindowMain.BorderCabPacient.Visibility = Visibility.Visible;
                WindowMain.LoadProfil.Visibility = Visibility.Visible;
                addboolPacientProfil = false;
                ViewModelRegisterAccountUser.ReestrOnOff = true;
                if (loadboolPacientProfil == true) WarningMessageNewProfilPacient();
                return;
            }
            else
            {  BoolTruePacientProfil(); }
            WindowProfilPacient.CombgenderProfil.SelectedIndex = Convert.ToInt32(SelectedCombProfil);
 
        }

        public bool RegSetAccountUser()
        {
            if (boolSetAccountUser == true && CallViewProfilLikar == "Admin") return boolSetAccountUser;
            if (loadboolProfilLikar == true && CallViewProfilLikar == "ProfilLikar") return loadboolProfilLikar;
            if (loadboolPacientProfil == true && CallViewProfilLikar == "PacientProfil") return loadboolPacientProfil;
            bool _return = true;
            WinRegisterAccountUser NewAccountUser = new WinRegisterAccountUser();
            NewAccountUser.ShowDialog();


            if (CallViewProfilLikar == "PacientProfil") _return = loadboolPacientProfil;
            if (CallViewProfilLikar == "ProfilLikar") _return = loadboolProfilLikar;
            if (CallViewProfilLikar == "Admin") _return = boolSetAccountUser;
            return _return;
        }

        private void SelectRegPacientProfil()
        {
            
            CallViewProfilLikar = "PacientProfil";
            selectedPacientProfil = new ModelPacient();
            WinNsiPacient NewOrder = new WinNsiPacient();
            NewOrder.ShowDialog();
            CallViewProfilLikar = "";
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]"))
            {
                WarningMessageOfProfilPacient();
                return;
            }
            ObservableViewPacientProfil(CmdStroka);
            SetValuePacientProfil();
            
        }

        public void SetValuePacientProfil()
        {
 
            if (selectedPacientProfil != null)
            {
                LoadInfoPacient("пацієнта.");
                MainWindow WindowPacientProfil = MainWindow.LinkNameWindow("WindowMain");
                _pacientProfil = selectedPacientProfil.kodPacient;
                _pacientGender = selectedPacientProfil.gender;
                _pacientName = selectedPacientProfil.name + " " + selectedPacientProfil.surname + " " + selectedPacientProfil.profession + " " + selectedPacientProfil.tel;
                SelectedPacientProfil = selectedPacientProfil;
                WindowPacientProfil.PacientIntert3.Text = _pacientName;
                WindowPacientProfil.PacentNameInterv.Text = "Опитування пацієнта: ";
                WindowPacientProfil.ReceptionPacientzap3.Text = _pacientName;
                WindowPacientProfil.StatusHealth3.Text = _pacientName;
                SelectedProfilPacient = selectedProfilPacient;
                modelColectionInterview.namePacient = selectedPacientProfil.name + selectedPacientProfil.surname;
                modelColectionInterview.kodPacient = selectedPacientProfil.kodPacient;

                ColectionInterviewIntevPacients = new ObservableCollection<ModelColectionInterview>();
                WindowPacientProfil.ColectionIntevPacientTablGrid.ItemsSource = ColectionInterviewIntevPacients;
                ViewReceptionPatients = new ObservableCollection<ModelColectionInterview>();
                WindowPacientProfil.ReceptionLikarTablGrid.ItemsSource = ViewReceptionPatients;
                ViewPacientMapAnalizs = new ObservableCollection<PacientMapAnaliz>();
                WindowPacientProfil.StatusHealthTablGrid.ItemsSource = ViewPacientMapAnalizs;
                boolVisibleMessage = true;
                MethodLoadtableColectionIntevPacient();
                MethodLoadReceptionPacient();
                MethodLoadStanHealthPacient();

                MessageWarning Info = MainWindow.LinkNameWindow("WarningMessage");
                if (Info != null) Info.Close();
                boolVisibleMessage = false;

            }                   
        }


        private string _SelectedCombProfil;
        public  string SelectedCombProfil
        {
            get => _SelectedCombProfil;
            set
            {
                //меняем значение в обычном порядке
                _SelectedCombProfil = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCombProfil)));
                //OnPropertyChanged(nameof(SelectedComb));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewCombProfil(_SelectedCombProfil);
            }
        }

        public void SetNewCombProfil(string selected = "")
        {
            MainWindow WindowProfilPacient = MainWindow.LinkNameWindow("WindowMain");
            WindowProfilPacient.PacientProfilt7.Text = selected == "0" ? "чол." : "жін.";
        }

        public static void BoolTruePacientProfil()
        {
            MainWindow WindowProfilPacient = MainWindow.LinkNameWindow("WindowMain");
            addboolPacientProfil = true;
            editboolPacientProfil = true;
            List<string> Units = new List<string> { "чол.", "жін." };
            WindowProfilPacient.CombgenderProfil.SelectedIndex = 0;
            WindowProfilPacient.CombgenderProfil.ItemsSource = Units;
            WindowProfilPacient.PacientProfilt7.Text = WindowProfilPacient.CombgenderProfil.SelectedIndex == 0 ? "чол." : "жін.";
            WindowProfilPacient.CombgenderProfil.IsEnabled = true;
            WindowProfilPacient.PacientProfilt2.IsEnabled = true;
            WindowProfilPacient.PacientProfilt2.Background = Brushes.AntiqueWhite;
            WindowProfilPacient.PacientProfilt3.IsEnabled = true;
            WindowProfilPacient.PacientProfilt3.Background = Brushes.AntiqueWhite;
            WindowProfilPacient.PacientProfilt4.IsEnabled = true;
            WindowProfilPacient.PacientProfilt4.Background = Brushes.AntiqueWhite;
            WindowProfilPacient.PacientProfilt5.IsEnabled = true;
            WindowProfilPacient.PacientProfilt5.Background = Brushes.AntiqueWhite;
            WindowProfilPacient.PacientProfilt6.IsEnabled = true;
            WindowProfilPacient.PacientProfilt6.Background = Brushes.AntiqueWhite;
            WindowProfilPacient.PacientProfilt7.IsEnabled = true;
            WindowProfilPacient.PacientProfilt7.Background = Brushes.AntiqueWhite;
            WindowProfilPacient.PacientProfilt8.IsEnabled = true;
            WindowProfilPacient.PacientProfilt8.Background = Brushes.AntiqueWhite;
            WindowProfilPacient.PacientProfilt9.IsEnabled = true;
            WindowProfilPacient.PacientProfilt9.Background = Brushes.AntiqueWhite;
            WindowProfilPacient.PacientProfilt11.IsEnabled = true;
            WindowProfilPacient.PacientProfilt11.Background = Brushes.AntiqueWhite;
            WindowProfilPacient.PacientProfilt13.IsEnabled = true;
            WindowProfilPacient.PacientProfilt13.Background = Brushes.AntiqueWhite;
            if (ViewModelRegisterAccountUser.ReestrOnOff == true)
            {
                WindowProfilDoctor.LoadProfil.Content = "Для збреження профілю необхідно натиснути на кнопку 'Зберегти'";
            }
            WindowMain.BorderCabPacient.Visibility = Visibility.Hidden;
        }

        private void BoolFalsePacientProfil()
        {
            addboolPacientProfil = false;
            editboolPacientProfil = false;
            WindowProfilPacient.CombgenderProfil.IsEnabled = false;
            WindowProfilPacient.PacientProfilt2.IsEnabled = false;
            WindowProfilPacient.PacientProfilt2.Background = Brushes.White;
            WindowProfilPacient.PacientProfilt3.IsEnabled = false;
            WindowProfilPacient.PacientProfilt3.Background = Brushes.White;
            WindowProfilPacient.PacientProfilt4.IsEnabled = false;
            WindowProfilPacient.PacientProfilt4.Background = Brushes.White;
            WindowProfilPacient.PacientProfilt5.IsEnabled = false;
            WindowProfilPacient.PacientProfilt5.Background = Brushes.White;
            WindowProfilPacient.PacientProfilt6.IsEnabled = false;
            WindowProfilPacient.PacientProfilt6.Background = Brushes.White;
            WindowProfilPacient.PacientProfilt7.IsEnabled = false;
            WindowProfilPacient.PacientProfilt7.Background = Brushes.White;
            WindowProfilPacient.PacientProfilt8.IsEnabled = false;
            WindowProfilPacient.PacientProfilt8.Background = Brushes.White;
            WindowProfilPacient.PacientProfilt9.IsEnabled = false;
            WindowProfilPacient.PacientProfilt9.Background = Brushes.White;
            WindowProfilPacient.PacientProfilt11.IsEnabled = false;
            WindowProfilPacient.PacientProfilt11.Background = Brushes.White;
            WindowProfilPacient.PacientProfilt13.IsEnabled = false;
            WindowProfilPacient.PacientProfilt13.Background = Brushes.White;

        }

        // команда удаления
        public void MethodRemovePacientProfil()
        { 
            if (selectedPacientProfil != null)
            {
                MetodRemovePrifilPacient( selectedPacientProfil.kodPacient);
                WindowProfilPacient.StatusHealthTablGrid.SelectedIndex = 0;
                selectedPacientProfil = new ModelPacient();
                SelectedPacientProfil = new ModelPacient();
                ExitCabinetLikar();
            }
            IndexAddEdit = "";       
        
        }

        public static void MetodRemovePrifilPacient( string kodPacient = "")
        {
            string json = pathcontrolerPacient + "0/" + kodPacient;
            CallServer.PostServer(pathcontrolerPacient, json, "DELETE");

            // удаление анализа крови PacientAnalizKrovi
            json = ViewModelColectionAnalizBlood.pathcontrollerAnalizBlood + "0/" + kodPacient;
            CallServer.PostServer(ViewModelColectionAnalizBlood.pathcontrollerAnalizBlood, json, "DELETE");

            // удаление анализа мочи PacientAnalizUrine
            json = ViewModelColectionAnalizUrine.pathcontrollerAnalizUrine + "0/" + kodPacient;
            CallServer.PostServer(ViewModelColectionAnalizUrine.pathcontrollerAnalizUrine, json, "DELETE");

            // удаление пульс давление ... PacientMapAnaliz
            json = pathcontrollerPacientMapAnaliz + "0/" + kodPacient;
            CallServer.PostServer(pathcontrollerPacientMapAnaliz, json, "DELETE");


            // удаление Жизни пациента и взаимодействие с врачами LifePacient
            json = controlerLifePacient + "0/" + kodPacient + "/0";
            CallServer.PostServer(controlerLifePacient, json, "DELETE");

            // удаление пациентов записавшихся на прием RegistrationAppointment
            json = pathcontrollerAppointment + "0/" + kodPacient + "/0";
            CallServer.PostServer(pathcontrollerAppointment, json, "DELETE");

            // удаление пациентов записавшихся на прием  у доктора LifeDoctor
            json = controlerLifeDoctor + "0/" + kodPacient + "/0";
            CallServer.PostServer(controlerLifeDoctor, json, "DELETE");

            // удаление список приемов пациентов записавшихся на прием у доктора AdmissionPatients
            json = pathcontrolerReceptionPacient + "0/" + kodPacient + "/0";
            CallServer.PostServer(pathcontrolerReceptionPacient, json, "DELETE");

            // удаление проведеных интервью ColectionInterview
            json = ColectioncontrollerIntevPacient + "0/0/" + kodPacient;
            CallServer.PostServer(ColectioncontrollerIntevPacient, json, "DELETE");


            // удаление  учетной записи AccountUser
            json = pathcontrolerAccountUser + "0/" + kodPacient;
            CallServer.PostServer(pathcontrolerAccountUser, json, "DELETE");

        }


        // команда  редактировать
        public void MethodEditProfilPacient()
        { 
            if (selectedPacientProfil != null)
            {
                IndexAddEdit = "editCommand";
                if (editboolPacientProfil == false)
                {
                   if(selectedPacientProfil != null)
                        if(selectedPacientProfil.id !=0) BoolTruePacientProfil();
                } 
                else
                {
                    BoolFalsePacientProfil();
                    IndexAddEdit = "";
                }
            }       
        }

        // команда сохранить редактирование
        public void MethodSavePacientProfil()
        { 
            
            if (WindowProfilPacient.PacientProfilt2.Text.Length != 0 && WindowProfilPacient.PacientProfilt3.Text.Length != 0)
            {
                if (IndexAddEdit == "addCommand")
                {
                    //  формирование кода Detailing по значениею группы выранного храктера жалобы
                    if (WindowProfilPacient.PacientProfilt8.Text.Trim() == "" )
                    {
                        MainWindow.MessageError = "Увага!" + Environment.NewLine +
                                 "Ви не ввели номер телефону" + Environment.NewLine + " Будь ласка введіть у поле 'Телефон' свій номер.";
                        SelectedFalseLogin(7);
                        return;
                    }
                    if ( WindowProfilPacient.PacientProfilt8.Text.Trim().Length < 12  )
                    {
                        MainWindow.MessageError = "Увага!" + Environment.NewLine +
                                 "Ви ввели невірно номер телефону" + Environment.NewLine + " Будь ласка введіть у поле 'Телефон' свій номер.";
                        SelectedFalseLogin(7);
                        return;
                    }
                    SelectNewPacientProfil();
                    AddNewPacientProfil();
                    if (ViewModelRegisterAccountUser.ReestrOnOff == true)
                    {
                        string json = JsonConvert.SerializeObject(selectedPacientProfil);
                        CallServer.PostServer(pathcontrolerPacientProfil, json, "POST");
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                        int Countins = ViewPacientProfils != null ? ViewPacientProfils.Count : 0;
                        if (ViewPacientProfils == null)
                        {
                            ViewPacientProfils = new ObservableCollection<ModelPacient>();
                            ViewPacientProfils.Add(Idinsert);
                        }
                        else ViewPacientProfils.Insert(Countins, Idinsert);
                        SelectedPacientProfil = Idinsert;
                        _pacientProfil = Idinsert.kodPacient;
                        WindowProfilPacient.PacientIntert3.Text = selectedPacientProfil.name + " " + selectedPacientProfil.surname + " " + selectedPacientProfil.profession + " " + selectedPacientProfil.tel;
                        WindowProfilPacient.ReceptionPacientzap3.Text = WindowProfilPacient.PacientIntert3.Text;
                        WindowProfilPacient.StatusHealth3.Text = WindowProfilPacient.PacientIntert3.Text;
                    }
                    else SelectedPacientProfil = new ModelPacient();


                }
                else
                {
                    string json = JsonConvert.SerializeObject(selectedPacientProfil);
                    CallServer.PostServer(pathcontrolerPacientProfil, json, "PUT");
                }
                BoolFalsePacientProfil();
            }
            IndexAddEdit = "";       
        }

        // команда закрытия окна
        RelayCommand? checkKeyTextTel;
        public RelayCommand CheckKeyTextTel
        {
            get
            {
                return checkKeyTextTel ??
                  (checkKeyTextTel = new RelayCommand(obj =>
                  {
                      if (MapOpisViewModel.WindowProfilPacient.ControlMain.SelectedIndex != 3)
                      {
                          IdCardKeyUp.CheckKeyUpIdCard(WindowProfilPacient.PacientProfilt8, 12);
                      }


                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? checkKeyTextPind;
        public RelayCommand CheckKeyTextPind
        {
            get
            {
                return checkKeyTextPind ??
                  (checkKeyTextPind = new RelayCommand(obj =>
                  {
                      if (MapOpisViewModel.WindowProfilPacient.ControlMain.SelectedIndex != 3)
                      {
                          IdCardKeyUp.CheckKeyUpIdCard(WindowProfilPacient.PacientProfilt13, 5);
                      }


                  }));
            }
        }

        public static void SelectNewPacientProfil()
        {
            if (selectedPacientProfil == null) selectedPacientProfil = new ModelPacient();
            CallServer.PostServer(pathcontrolerPacientProfil, pathcontrolerPacientProfil + "0/0/0/0/0", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            selectedPacientProfil.id = 0;
            selectedPacientProfil.kodPacient = "PCN.0000000001";
            if (CmdStroka.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                int indexdia = Convert.ToInt32(Idinsert.kodPacient.Substring(Idinsert.kodPacient.LastIndexOf(".") + 1, Idinsert.kodPacient.Length - (Idinsert.kodPacient.LastIndexOf(".") + 1))) + 1;
                string _repl = "0000000000";
                selectedPacientProfil.kodPacient = "PCN." + _repl.Substring(0, _repl.Length - indexdia.ToString().Length) + indexdia.ToString();
            }

        }
        private void AddNewPacientProfil()
        {
            MapOpisViewModel.PacientPostIndex = WindowProfilPacient.PacientProfilt13.Text.ToString();
            selectedPacientProfil.age = Convert.ToInt32(WindowProfilPacient.PacientProfilt4.Text);
            selectedPacientProfil.email = WindowProfilPacient.PacientProfilt11.Text;
            selectedPacientProfil.gender = WindowProfilPacient.PacientProfilt7.Text;
            selectedPacientProfil.growth = WindowProfilPacient.PacientProfilt6.Text.ToString() != "" ? Convert.ToInt32(WindowProfilPacient.PacientProfilt6.Text) : 0;
            selectedPacientProfil.pind = WindowProfilPacient.PacientProfilt13.Text.ToString();
            selectedPacientProfil.name = WindowProfilPacient.PacientProfilt2.Text;
            selectedPacientProfil.surname = WindowProfilPacient.PacientProfilt3.Text;
            selectedPacientProfil.profession = WindowProfilPacient.PacientProfilt9.Text;
            selectedPacientProfil.tel = "+" + WindowProfilPacient.PacientProfilt8.Text;
            selectedPacientProfil.weight = WindowProfilPacient.PacientProfilt5.Text.ToString() != "" ? Convert.ToDecimal(WindowProfilPacient.PacientProfilt5.Text) : 0;

            MainWindow.MessageError = "Увага!" + Environment.NewLine +
                      "Ви бажаєте створити кабінет пацієнта для зберігання" + Environment.NewLine +" результатів ваших опитувань та записів до лікаря?";
            SelectedRemove();
            if (MapOpisViewModel.DeleteOnOff == true)
            {
                MapOpisViewModel.selectedProfilPacient = selectedPacientProfil;
                NewAccountRecords();
                
            }


        }

        // команда печати
        public void MethodPrintPacientProfil()
        {
            if (selectedPacientProfil != null)
            {
                MessageBox.Show("Пацієнт :" + selectedPacientProfil.name.ToString());
            }
        }


        // команда закрытия окна
        RelayCommand? exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ??
                  (exitCommand = new RelayCommand(obj =>
                  {
                      ExitCabinetLikar();
                  }));
            }
        }

        #endregion
        #endregion
    }
}

