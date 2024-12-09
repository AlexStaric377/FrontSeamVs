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
using Microsoft.Extensions.Configuration;
using System.IO;

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
/// функції виведення інформаційних повідомлень
namespace FrontSeam
{
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {
        public static MainWindow WindowInfo = MainWindow.LinkNameWindow("WindowMain");
        public static void SelectedDelete(int HeightWidth = 0)
        {
            WinDeleteData NewOrder = new WinDeleteData(MainWindow.MessageError);
            if (HeightWidth < 0)
            {

                //Random r = new Random();
                //Brush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)r.Next(1, 200), (byte)r.Next(1, 224), (byte)r.Next(1, 247)));
                //NewOrder.Yes.Background = brush;
                if (HeightWidth == -1 )NewOrder.Height = NewOrder.Height + 200;
                NewOrder.Width = NewOrder.Width + 270;
                NewOrder.BorderNo.Margin = new Thickness(0, 0, 250, 0);
                NewOrder.BorderYes.Margin = new Thickness(250, 0, 0, 0);
            }

            NewOrder.ShowDialog();
        }

        public static void SelectedMessageError()
        {

            MessageError NewOrder = new MessageError(MainWindow.MessageError);
            NewOrder.ShowDialog();
        }
        
        public static void SelectedFalseLogin(int TimePauza = 0)
        {
            TimePauza = TimePauza == 0 ? 7 : TimePauza;
            MessageWarn NewOrder = new MessageWarn(MainWindow.MessageError, 2, TimePauza);
            NewOrder.Height = NewOrder.Height + 90;
            NewOrder.grid2.Height = NewOrder.grid2.Height + 60;
            NewOrder.MessageText.Height = NewOrder.MessageText.Height + 60;
            //NewOrder.Left = 250;
            //NewOrder.Top = 100;
            NewOrder.ShowDialog();
        }

        public static void SelectedRemove()
        {
            WinDeleteData NewOrder = new WinDeleteData(MainWindow.MessageError);
            NewOrder.ShowDialog();
        }


        public static void SelectedMessageDialog()
        {
            WinMessageDialog NewOrder = new WinMessageDialog(MainWindow.MessageError);
            NewOrder.ShowDialog();
        }
        public static void SelectedWirning(int TimePauza = 0)
        {
            TimePauza = TimePauza == 0 ? 7 : TimePauza;
            MessageWarning NewOrder = new MessageWarning(MainWindow.MessageError, 2, TimePauza);
            NewOrder.ShowDialog();
        }

        public static void SelectedLoad(int TimePauza = 0)
        {
            TimePauza = TimePauza == 0 ? 100 : TimePauza;
            MessageWarning NewOrder = new MessageWarning(MainWindow.MessageError, 2, TimePauza);
            NewOrder.Height = NewOrder.Height - 90;
            NewOrder.grid2.Height = NewOrder.grid2.Height - 60;
            NewOrder.MessageText.Height = NewOrder.MessageText.Height - 60;
            NewOrder.Show();
        }
        public void MessageDeleteData()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine + "Ви дійсно бажаєте стерти облікові данні?";
            SelectedDelete();
        }

        
        public void LoadMessageError()
        {
            MainWindow.MessageError = "Похибка!" + Environment.NewLine +
            "Пацієнту дозволено реєструєтруватися тільки в кабінеті пацієнта" ;
            SelectedMessageError();
        }

        public void PacientProfilMessageError()
        {
            MainWindow.MessageError = "Похибка!" + Environment.NewLine +
            "Пацієнт не має доступу до кабінету лікаря";
            SelectedMessageError();
        }
        public void LoadMessageErrorProfilLikar()
        {
            MainWindow.MessageError = "Похибка!" + Environment.NewLine +
            "Лікарю дозволено реєструєтруватися тільки в кабінеті лікаря";
            SelectedMessageError();
        }
        public void ProfilLikarMessageError()
        {
            MainWindow.MessageError = "Похибка!" + Environment.NewLine +
            "Лікар не має доступу до кабінету пацієнта";
            SelectedMessageError();
        }
        public void ProfilLikarAdminMessageError()
        {
            MainWindow.MessageError = "Похибка!" + Environment.NewLine +
            "Лікар не має доступу до функцій адміністратора";
            SelectedMessageError();
        }

        
            public void PacientProfilAdminMessageError()
        {
            MainWindow.MessageError = "Похибка!" + Environment.NewLine +
            "Пацієнт не має доступу до функцій адміністратора";
            SelectedMessageError();
        }

        public void LoadInfoPacient(string user = "")
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Будь ласка зачекайте, завантажується вся інформація щодо " + user;
            SelectedLoad();
        }
        public void MessageOnOffKabinetPacient()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для входження до кабінету пацієнта необхідно вийти з кабінету лікаря " + Environment.NewLine +
            "Закрити кабінет лікаря?";
            SelectedDelete(-2);
        }

        public void MessageOnOffProfilPacient()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для входження до кабінету пацієнта необхідно вийти з кабінету попереднього  пацієнта " + Environment.NewLine +
            "Закрити кабінет попереднього  пацієнта?";
            SelectedDelete(-2);
        }

        public void MessageOnOffKabinetAdmin()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для входження до кабінету пацієнта необхідно вийти з кабінету адміністратора " + Environment.NewLine +
            "Закрити кабінет адміністратора?";
            SelectedDelete(-2);
        }
        public void MessageOnOffKabinetLikarAdmin()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для входження до кабінету лікаря необхідно вийти з кабінету адміністратора " + Environment.NewLine +
            "Закрити кабінет адміністратора?";
            SelectedDelete(-2);
        }

        public void WarningMessageLoadProfilPacient()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для завантаження інформації необхідно завантажити профіль паціента ";
            MessageWarn NewOrder = new MessageWarn(MainWindow.MessageError, 2, 4);
            SelectedFalseLogin();
        }
        public void MessageOnOffKabinetLikar()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для входження до кабінету лікаря треба вийти з кабінету пацієнта " + Environment.NewLine +
            "Закрити кабінет пацієнта?"; ;
            SelectedDelete(-2);
        }

        public void MessageOnOffKabinetProfilLikar()
        {

            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для входження до кабінету лікаря треба вийти з кабінету попереднього лікаря. " + Environment.NewLine +
            "Закрити кабінет попереднього лікаря?"; ;
            SelectedDelete(-2);
        }

        public void WarningMessageOfProfilLikar()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для проведення опитування необхідно завантажити профіль лікаря ";
            SelectedFalseLogin();
        }

        public void WarningMessageLoadProfilLikar()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для завантаження інформації необхідно завантажити профіль лікаря ";
            MessageWarn NewOrder = new MessageWarn(MainWindow.MessageError, 2, 4);
            SelectedFalseLogin();
        }
        public void WarningMessageOfProfilPacient()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для проведення опитування  необхідно завантажити профіль паціента ";
            SelectedFalseLogin();
        }
        public void WarningMessageNewProfilPacient()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для реєстрації нового профіля треба  вийти з кабінету пацієнта. ";
            SelectedFalseLogin();
        }

        public void WarningMessageNewProfilLikar()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для реєстрації нового профіля треба  вийти з кабінету лікаря. ";
            SelectedFalseLogin();
        }

        public void WarningMessageReceptionLIkarGuest()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для запису на прийом до лікаря необхідно пройти опитування." + Environment.NewLine +
            "Для цього натиснути на кнопку 'Кімната для опитування' потім натиснути на кнопку 'Додати'.";
            SelectedFalseLogin();
        }

        public void WarningMessageReceptionLIkar()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Для запису на прийом до лікаря необхідно пройти опитування." + Environment.NewLine +
            "Для цього натиснути на кнопку 'Провести опитування' потім натиснути на кнопку 'Додати'.";
            SelectedFalseLogin();
        }


        public static void MessageDialogFeature()
        {
            MainWindow.MessageError = "Дуже добре!" + Environment.NewLine +
            "У наступному меню треба визначтити, як ця хворобливість себе проявляє.";
            SelectedMessageDialog();
        }

        public static void MessageDialogDetailing()
        {
            MainWindow.MessageError = "Дуже добре!" + Environment.NewLine +
            "У наступному меню треба визначтити, як ця хворобливість проявляється у часі," + Environment.NewLine + " в яких ситуаціях, при якій температурі,тощо.";
            SelectedMessageDialog();
        }

        public static void MessageEndDialog()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
          "Ви зупинили опитування і відмовилися від подальшої детелізації характеру зовнішніх проявів хворобливості." + Environment.NewLine +
           "Для одержання рекомендацій щодо поточного стану опитування натисніть на кнопку < Далі > або відмовитися на кнопку <Припинити> ";
            SelectedFalseLogin(12);
        }
        public void WarningMessageLoadLanguageUI()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Шановний користувач необхідно перезавантажити програму. ";
            SelectedWirning();
        }

        public void MessageAddOffProfilLikar()
        {
            MainWindow.MessageError = "Увага!" + Environment.NewLine +
            "Шановний користувач! Створення профілю лікаря здійснюється Адміністратором системи.";
            SelectedWirning();
        }

        public static void VersiyaBack()
        {
            
            // Информация для администрирования Версия программы, путь архивирования
            //string PuthConecto = Process.GetCurrentProcess().MainModule.FileName;
            //string Versia = FileVersionInfo.GetVersionInfo(PuthConecto).ToString();  // версия файла.
            //string VersiaT = Versia.Substring(Versia.IndexOf("FileVersion") + 12, Versia.IndexOf("FileDescription") - (Versia.IndexOf("FileVersion") + 12)).Replace("\r\n", "").Replace(" ", "");
            MainWindow.ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            MainWindow.ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            //WindowInfo.Add.Background = Brushes.Blue;
            Process[] ObjModulesList = Process.GetProcessesByName("FrontSeam");
            foreach (Process nobjModule in ObjModulesList)
            {
                // Заполнить коллекцию модулей
                ProcessModuleCollection ObjModules = ObjModulesList[0].Modules;
                // Итерация по коллекции модулей.
                foreach (ProcessModule objModule in ObjModules)
                {
                    //Получить правильный путь к модулю
                    string strModulePath = objModule.FileName.ToString();
                    //Если модуль существует
                    if (System.IO.File.Exists(objModule.FileName.ToString()))
                    {
                        //Читать версию
                        string strFileVersion = objModule.FileVersionInfo.FileVersion.ToString();
                        //Читать размер файла
                        string strFileSize = objModule.ModuleMemorySize.ToString();
                        //Читать дату модификации
                        FileInfo objFileInfo = new FileInfo(objModule.FileName.ToString());
                        string strFileModificationDate = objFileInfo.LastWriteTime.ToShortDateString();
                        //Читать описание файла
                        string strFileDescription = objModule.FileVersionInfo.FileDescription.ToString();
                        //Читать имя продукта
                        string strProductName = objModule.FileVersionInfo.ProductName.ToString();
                        //Читать версию продукта
                        string strProductVersion = objModule.FileVersionInfo.ProductVersion.ToString();
                        WindowInfo.InfoSeamVer.Text = WindowInfo.InfoSeamVer.Text + strFileVersion + " Дата зборки: " + strFileModificationDate;
                        WindowInfo.Title += " Версія: " + strFileVersion;
                        break;
                    }
                }
            }
            CallServer._UrlAdres = ConfigBuild();
            LoadSelectLanguageUI();
            ViewAccountUser();
        }

        public static string ConfigBuild()
        {
            
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", true);

            IConfigurationRoot config = builder.Build();
            CallServer.UnloadString = config.GetSection("ConnectionStrings:UnloadString").Value;
            MainWindow.SelectLanguageUI = config.GetSection("ConnectionStrings:LanguageUI").Value;
            WindowInfo.LanguageUIt1.Text = MainWindow.SelectLanguageUI;
            return config.GetConnectionString("Urls");
        }

        public static void LoadSelectLanguageUI()
        {
           
            MainWindow.SelectLanguageUI = "Ukraine";
            string LoadLanguageUI = "", OutFile = "";
            if (MainWindow.SelectLanguageUI != "Ukraine")
            {
                LoadLanguageUI = Directory.GetCurrentDirectory() + @"\LanguageUI\";
                OutFile = LoadLanguageUI + MainWindow.SelectLanguageUI + ".json"; ;
                if (Directory.Exists(LoadLanguageUI))
                {
                    if (System.IO.File.Exists(OutFile))
                    {

                        Encoding code = Encoding.Default;
                        string[] fileLines = System.IO.File.ReadAllLines(OutFile, code);
                        foreach (string str in fileLines)
                        {
                            string[] data = str.Split('=');
                            if (data[0].Trim() == "Gues") WindowInfo.Gues.Header = data[1].Trim();

                        }
                    }
                }
            }

        }

        public static void ViewAccountUser()
        {
            if (MapOpisViewModel.boolSetAccountUser == false)
            {
                WindowInfo.AccountZap.Visibility = Visibility.Hidden;
                WindowInfo.NsiStatusUser.Visibility = Visibility.Hidden;
            }
            else
            {
                WindowInfo.AccountZap.Visibility = Visibility.Visible;
                WindowInfo.NsiStatusUser.Visibility = Visibility.Visible;
            }
        
        }
   
        public static void ViewNsiFeature()
        {
            WinNsiFeature zagolovok = MainWindow.LinkMainWindow("WinNsiFeature");
            zagolovok.Zagolovok.Content += MapOpisViewModel.selectedComplaintname.ToUpper();
        }

        public static void ViewNsiDetaling()
        {
            NsiDetailing zagolovok = MainWindow.LinkMainWindow("NsiDetailing");
            zagolovok.Zagolovok.Content += MapOpisViewModel.selectFeature.ToUpper();
        }
        public static void ViewNsiGrDetaling()
        {
            WinNsiGrDetailing zagolovok = MainWindow.LinkMainWindow("WinNsiGrDetailing");
            zagolovok.Zagolovok.Content += MapOpisViewModel.selectGrDetailing.ToUpper();
        }
        public static void ViewNsiQualification()
        {
            WinNsiQualification zagolovok = MainWindow.LinkMainWindow("WinNsiQualification");
            zagolovok.Zagolovok.Content += MapOpisViewModel.selectQualification.ToUpper();
        }

        public static void VisibleLikarOffSelect()
        {
            WinLikarGrupDiagnoz winLikarGrDiagnoz = MainWindow.LinkMainWindow("WinLikarGrupDiagnoz");
            if (SelectActivGrupDiagnoz == "GrupDiagnoz")
            {
                winLikarGrDiagnoz.ButtonAdd.Visibility = Visibility.Hidden;
                winLikarGrDiagnoz.ButtonDelete.Visibility = Visibility.Hidden;
            }
            else
            { winLikarGrDiagnoz.ButtonSelect.Visibility = Visibility.Hidden; }


        }

        public static void NotVisitingDays()
        {
            if (MapOpisViewModel.boolVisibleMessage == true) return;
            MainWindow.MessageError = "Розклад прийому пацієнтів не введено" + Environment.NewLine +
            "Дата та час прийому буде визначена лікарем додатково";
            MessageWarn NewOrder = new MessageWarn(MainWindow.MessageError, 2, 5);
            NewOrder.ShowDialog();

        }


    }
}
