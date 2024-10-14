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
    public class ViewModelRegisterAccountUser : BaseViewModel
    {
        

        private WinRegisterAccountUser WindowAccount = MainWindow.LinkMainWindow("WinRegisterAccountUser");
        public static bool OnOffEye = false, ReestrOnOff = false, _GhangePaswUser = false;
        public static string PasswordText = "", CountryKod = "+380", AccountTel="";
        private static string pathcontrolerAccountUser = "/api/AccountUserController/";
        private string pathcontroller = "/api/PacientController/";
        private string pathcontrolerProfilLikar = "/api/ApiControllerDoctor/";
        private static ModelAccountUser selectedModelAccountUser;
        public ModelAccountUser SelectedModelAccountUser
        {
            get { return selectedModelAccountUser; }
            set { selectedModelAccountUser = value; OnPropertyChanged("SelectedModelAccountUser"); }
        }
        
          
        // команда закрытия окна
        RelayCommand? passwordOnOff;
        public RelayCommand PasswordOnOff
        {
            get
            {
                return passwordOnOff ??
                  (passwordOnOff = new RelayCommand(obj =>
                  {
                      if (OnOffEye == false)
                      {
                          PasswordText = WindowAccount.Passw.Password;
                          WindowAccount.PasswText.Text = WindowAccount.Passw.Password;
                      }
                      else
                      {
                          PasswordText = WindowAccount.PasswText.Text;
                          WindowAccount.Passw.Password = WindowAccount.PasswText.Text;
                      }
                  }));
            }
        }


        // команда закрытия окна
        RelayCommand? eyeOnOff;
        public RelayCommand EyeOnOff
        {
            get
            {
                return eyeOnOff ??
                  (eyeOnOff = new RelayCommand(obj =>
                  {
                      if (OnOffEye == false)
                      {
                          WindowAccount.PasswText.Visibility = Visibility.Visible;
                          WindowAccount.Passw.Visibility = Visibility.Hidden;
                          OnOffEye = true;
                          WindowAccount.Eye.Visibility = Visibility.Visible;
                          WindowAccount.EyeDis.Visibility = Visibility.Hidden;
                      }
                      else
                      {
                          WindowAccount.PasswText.Visibility = Visibility.Hidden;
                          WindowAccount.Passw.Visibility = Visibility.Visible;
                          OnOffEye = false;
                          WindowAccount.Eye.Visibility = Visibility.Hidden;
                          WindowAccount.EyeDis.Visibility = Visibility.Visible;
                      }
                  }));
            }
        }

        // команда контроля нажатия клавиши enter
        RelayCommand? checkKeyEnter;
        public RelayCommand CheckKeyEnter
        {
            get
            {
                return checkKeyEnter ??
                  (checkKeyEnter = new RelayCommand(obj =>
                  {
                      MetodNextAccount();
                  }));
            }
        }

        private void MetodNextAccount()
        {
            if (WindowAccount.TelAccount.Text.Length < 2)
            {
                MainWindow.MessageError = "Увага!" + Environment.NewLine +
                "Невірно введений логін облікового запису.";
                MapOpisViewModel.SelectedFalseLogin();
                MapOpisViewModel.boolSetAccountUser = false;
                WindowAccount.Open.Visibility = Visibility.Hidden;
                WindowAccount.PasswText.Visibility = Visibility.Hidden;
                WindowAccount.Passw.Visibility = Visibility.Hidden;
                return;
            }
            WindowAccount.Open.Visibility = Visibility.Visible;
            WindowAccount.PasswText.Visibility = Visibility.Visible;
            WindowAccount.Passw.Visibility = Visibility.Visible;
        }

        // команда закрытия окна
        RelayCommand? checkKeyText;
        public RelayCommand CheckKeyText
        {
            get
            {
                return checkKeyText ??
                  (checkKeyText = new RelayCommand(obj =>
                  {
                      //if (MapOpisViewModel.WindowProfilPacient.ControlMain.SelectedIndex != 3)
                      //{ 
                      //  IdCardKeyUp.CheckKeyUpIdCard(WindowAccount.TelAccount,13);
                      //}    
                      
                     
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? closeAccount;
        public RelayCommand CloseAccount
        {
            get
            {
                return closeAccount ??
                  (closeAccount = new RelayCommand(obj =>
                  {
                      
                      if (MapOpisViewModel.CallViewProfilLikar == "PacientProfil")
                      { 
                          MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
                          WindowMain.StackPanelCabPacient.Visibility = Visibility.Visible;
                          WindowMain.LoadProfil.Visibility = Visibility.Visible;
                      }
                      if (MapOpisViewModel.CallViewProfilLikar == "ProfilLikar")
                      {
                          MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
                          WindowMain.LikarLoadInf.Visibility = Visibility.Visible;
                          WindowMain.LikarLoadinterv.Visibility = Visibility.Visible;
                          WindowMain.StackPanelCabLikar.Visibility = Visibility.Visible;
                      }
                      WindowAccount.Close();
                  }));
            }
        }

        // команда корретировки пароля
        private RelayCommand? ghangeAccount;
        public RelayCommand GhangeAccount
        {
            get
            {
                return ghangeAccount ??
                  (ghangeAccount = new RelayCommand(obj =>
                  {
                      AccountTel = WindowAccount.TelAccount.Text.ToString();
                      _GhangePaswUser = true;
                      MapOpisViewModel.NewAccountRecords();
                      _GhangePaswUser = false;
                  }));
            }
        }

        // команда контроля нажатия клавиши enter
        RelayCommand? checkKeyPassword;
        public RelayCommand CheckKeyPassword
        {
            get
            {
                return checkKeyPassword ??
                  (checkKeyPassword = new RelayCommand(obj =>
                  {
                      MetodOpenAccount();
                  }));
            }
        }




        RelayCommand? nextAccount;
        public RelayCommand NextAccount
        {
            get
            {
                return nextAccount ??
                  (nextAccount = new RelayCommand(obj =>
                  {
                      MetodNextAccount();
                  }));
            }
        }

        
        // команда регистрации нового пользователя
        RelayCommand? reestrAccount;
        public RelayCommand ReestrAccount
        {
            get
            {
                return reestrAccount ??
                  (reestrAccount = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.IndexAddEdit = "addCommand";
                      ReestrOnOff = true;
                      MainWindow WindowMen = MainWindow.LinkNameWindow("WindowMain");
                      MainWindow.MessageError = "Для реєєстрації необхідно виконати наступні дії:" + Environment.NewLine;
                      switch (MapOpisViewModel.CallViewProfilLikar)
                      {
                          case "ProfilLikar":
                              MainWindow.MessageError += "1. Ввести реєстраційні данні до профілю лікаря.";
                              MapOpisViewModel.BoolTrueProfilLikar();
                              break;
                          case "PacientProfil":
                              MainWindow.MessageError += "1. Ввести реєстраційні данні до профілю пацієнта.";
                              MapOpisViewModel.BoolTruePacientProfil();
                              break;
                      }
                      MainWindow.MessageError += Environment.NewLine + "2. Зберегти введені дані натиснувши на кнопку 'Зберегти'" +
                                Environment.NewLine + "3. У відповідь на запит ввести логін та пароль";
                      MapOpisViewModel.SelectedFalseLogin();
                      WindowAccount.Close();
                  }));
            }
        }

        RelayCommand? openAccount;
        public RelayCommand OpenAccount
        {
            get
            {
                return openAccount ??
                  (openAccount = new RelayCommand(obj =>
                  {
                      MetodOpenAccount();
                  }));
            }
        }

        private void MetodOpenAccount()
        {
           
            string LogPasw =  WindowAccount.TelAccount.Text.ToString(); //CountryKod +
            if (MapOpisViewModel.WindowProfilPacient.ControlMain.SelectedIndex == 3) LogPasw = WindowAccount.TelAccount.Text.ToString();
            string json = pathcontrolerAccountUser + "0/" + LogPasw + "/"+ WindowAccount.PasswText.Text.ToString() + "/0";
            CallServer.PostServer(pathcontrolerAccountUser, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]"))
            {
                MainWindow.MessageError = "Увага!" + Environment.NewLine +
                "Невірно введені логін або пароль облікового запису.";
                MapOpisViewModel.SelectedFalseLogin();
                MapOpisViewModel.boolSetAccountUser = false;
                return;
            }
            CmdStroka = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            ModelAccountUser IdAccountUser = JsonConvert.DeserializeObject<ModelAccountUser>(CmdStroka);

            switch(IdAccountUser.idStatus)
            {
                case "1":
                    WindowAccount.Open.Visibility = Visibility.Hidden;
                    MapOpisViewModel.boolSetAccountUser = true;
                    MapOpisViewModel.loadboolAccountUser = true;
                    MapOpisViewModel.CallViewProfilLikar = "Admin";
                    break;
                case "2":
                    CallServer.PostServer(pathcontroller, pathcontroller+ IdAccountUser.idUser+ "/0/0/0/0", "GETID");
                    CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]"))
                    {
                        MainWindow.MessageError = "Увага!" + Environment.NewLine +
                        "Інформація про користувача за вказаним обліковим записом відсутня у довіднику пацієнтів ";
                        MapOpisViewModel.SelectedFalseLogin();
                        MapOpisViewModel.loadboolPacientProfil = false;
                        MapOpisViewModel.boolSetAccountUser = false;
                        WindowAccount.Open.Visibility = Visibility.Hidden;
                        return;
                    }
                    MapOpisViewModel.ObservableViewPacientProfil(CmdStroka);
                    MapOpisViewModel.loadboolPacientProfil = true;
                    MapOpisViewModel.boolSetAccountUser = false;
                    MapOpisViewModel.CallViewProfilLikar = "PacientProfil";
                    break;
                case "3":
                    CallServer.PostServer(pathcontrolerProfilLikar, pathcontrolerProfilLikar + IdAccountUser.idUser + "/0/0", "GETID");
                    CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]"))
                    {
                        MainWindow.MessageError = "Увага!" + Environment.NewLine +
                        "Інформація про користувача за вказаним обліковим записом відсутня у довіднику лікарів ";
                        MapOpisViewModel.SelectedFalseLogin();
                        MapOpisViewModel.boolSetAccountUser = false;
                        MapOpisViewModel.loadboolProfilLikar = false;
                        WindowAccount.Open.Visibility = Visibility.Hidden;
                        return;
                    }
                    MapOpisViewModel.ObservableViewProfilLikars(CmdStroka);
                    MapOpisViewModel.boolSetAccountUser = false;
                    MapOpisViewModel.loadboolProfilLikar = true;
                    MapOpisViewModel.CallViewProfilLikar = "ProfilLikar";
                    break;
                      
                      
            }
            WindowAccount.Close();

            
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
    }

    //    public void SetNewCountry(string selected = "")
    //    {
    //        //MainWindow WindowMen = MainWindow.LinkNameWindow("WindowMain");
    //        CountryKod = selected == "0" ? "+380" : "+44";
    //    }
    //}

    public class IdCardKeyUp
    {
        public static void CheckKeyUpIdCard(TextBox SetTextBox, int IdLengh)
        {

            if (SetTextBox.Text.Length >= (IdLengh + 1)) SetTextBox.Text = SetTextBox.Text.Substring(0, IdLengh); ;

            for (int indPoint = 2; indPoint <= SetTextBox.Text.Length; indPoint++)
            {
                if (!Char.IsDigit(Convert.ToChar(SetTextBox.Text.Substring(indPoint - 1, 1)))) SetTextBox.Text = SetTextBox.Text.Substring(0, indPoint - 1);
            }
        }

    }
}
