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
    class ViewModelAccountRecords : BaseViewModel
    {
      
        private static  WinAccountRecords WindowAccount = MainWindow.LinkNameWindow("AccountRecords");
        public static bool On1OffEye = false, Reestr1OnOff = false, On2OffEye = false, Reestr2OnOff = false, checkregkod = true;
        public static string Password1Text = "", Password2Text = "", _telefon ="", _fio="", CheckRegKod="";
        private static string pathcontrolerAccountUser = "/api/AccountUserController/";
        private string pathcontroller = "/api/PacientController/";
        private string pathcontrolerProfilLikar = "/api/ApiControllerDoctor/";
        public static ModelAccountUser selectedModelAccountUser;
        public static ModelDoctor selectProfilLikar;
        public ModelAccountUser SelectedModelAccountUser
        {
            get { return selectedModelAccountUser; }
            set { selectedModelAccountUser = value; OnPropertyChanged("SelectedModelAccountUser"); }
        }

        public ModelDoctor SelectProfilLikar
        {
            get { return selectProfilLikar; }
            set { selectProfilLikar = value; OnPropertyChanged("SelectProfilLikar"); }
        }

        public ViewModelAccountRecords()
        {
            switch (MapOpisViewModel.CallViewProfilLikar )
            {
                case  "ProfilLikar":
                selectProfilLikar = MapOpisViewModel.selectedProfilLikar;
                break;
                case "PacientProfil":
                    selectProfilLikar = new ModelDoctor();
                    if (ViewModelRegisterAccountUser._GhangePaswUser == false)
                    {
                        if (MapOpisViewModel.selectedProfilPacient != null)
                        { 
                            selectProfilLikar.name = MapOpisViewModel.selectedProfilPacient.name;
                            selectProfilLikar.surname = MapOpisViewModel.selectedProfilPacient.surname;
                            selectProfilLikar.telefon = MapOpisViewModel.selectedProfilPacient.tel;                        
                        }

                    }
                    else { selectProfilLikar.telefon = ViewModelRegisterAccountUser.AccountTel; }

                    break;
            }
            
 
        }



        // команда закрытия окна
        RelayCommand? closeAccount;
        public RelayCommand CloseAccountRecords
        {
            get
            {
                return closeAccount ??
                  (closeAccount = new RelayCommand(obj =>
                  {
                      WinAccountRecords WindowAccount = MainWindow.LinkNameWindow("AccountRecords");
                      WindowAccount.Close();
                      MapOpisViewModel.boolSetAccountUser = false;
                      MapOpisViewModel.BoolFalsePacientProfil();
                      WinRegisterAccountUser Windowrecord = MainWindow.LinkMainWindow("WinRegisterAccountUser");
                      if (Windowrecord != null) Windowrecord.Close();
                      
                  }));
            }
        }


        // команда ввода пароля
        RelayCommand? password1OnOff;
        public RelayCommand Password1OnOff
        {
            get
            {
                return password1OnOff ??
                  (password1OnOff = new RelayCommand(obj =>
                  {
                      if (On1OffEye == false)
                      {
                          Password1Text = WindowAccount.Passw1.Password;
                          WindowAccount.Passw1Text.Text = Password1Text;
                      }
                      else
                      {
                          Password1Text = WindowAccount.Passw1Text.Text;
                          WindowAccount.Passw1.Password = WindowAccount.Passw1Text.Text;
                      }
                  }));
            }
        }


        // команда изменения состояния видимости текста пароля
        RelayCommand? eye1OnOff;
        public RelayCommand Eye1OnOff
        {
            get
            {
                return eye1OnOff ??
                  (eye1OnOff = new RelayCommand(obj =>
                  {
                      if (On1OffEye == false)
                      {
                          WindowAccount.Passw1Text.Visibility = Visibility.Visible;
                          WindowAccount.Passw1.Visibility = Visibility.Hidden;
                          On1OffEye = true;
                          WindowAccount.Eye1.Visibility = Visibility.Visible;
                          WindowAccount.EyeDis1.Visibility = Visibility.Hidden;
                      }
                      else
                      {
                          WindowAccount.Passw1Text.Visibility = Visibility.Hidden;
                          WindowAccount.Passw1.Visibility = Visibility.Visible;
                          On1OffEye = false;
                          WindowAccount.Eye1.Visibility = Visibility.Hidden;
                          WindowAccount.EyeDis1.Visibility = Visibility.Visible;
                      }
                  }));
            }
        }

        // команда ввода пароля
        RelayCommand? password2OnOff;
        public RelayCommand Password2OnOff
        {
            get
            {
                return password2OnOff ??
                  (password2OnOff = new RelayCommand(obj =>
                  {
                      if (On2OffEye == false)
                      {
                          Password2Text = WindowAccount.Passw.Password;
                          WindowAccount.PasswText.Text = Password2Text;
                      }
                      else
                      {
                          Password2Text = WindowAccount.PasswText.Text;
                          WindowAccount.Passw.Password = WindowAccount.PasswText.Text;
                      }
                  }));
            }
        }


        // команда изменения состояния видимости текста пароля
        RelayCommand? eye2OnOff;
        public RelayCommand Eye2OnOff
        {
            get
            {
                return eye2OnOff ??
                  (eye2OnOff = new RelayCommand(obj =>
                  {
                      if (On2OffEye == false)
                      {
                          WindowAccount.PasswText.Visibility = Visibility.Visible;
                          WindowAccount.Passw.Visibility = Visibility.Hidden;
                          On2OffEye = true;
                          WindowAccount.Eye.Visibility = Visibility.Visible;
                          WindowAccount.EyeDis.Visibility = Visibility.Hidden;
                      }
                      else
                      {
                          WindowAccount.PasswText.Visibility = Visibility.Hidden;
                          WindowAccount.Passw.Visibility = Visibility.Visible;
                          On2OffEye = false;
                          WindowAccount.Eye.Visibility = Visibility.Hidden;
                          WindowAccount.EyeDis.Visibility = Visibility.Visible;
                      }
                  }));
            }
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
                      IdCardKeyUp.CheckKeyUpIdCard(WindowAccount.TelAccount, 12);

                  }));
            }
        }

        
        // команда прверка номера телефона
        RelayCommand? sendCmcTel;
        public RelayCommand SendCmcTel
        {
            get
            {
                return sendCmcTel ??
                  (sendCmcTel = new RelayCommand(obj =>
                  {
                      SendSmsTel();
                      WinRegKodDostup NewOrder = new WinRegKodDostup();
                      NewOrder.ShowDialog();
                      NewOrder.Close();

                      if (checkregkod == true)
                      {
                          WindowAccount.Password1.Visibility = Visibility.Visible;
                          WindowAccount.Passw1.Visibility = Visibility.Visible;
                          WindowAccount.Passw1.Visibility = Visibility.Visible;
                          WindowAccount.Eye1.Visibility = Visibility.Visible;
                          WindowAccount.EyeDis1.Visibility = Visibility.Visible;
                          WindowAccount.Pas.Visibility = Visibility.Visible;
                          WindowAccount.Passw.Visibility = Visibility.Visible;
                          WindowAccount.Eye.Visibility = Visibility.Visible;
                          WindowAccount.EyeDis.Visibility = Visibility.Visible;
                          WindowAccount.Open.Visibility = Visibility.Visible;
                          WindowAccount.Line2.Visibility = Visibility.Visible;
                          WindowAccount.Line1.Visibility = Visibility.Visible;
                          WindowAccount.PasswText.Visibility = Visibility.Visible;
                          WindowAccount.Passw1Text.Visibility = Visibility.Visible;
                      }

                  }));
            }
        }


        // процедура посилки сообщения с кодом подтверждения регистрации  номера телефона
        public void SendSmsTel()
        {

            CheckRegKod = "12345";
        }


        //
        RelayCommand? closeRegKod;
        public RelayCommand CloseRegKod
        {
            get
            {
                return closeRegKod ??
                  (closeRegKod = new RelayCommand(obj =>
                  {
                      WinRegKodDostup Windowregkod = MainWindow.LinkMainWindow("WinRegKodDostup");
                      Windowregkod.Close();
                  }));
            }
        }

        //

        RelayCommand? saveRegKod;
        public RelayCommand SaveRegKod
        {
            get
            {
                return saveRegKod ??
                  (saveRegKod = new RelayCommand(obj =>
                  {
                      WinRegKodDostup Windowregkod = MainWindow.LinkMainWindow("WinRegKodDostup");
                      if (Windowregkod.TelRegKod.Text != CheckRegKod)
                      {
                          checkregkod = false;
                          MainWindow.MessageError = "Увага!" + Environment.NewLine +
                          "Невірно введений реєстраційний код отриманий з смс повідомлення." + Environment.NewLine +
                          "Повторіть введення коду або завершіть реєстрацію.";
                          MapOpisViewModel.SelectedFalseLogin();
                      }
                      Windowregkod.Close();
                  }));
            }
        }

        // команда сохранения параметров учетной записи
        RelayCommand? saveAccountRecords;
        public RelayCommand SaveAccountRecords
        {
            get
            {
                return saveAccountRecords ??
                  (saveAccountRecords = new RelayCommand(obj =>
                  {

                      if (WindowAccount.Passw1Text.Text != WindowAccount.PasswText.Text)
                      {
                          MainWindow.MessageError = "Увага!" + Environment.NewLine +
                            "Перший та повторний паролі не співпадають." + Environment.NewLine +
                            "Необхідно повторити введення паролю.";
                          MapOpisViewModel.SelectedFalseLogin();
                          return;
                      }
                      if (ViewModelRegisterAccountUser._GhangePaswUser == true)
                      {
                          GhangePasswordUser();
                          return;
                      }
                      if (MapOpisViewModel.selectedProfilLikar == null && MapOpisViewModel.selectedProfilPacient == null)
                      {
                          MapOpisViewModel.regaccountUser.login = "+"+WindowAccount.TelAccount.Text;
                          MapOpisViewModel.regaccountUser.password = WindowAccount.Passw1Text.Text;
                          MapOpisViewModel.regaccountUser.idStatus = MapOpisViewModel.CallViewProfilLikar == "ProfilLikar" ? "3" : "2";
                      }
                      if (ViewModelRegisterAccountUser.IdAccountUser == null) MetodSaveAccountRecords();
                      else 
                      {
                          if (ViewModelRegisterAccountUser.IdAccountUser.idStatus == "Admin") return;
                          else MetodSaveAccountRecords();                      
                      }

 
                      MapOpisViewModel.loadboolPacientProfil = true;
                      WindowAccount.Close();
                      WinRegisterAccountUser Windowrecord = MainWindow.LinkMainWindow("WinRegisterAccountUser");
                      if (Windowrecord != null) Windowrecord.Close();
                  }));
            }
        }

        public static void MetodSaveAccountRecords()
        {
            selectedModelAccountUser = new ModelAccountUser();
            string AccountUserlog = MapOpisViewModel.selectedProfilPacient == null ? "" : "+" + MapOpisViewModel.selectedProfilPacient.tel;
            selectedModelAccountUser.id = 0;
            if (MapOpisViewModel.selectedProfilLikar != null || MapOpisViewModel.selectedProfilPacient != null)
            {
                 selectedModelAccountUser.idUser = MapOpisViewModel.CallViewProfilLikar == "ProfilLikar" ? MapOpisViewModel.selectedProfilLikar.kodDoctor : MapOpisViewModel.selectedProfilPacient.kodPacient;           
            }

            
            
            if (MapOpisViewModel.regaccountUser.login != null)
            {
                selectedModelAccountUser.login = MapOpisViewModel.regaccountUser.login;
                selectedModelAccountUser.password = MapOpisViewModel.regaccountUser.password;
                selectedModelAccountUser.idStatus = MapOpisViewModel.regaccountUser.idStatus;
            }
            else
            {
                selectedModelAccountUser.login = ViewModelRegisterAccountUser.IdAccountUser.login; //MapOpisViewModel.CallViewProfilLikar == "ProfilLikar" ? MapOpisViewModel.selectedProfilLikar.telefon : MapOpisViewModel.selectedProfilPacient.tel; //_telefon+
                selectedModelAccountUser.password = ViewModelRegisterAccountUser.IdAccountUser.password;
                selectedModelAccountUser.idStatus = ViewModelRegisterAccountUser.IdAccountUser.idStatus; //MapOpisViewModel.CallViewProfilLikar == "ProfilLikar" ? "3" : "2";            
            }


            string json = MapOpisViewModel.pathcontrolerStatusUser + selectedModelAccountUser.idStatus.ToString();
            CallServer.PostServer(MapOpisViewModel.pathcontrolerStatusUser, json, "GETID");
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            NsiStatusUser Idinsert = JsonConvert.DeserializeObject<NsiStatusUser>(CallServer.ResponseFromServer);
            if (Idinsert != null)
            {
                selectedModelAccountUser.kodDostupa = Idinsert.kodDostupa;
                selectedModelAccountUser.nameStatus = Idinsert.nameStatus;
                selectedModelAccountUser.accountCreatDate = DateTime.Now.ToShortDateString();
                selectedModelAccountUser.subscription = "";

            }
            json = JsonConvert.SerializeObject(selectedModelAccountUser);
            CallServer.PostServer(pathcontrolerAccountUser, json, "POST");
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            AccountUser Insert = JsonConvert.DeserializeObject<AccountUser>(CallServer.ResponseFromServer);
            if (Insert == null)
            {
                MainWindow.MessageError = "Увага!" + Environment.NewLine +
                "Обліковий запис не створено. За консультацією зверніться до системного адміністратора";
                MapOpisViewModel.SelectedFalseLogin();

 
            }
            else
            {
                
            }
        }


        private void GhangePasswordUser()
        {

            string json = "";
            json = pathcontrolerAccountUser +"0/"+ ViewModelRegisterAccountUser.CountryKod +ViewModelRegisterAccountUser.AccountTel+"/0";
            CallServer.PostServer(pathcontrolerAccountUser, json, "GETID");
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            AccountUser Idinsert = JsonConvert.DeserializeObject<AccountUser>(CallServer.ResponseFromServer);
            if (Idinsert != null)
            {
                Idinsert.password = WindowAccount.Passw1Text.Text.ToString();
                json = JsonConvert.SerializeObject(Idinsert);
                CallServer.PostServer(pathcontrolerAccountUser, json, "PUT");
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                AccountUser Insert = JsonConvert.DeserializeObject<AccountUser>(CallServer.ResponseFromServer);
                if (Insert != null)
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine +
                    "Ваш пароль змінено, обліковий запис успішно модифіковано." + Environment.NewLine +
                    "Відтепер ви реєструєте вхід до кабінету з новим паролем. ";
                    MapOpisViewModel.SelectedFalseLogin();
                }
                else
                {
                    MainWindow.MessageError = "Увага!" + Environment.NewLine +
                    "Пароль не змінено. Невірно задано логін користувача";
                    MapOpisViewModel.SelectedFalseLogin();
                }

            }
            else
            {
                MainWindow.MessageError = "Увага!" + Environment.NewLine +
                "Пароль не змінено. Невірно задано логін користувача";
                MapOpisViewModel.SelectedFalseLogin();
            }
        }
        public void NewAccountUser()
        {
            CallServer.PostServer(pathcontrolerAccountUser, pathcontrolerAccountUser, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) selectedModelAccountUser.idUser = "CNT.0000000001";
            else
            {
                var result = JsonConvert.DeserializeObject<ListAccountUser>(CmdStroka);
                List<AccountUser> res = result.AccountUser.ToList();
                MapOpisViewModel.ViewAccountUsers = new ObservableCollection<AccountUser>((IEnumerable<AccountUser>)res);
                int _keyAccountUserindex = 0, setindex = 0;
                _keyAccountUserindex = Convert.ToInt32(MapOpisViewModel.ViewAccountUsers[0].idUser.Substring(MapOpisViewModel.ViewAccountUsers[0].idUser.LastIndexOf(".") + 1, MapOpisViewModel.ViewAccountUsers[0].idUser.Length - (MapOpisViewModel.ViewAccountUsers[0].idUser.LastIndexOf(".") + 1)));
                for (int i = 0; i < MapOpisViewModel.ViewAccountUsers.Count; i++)
                {
                    setindex = Convert.ToInt32(MapOpisViewModel.ViewAccountUsers[i].idUser.Substring(MapOpisViewModel.ViewAccountUsers[i].idUser.LastIndexOf(".") + 1, MapOpisViewModel.ViewAccountUsers[i].idUser.Length - (MapOpisViewModel.ViewAccountUsers[i].idUser.LastIndexOf(".") + 1)));
                    if (_keyAccountUserindex < setindex) _keyAccountUserindex = setindex;
                }
                _keyAccountUserindex++;
                string _repl = "0000000000";
                selectedModelAccountUser.idUser = "CNT." + _repl.Substring(0, _repl.Length - _keyAccountUserindex.ToString().Length) + _keyAccountUserindex.ToString();

            }

        }
        
        //public static List<string> Countrys { get; set; } = new List<string> { "+380", "+44" };

        //private string _SelectedCountry;
        //public string SelectedCountry
        //{
        //    get => _SelectedCountry;
        //    set
        //    {
        //        //// сохраняем старое значение
        //        //var origValue = _SelectedUnit;

        //        //меняем значение в обычном порядке
        //        _SelectedCountry = value;
        //        //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCountry)));
        //        //OnPropertyChanged(nameof(SelectedUnit));
        //        //а здесь уже преобразуем изменившиеся значение
        //        //в необходимое uint
        //        SetNewCountry(_SelectedCountry);
        //    }
        //}

        //public void SetNewCountry(string selected = "")
        //{
        //    ViewModelAccountRecords WindowMen = MainWindow.LinkMainWindow("ViewModelAccountRecords");
        //    _telefon = selected == "0" ? "+380" : "+44";
        //}
    }
}
