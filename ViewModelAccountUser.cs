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

        // ViewAccountUsers  Справочник учетных записей пользователей
        // клавиша в окне:  Облікові записи
        
       
        #region Обработка событий и команд вставки, удаления и редектирования справочника 
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех жалоб из БД
        /// через механизм REST.API
        /// </summary>      
        public  static MainWindow WindowAccountUser = MainWindow.LinkNameWindow("WindowMain");
        public static  bool editboolAccountUser = false, addboolAccountUser = false, loadboolAccountUser = false;
        private string edittextAccountUser = "", SetIdStatus = "";
        private static string pathcontrolerAccountUser = "/api/AccountUserController/";
        public static string pathcontrolerStatusUser = "/api/NsiStatusUserController/";
        private static string pathcontrolernsiPacient = "/api/PacientController/";
        private static string pathcontrolernsiLikar = "/api/ApiControllerDoctor/";
        public static ModelAccountUser selectedModelAccountUser;
        public static AccountUser selectedAccountUser;

        public static ObservableCollection<ModelAccountUser> ViewModelAccountUsers { get; set; }
        public static ObservableCollection<AccountUser> ViewAccountUsers { get; set; }
        public ModelAccountUser SelectedModelAccountUser
        {
            get { return selectedModelAccountUser; }
            set { selectedModelAccountUser = value; OnPropertyChanged("SelectedModelAccountUser"); }
        }

        public AccountUser SelectedAccountUser
        {
            get { return selectedAccountUser; }
            set { selectedAccountUser = value; OnPropertyChanged("SelectedAccountUser"); }
        }
       
        public static void ObservableViewAccountUsers(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListAccountUser>(CmdStroka);
            List<AccountUser> res = result.AccountUser.ToList();
            ViewAccountUsers = new ObservableCollection<AccountUser>((IEnumerable<AccountUser>)res);
            MethodLoadModelAccountUser();

        }

        private static void MethodLoadModelAccountUser()
        {
            ViewModelAccountUsers = new ObservableCollection<ModelAccountUser>();
            foreach (AccountUser accountUser in ViewAccountUsers)
            {
                selectedModelAccountUser = new ModelAccountUser();
                selectedModelAccountUser.id = accountUser.id;
                selectedModelAccountUser.idStatus = accountUser.idStatus;
                selectedModelAccountUser.idUser = accountUser.idUser;
                selectedModelAccountUser.login = accountUser.login;
                selectedModelAccountUser.password = accountUser.password;
                string json = "";
                if (accountUser.idStatus == null)
                {
                    json = pathcontrolerAccountUser + accountUser.id;
                    CallServer.PostServer(pathcontrolerAccountUser, json, "DELETE");

                }
                else
                { 
                    json = pathcontrolerStatusUser + accountUser.idStatus.ToString();
                    CallServer.PostServer(pathcontrolerStatusUser, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    NsiStatusUser Idinsert = JsonConvert.DeserializeObject<NsiStatusUser>(CallServer.ResponseFromServer);
                    if (Idinsert != null)
                    {
                        selectedModelAccountUser.kodDostupa = Idinsert.kodDostupa;
                        selectedModelAccountUser.nameStatus = Idinsert.nameStatus;
                    }
                    ViewModelAccountUsers.Add(selectedModelAccountUser);                
                }

         
            }
            WindowAccountUser.AccountUserTablGrid.ItemsSource = ViewModelAccountUsers;
            
        }

        #region Команды вставки, удаления и редектирования справочника "детализация характера"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "детализация характера"
        /// </summary>
        /// 
 
        // загрузка справочника по нажатию клавиши Завантажити
        private void MethodLoadAccountUser()
        {
            IndexAddEdit = "";
            loadboolAccountUser = true;
            CallViewProfilLikar = "Admin";
            RegStatusUser = "Адміністратор";
            WindowAccountUser.LoadAccountUser.Visibility = Visibility.Hidden;
            if (RegSetAccountUser() == false) return;


            CallServer.PostServer(pathcontrolerAccountUser, pathcontrolerAccountUser, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewAccountUsers(CmdStroka);

        }
 
        // команда добавления нового объекта
        public void AddComandAccountUser()
        {
            if (boolSetAccountUser == true)
            { 
                 if (loadboolAccountUser == false) MethodLoadAccountUser();
                MethodaddcomAccountUser();           
            }

        }

        private void MethodaddcomAccountUser()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            selectedAccountUser = new AccountUser();
            if (addboolAccountUser == false) BoolTrueAccountUser();
            else BoolFalseAccountUser();
            WindowAccountUser.AccountUserTablGrid.SelectedItem = null;

        }

 


        private void BoolTrueAccountUser()
        {
            addboolAccountUser = true;
            editboolAccountUser = true;
            WindowAccountUser.AccountUsert2.IsEnabled = true;
            WindowAccountUser.AccountUsert2.Background = Brushes.AntiqueWhite;
            WindowAccountUser.AccountUsert4.IsEnabled = true;
            WindowAccountUser.AccountUsert4.Background = Brushes.AntiqueWhite;
            if (IndexAddEdit == "addCommand")
            { 
                WindowAccountUser.FoldAccountUser0.Visibility = Visibility.Visible;
                WindowAccountUser.FoldAccountUser1.Visibility = Visibility.Visible;
            } 
        }

        private void BoolFalseAccountUser()
        {
            addboolAccountUser = false;
            editboolAccountUser = false;
            WindowAccountUser.AccountUsert2.IsEnabled = false;
            WindowAccountUser.AccountUsert2.Background = Brushes.White;
            WindowAccountUser.AccountUsert4.IsEnabled = false;
            WindowAccountUser.AccountUsert4.Background = Brushes.White;
            WindowAccountUser.FoldAccountUser0.Visibility = Visibility.Hidden;
            WindowAccountUser.FoldAccountUser1.Visibility = Visibility.Hidden;
            selectedModelAccountUser = new ModelAccountUser();
            SelectedModelAccountUser = selectedModelAccountUser;

        }

        // команда удаления
        public void MethodRemoveAccountUser()
        {
            if (selectedModelAccountUser != null)
            {
                if (selectedModelAccountUser.id != 0)
                {
                    
                    // Видалення данных о гостях, пациентах, докторах, учетных записях
                    if (MapOpisViewModel.DeleteOnOff == true)
                    {
                        MetodRemovePrifilPacient(selectedModelAccountUser.idUser);
                        selectedAccountUser.id = selectedModelAccountUser.id;
                        ViewAccountUsers.Remove(selectedAccountUser);
                        ViewModelAccountUsers.Remove(selectedModelAccountUser);
                        selectedAccountUser = new AccountUser();
                        selectedModelAccountUser = new ModelAccountUser();
                    }

                }
                else
                {
                    WindowAccountUser.AccountUsert1.Text = "";
                    WindowAccountUser.AccountUsert2.Text = "";
                    WindowAccountUser.AccountUsert3.Text = "";
                    WindowAccountUser.AccountUsert4.Text = "";
                    WindowAccountUser.AccountUsert5.Text = "";

                }
            }
            BoolFalseAccountUser();
            IndexAddEdit = "";       
        }


        // команда  редактировать
        public void MethodEditAccountUser()
        {
            if (boolSetAccountUser == true)
            { 
                IndexAddEdit = "editCommand";
                if (editboolAccountUser == false)
                {
                    BoolTrueAccountUser();
                }
                else
                {
                    BoolFalseAccountUser();
                    WindowAccountUser.AccountUserTablGrid.SelectedItem = null;
                    IndexAddEdit = "";
                }                    
            }
 
        }

        // команда сохранить редактирование
        public void MethodSaveAccountUser()
        { 
                      
            if (WindowAccountUser.AccountUsert2.Text.Length != 0)
            {
                if (selectedAccountUser == null) selectedAccountUser = new AccountUser();
                if (IndexAddEdit == "addCommand")
                          {
                    selectedAccountUser.login = WindowAccountUser.AccountUsert2.Text.ToString();
                    selectedAccountUser.password = WindowAccountUser.AccountUsert4.Text.ToString();
                    //  формирование кода Detailing по значениею группы выранного храктера жалобы
                    SelectNewAccountUser();
                              string json = JsonConvert.SerializeObject(selectedAccountUser);
                              CallServer.PostServer(pathcontrolerAccountUser, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              AccountUser Idinsert = JsonConvert.DeserializeObject<AccountUser>(CallServer.ResponseFromServer);
                              if (ViewAccountUsers == null)
                              {
                                  ViewAccountUsers = new ObservableCollection<AccountUser>();
                                  ViewAccountUsers.Add(Idinsert);
                              } 
                              else
                              { ViewAccountUsers.Insert(ViewAccountUsers.Count, Idinsert);  }
                              SelectedAccountUser = Idinsert;
                              MethodLoadModelAccountUser();
                              //ViewModelAccountUsers.Add(selectedModelAccountUser);
                              //SelectedModelAccountUser = selectedModelAccountUser;
                          }
                          else
                          {
                                selectedAccountUser.id = selectedModelAccountUser.id;
                                selectedAccountUser.idUser = selectedModelAccountUser.idUser.Contains(":") ?selectedModelAccountUser.idUser.Substring(0, selectedModelAccountUser.idUser.IndexOf(":")) : selectedModelAccountUser.idUser;
                                selectedAccountUser.idStatus = selectedModelAccountUser.idStatus;
                                selectedAccountUser.login = WindowAccountUser.AccountUsert2.Text.ToString();
                                selectedAccountUser.password = WindowAccountUser.AccountUsert4.Text.ToString();
                                string json = JsonConvert.SerializeObject(selectedAccountUser);
                                CallServer.PostServer(pathcontrolerAccountUser, json, "PUT");
                          }
            }
            else WindowAccountUser.AccountUsert2.Text = edittextAccountUser;
            WindowAccountUser.AccountUserTablGrid.SelectedItem = null;
            IndexAddEdit = "";
            BoolFalseAccountUser();       
        }
        public void SelectNewAccountUser()
        {
           
           if (ViewAccountUsers != null)
           {
                if (WindowAccountUser.AccountUsert5.Text.Trim().Length == 0)
                {

                    int _keyAccountUserindex = 0, setindex = 0;
                    _keyAccountUserindex = Convert.ToInt32(ViewAccountUsers[0].idUser.Substring(ViewAccountUsers[0].idUser.LastIndexOf(".") + 1, ViewAccountUsers[0].idUser.Length - (ViewAccountUsers[0].idUser.LastIndexOf(".") + 1)));
                    for (int i = 0; i < ViewAccountUsers.Count; i++)
                    {
                        setindex = Convert.ToInt32(ViewAccountUsers[i].idUser.Substring(ViewAccountUsers[i].idUser.LastIndexOf(".") + 1, ViewAccountUsers[i].idUser.Length - (ViewAccountUsers[i].idUser.LastIndexOf(".") + 1)));
                        if (_keyAccountUserindex < setindex) _keyAccountUserindex = setindex;
                    }
                    _keyAccountUserindex++;
                    string _repl = "0000000000";
                    selectedAccountUser.idUser = "CNT." + _repl.Substring(0, _repl.Length - _keyAccountUserindex.ToString().Length) + _keyAccountUserindex.ToString();
 
                }
                else { selectedAccountUser.idUser = WindowAccountUser.AccountUsert5.Text.ToString().Substring(0, WindowAccountUser.AccountUsert5.Text.ToString().IndexOf(":")); }               
           }
           else { selectedAccountUser.idUser = "CNT.0000000001"; }

            if (WindowAccountUser.AccountUsert3.Text.Length == 0)
            {
                MainWindow.MessageError = "Увага!" + Environment.NewLine + "Не введено статус користувача";// + Environment.NewLine + "";
                MapOpisViewModel.SelectedFalseLogin(8);
                return;
            }

        }

        // команда печати
        public void MethodPrintAccountUser()
        { 
                      WindowAccountUser.AccountUserTablGrid.SelectedItem = null;
                      if (ViewAccountUsers != null)
                      {
                          MessageBox.Show("Обліковий запис :" + ViewAccountUsers[0].idStatus.ToString());
                      }        
        }

        
  

        // команда добавления пользователя
        private RelayCommand? addStatusUser;
        public RelayCommand AddStatusUser
        {
            get
            {
                return addStatusUser ??
                  (addStatusUser = new RelayCommand(obj =>
                  {
                      AddComandAddStatusUser();
                  }));
            }
        }
        private void AddComandAddStatusUser()
        {
            MapOpisViewModel.CallViewProfilLikar = "WinNsiStatusUser";
            WinNsiStatusUser NewOrder = new WinNsiStatusUser();
            //NewOrder.Left = 600;
            //NewOrder.Top = 200;
            NewOrder.ShowDialog();
            MapOpisViewModel.CallViewProfilLikar = "";
            if (WindowAccountUser.AccountUsert3.Text.Trim().Length > 0)
            {
                SetIdStatus = WindowAccountUser.AccountUsert3.Text.ToString().Substring(0, WindowAccountUser.AccountUsert3.Text.ToString().IndexOf(":"));
                selectedAccountUser.idStatus = WindowAccountUser.AccountUsert3.Text.ToString().Substring(0, WindowAccountUser.AccountUsert3.Text.ToString().IndexOf(":"));
            }
        }


        

        private RelayCommand? addIdUser;
        public RelayCommand AddIdUser
        {
            get
            {
                return addIdUser ??
                  (addIdUser = new RelayCommand(obj =>
                  {
                      switch (SetIdStatus)
                      {
                          case "2":
                              MapOpisViewModel.CallViewProfilLikar = "WinNsiPacient";
                              AddComandAddWinNsiPacient();
                              break;
                          case "3":
                              MapOpisViewModel.CallViewProfilLikar = "WinNsiLikar";
                              AddComandAddWinNsiLikar();
                            break;
                      }
                      MapOpisViewModel.CallViewProfilLikar = "";
                  }));
            }
        }

        private void AddComandAddWinNsiPacient()
        {
            WinNsiPacient NewOrder = new WinNsiPacient();
            //NewOrder.Left = 600;
            //NewOrder.Top = 200;
            NewOrder.ShowDialog();
            
        }

        private void AddComandAddWinNsiLikar()
        {
            WinNsiLikar NewOrder = new WinNsiLikar();
            //NewOrder.Left = 600;
            //NewOrder.Top = 200;
            NewOrder.ShowDialog();
        }

        
        private RelayCommand? addnameUser;
        public RelayCommand AddnameUser
        {
            get
            {
                return addnameUser ??
                  (addnameUser = new RelayCommand(obj =>
                  {
                      if(WindowAccountUser.AccountUserTablGrid.SelectedIndex >=0)
                      {


                          selectedModelAccountUser = ViewModelAccountUsers[WindowAccountUser.AccountUserTablGrid.SelectedIndex];
                          selectedAccountUser = ViewAccountUsers[WindowAccountUser.AccountUserTablGrid.SelectedIndex];
                          if (selectedModelAccountUser != null)
                          { 
                              string json = "";
                              string Iduser = selectedModelAccountUser.idUser.ToString().Contains(":") ? selectedModelAccountUser.idUser.ToString().Substring(0, selectedModelAccountUser.idUser.ToString().IndexOf(":")) : selectedModelAccountUser.idUser.ToString();

                              switch (selectedModelAccountUser.idStatus)
                              {
                                  case "2":
                                      json = pathcontrolernsiPacient + Iduser+ "/0/0/0/0";
                                      CallServer.PostServer(pathcontrolernsiPacient, json, "GETID");
                                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                      ModelPacient Idinsert = JsonConvert.DeserializeObject<ModelPacient>(CallServer.ResponseFromServer);
                                      if (Idinsert != null) WindowAccountUser.AccountUsert5.Text = Idinsert.kodPacient + ": " + Idinsert.name + " " + Idinsert.surname;
                                      break;
                                  case "3":
                                      json = pathcontrolernsiLikar + Iduser + "/0";
                                      CallServer.PostServer(pathcontrolernsiLikar, json, "GETID");
                                      CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                                      ModelDoctor Insert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                                      if (Insert != null)
                                      { 
                                        WindowAccountUser.AccountUsert5.Text = Insert.kodDoctor + ": " + Insert.name + " " + Insert.surname;
                                      }
                                      break;
                              }                      
                          }

                      }
                  }));
            }
        }

        // команда 
        RelayCommand? exitCommandAdmin;
        public RelayCommand ExitCommandAdmin
        {
            get
            {
                return exitCommandAdmin ??
                  (exitCommandAdmin = new RelayCommand(obj =>
                  {
                      BoolFalseAccountUser();
                      ViewModelAccountUsers = new ObservableCollection<ModelAccountUser>();
                      WindowAccountUser.AccountUserTablGrid.ItemsSource = null;
                      loadboolAccountUser = false;
                      loadboolPacientProfil = false;
                      loadboolProfilLikar = false;
                      boolSetAccountUser = false;
                      selectedProfilLikar = new ModelDoctor();
                      SelectedProfilLikar = new ModelDoctor();
                      SelectedGridProfilLikar = new ModelGridDoctor();
                      selectedPacientProfil = new ModelPacient();
                      SelectedPacientProfil= new ModelPacient();

                      SelectedColectionIntevLikar = new ModelColectionInterview();
                      WindowAccountUser.ColectionIntevLikarTablGrid.ItemsSource = null;
                      SelectedReceptionPacient = new ModelReceptionPatient();
                      WindowAccountUser.ReceptionPacientTablGrid.ItemsSource = null;
                      SelectedColectionIntevPacient = new ModelColectionInterview();
                      WindowAccountUser.ColectionIntevPacientTablGrid.ItemsSource = null;
                      SelectedColectionReceptionPatient = new ModelColectionInterview();
                      WindowAccountUser.ReceptionLikarTablGrid.ItemsSource = null;
                      _kodDoctor = "";
                      _pacientProfil = "";


                  }));
            }
        }
        #endregion
        #endregion
    }
}
