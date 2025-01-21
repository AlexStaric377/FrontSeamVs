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
/// Многопоточность
using System.Threading;
using System.Windows.Threading;

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {
        public static bool DeleteOnOff = false, LoadKabinet = false, endwait = false;

        
        private RelayCommand? geustGridLoadHidden;
        public RelayCommand GeustGridLoadHidden
        {
            get
            {
                return geustGridLoadHidden ??
                  (geustGridLoadHidden = new RelayCommand(obj =>
                  {
 
                          switch (WindowProfilPacient.ControlMain.SelectedIndex)
                          {
                              // Закладка Гость
                              case 0:
                                  WindowIntevLikar.GridLoadButton.Visibility = Visibility.Hidden;
                                  WindowIntevLikar.GridAdd.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridGhange.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridSave.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridPrint.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridDelete.Visibility = Visibility.Visible;
                                  break;
                          // Закладка пациент
                          case 1:
                                  WindowIntevLikar.GridLoadButton.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridAdd.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridGhange.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridSave.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridPrint.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridDelete.Visibility = Visibility.Visible;
                                  switch (WindowProfilPacient.ControlPacient.SelectedIndex)
                                  {
                                      case 1:
                                          WindowIntevLikar.GridLoadButton.Visibility = Visibility.Hidden;
                                          WindowIntevLikar.GridGhange.Visibility = Visibility.Hidden;
                                          WindowIntevLikar.GridSave.Visibility = Visibility.Hidden;
                                          WindowIntevLikar.GridDelete.Visibility = Visibility.Hidden;
                                          
                                            // Провести опитування пацієнта
                                            CheckLoadKabinetPacient();
                                          if (_pacientProfil == "") MethodLoadPacientProfil();

                                      if (_pacientProfil == "") { WindowProfilPacient.ControlPacient.SelectedIndex = 0; return; } 
                                          MethodStartInteviewPacient();
                                      WindowProfilPacient.ControlPacient.SelectedIndex = 0;
                                      break;
                                      case 2:
                                          WindowIntevLikar.GridAdd.Visibility = Visibility.Hidden;
                                          WindowIntevLikar.GridGhange.Visibility = Visibility.Hidden;
                                          WindowIntevLikar.GridSave.Visibility = Visibility.Hidden;
                                          // Завантажити показники проведених інтервью
                                          CheckLoadKabinetPacient();
                                          if (_pacientProfil == "") MethodLoadPacientProfil();
                                          if (_pacientProfil == "") { WindowProfilPacient.ControlPacient.SelectedIndex = 0; return; }

                                      break;

                                  }
                                  break;
                          // Закладка лікар
                          case 2:
                                  WindowIntevLikar.GridLoadButton.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridAdd.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridGhange.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridSave.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridPrint.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridDelete.Visibility = Visibility.Visible;
                                  switch (WindowProfilPacient.ControlLikar.SelectedIndex)
                                  {
                                      case 1:
                                          WindowIntevLikar.GridLoadButton.Visibility = Visibility.Hidden;
                                          WindowIntevLikar.GridGhange.Visibility = Visibility.Hidden;
                                          WindowIntevLikar.GridSave.Visibility = Visibility.Hidden;
                                          WindowIntevLikar.GridDelete.Visibility = Visibility.Hidden;
                                          if (_kodDoctor == "") MethodloadProfilLikar(); // WarningMessageOfProfilLikar();
                                          if (_kodDoctor == "") { WindowProfilPacient.ControlLikar.SelectedIndex = 0; return; }
                                          MethodStartInterviewLikar();

                                      break;
                                      case 2:
                                          WindowIntevLikar.GridAdd.Visibility = Visibility.Hidden;
                                          WindowIntevLikar.GridGhange.Visibility = Visibility.Hidden;
                                          WindowIntevLikar.GridSave.Visibility = Visibility.Hidden;
                                          CheckLoadKabinetLikar();
                                          if (_kodDoctor == "") MethodloadProfilLikar(); // WarningMessageOfProfilLikar();
                                          if (_kodDoctor == "") { WindowProfilPacient.ControlLikar.SelectedIndex = 0; return; }
                                      break;

                                  }
                                  break;
                          // Закладка Администрування
                          case 3:
                              WindowIntevLikar.GridLoadButton.Visibility = Visibility.Visible;
                              WindowProfilPacient.ControlLikar.SelectedIndex = 0;
                              if (MapOpisViewModel.boolSetAccountUser == true)
                              {
                                  WindowIntevLikar.GridAdd.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridGhange.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridSave.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridPrint.Visibility = Visibility.Visible;
                                  WindowIntevLikar.GridDelete.Visibility = Visibility.Visible;
                              }

                              switch (WindowProfilPacient.ControlAdmin.SelectedIndex)
                              {
                                  case 0:
                                      WindowIntevLikar.GridAdd.Visibility = Visibility.Hidden;
                                      WindowIntevLikar.GridGhange.Visibility = Visibility.Hidden;
                                      WindowIntevLikar.GridSave.Visibility = Visibility.Hidden;
                                      WindowIntevLikar.GridPrint.Visibility = Visibility.Hidden;
                                      WindowIntevLikar.GridDelete.Visibility = Visibility.Hidden;
                                      break;
                                  case 1:
                                      if (ViewAccountUsers == null) MethodLoadAccountUser();
                                      break;
                                  case 2:
                                      if (ViewStatustUsers == null) MethodLoadNsiStatusUser();
                                      break;
 
                              }
                              break;
                          // Закладка про програму
                          case 4:
                                  WindowIntevLikar.GridLoadButton.Visibility = Visibility.Hidden;
                                  WindowIntevLikar.GridAdd.Visibility = Visibility.Hidden;
                                  WindowIntevLikar.GridGhange.Visibility = Visibility.Hidden;
                                  WindowIntevLikar.GridSave.Visibility = Visibility.Hidden;
                                  WindowIntevLikar.GridPrint.Visibility = Visibility.Hidden;
                                  WindowIntevLikar.GridDelete.Visibility = Visibility.Hidden;
                                  break;
                          }                     

                  }));
            }
        }

        private RelayCommand? visibleHiddenAddEditSave;
        public RelayCommand VisibleHiddenAddEditSave
        {
            get
            {
                return visibleHiddenAddEditSave ??
                  (visibleHiddenAddEditSave = new RelayCommand(obj =>
                  {

                      switch (WindowProfilPacient.ControlLikar.SelectedIndex)
                      {

                          case 2:
                              WindowIntevLikar.GridAdd.Visibility = Visibility.Hidden;
                              WindowIntevLikar.GridGhange.Visibility = Visibility.Hidden;
                              WindowIntevLikar.GridSave.Visibility = Visibility.Hidden;
                              
                              break;
                          default:
                              WindowIntevLikar.GridAdd.Visibility = Visibility.Visible;
                              WindowIntevLikar.GridGhange.Visibility = Visibility.Visible;
                              WindowIntevLikar.GridSave.Visibility = Visibility.Visible;
                              break;
                      }

                      
                  }));
            }
        }
        
        // загрузка інформації по нажатию клавиши Завантажити
        private RelayCommand? loadCommand;
        public RelayCommand LoadCommand
        {
            get
            {
                return loadCommand ??
                  (loadCommand = new RelayCommand(obj =>
                  {
                      // Загрузка данных о гостях, пациентах, докторах, учетных записях
                      switch (WindowProfilPacient.ControlMain.SelectedIndex)
                      {
                          // Закладка Гость
                          case 0:
                              switch (WindowProfilPacient.ControlGuest.SelectedIndex)
                              {
                                  case 1:
                                      // Завантажити записи гостя до лікаря
                                      MethodLoadReceptionLIkarGuest(); 
                                      break;
                              }
                              break;
                          // Закладка Пациент
                          case 1:
                              switch (WindowProfilPacient.ControlPacient.SelectedIndex)
                              {
                                  case 0:
                                      // Завантажити профіль пацієнта
                                      CheckLoadKabinetPacient();
                                      MethodLoadPacientProfil();
                                      if (_pacientProfil == "") return;
                                      break;
                                  case 1:
                                      // Провести опитування пацієнта
                                      CheckLoadKabinetPacient();
                                      if (_pacientProfil == "") MethodLoadPacientProfil();
                                      if (_pacientProfil == "") return;
                                      MethodStartInteviewPacient();

                                      break;
                                  case 2:
                                      // Завантажити показники проведених інтервью
                                      CheckLoadKabinetPacient();
                                      if (_pacientProfil == "") MethodLoadPacientProfil();
                                      if (_pacientProfil == "") return;
                                      MethodLoadtableColectionIntevPacient();   
                                      break;
                                  case 3:
                                      // Завантажити реєстр запсів до лікаря
                                      CheckLoadKabinetPacient();
                                      if (_pacientProfil == "") MethodLoadPacientProfil();
                                      if (_pacientProfil == "") return;
                                      MethodLoadReceptionPacient();   //WarningMessageLoadProfilPacient(); 
                                      break;
                                  case 4:
                                      // Завантажити показники стану здоровья
                                      CheckLoadKabinetPacient();
                                      if (_pacientProfil == "") MethodLoadPacientProfil();
                                      if (_pacientProfil == "") return;
                                      MethodLoadStanHealthPacient();  //WarningMessageLoadProfilPacient(); 
                                      break;
                              }
                              break;
                          // Закладка Доктор
                          case 2:
                              switch (WindowProfilPacient.ControlLikar.SelectedIndex)
                              {
                                  case 0:
                                      // Завантажити профіль лікаря
                                      CheckLoadKabinetLikar();
                                      MethodloadProfilLikar();
                                      break;
                                  case 1:
                                      // Провести опитування лікарем
                                      if (_kodDoctor == "") MethodloadProfilLikar(); // WarningMessageOfProfilLikar();
                                      if (_kodDoctor == "") return;
                                      MethodStartInterviewLikar();
                                      break;
                                      
                                  case 2:
                                      // Завантажити показники проведених інтервью
                                      CheckLoadKabinetLikar();
                                      if (_kodDoctor == "") MethodloadProfilLikar(); // WarningMessageOfProfilLikar();
                                      if (_kodDoctor == "") return;
                                      MethodLoadtableColectionIntevLikar(); 
                                      break;
                                  case 3:
                                      // Завантажити реєстр прийому пацієнтів до лікаря
                                      CheckLoadKabinetLikar();
                                      if (_kodDoctor == "") MethodloadProfilLikar(); // WarningMessageOfProfilLikar();
                                      if (_kodDoctor == "") return;
                                      MethodLoadReceptionLikar(); 
                                      break;
                                  case 4:
                                      // Завантажити розклад прийому пацієнтів до лікаря
                                      CheckLoadKabinetLikar();
                                      if (_kodDoctor == "") MethodloadProfilLikar(); // WarningMessageOfProfilLikar();
                                      if (_kodDoctor == "") return;
                                      MethodLoadVisitingDays();
                                      break;
                                  case 5:
                                      // Просмотр действующих диагнозов в библиотеке диагонозов системы
                                      CheckLoadKabinetLikar();
                                      if (_kodDoctor == "") MethodloadProfilLikar();
                                      if (_kodDoctor == "") return;
                                      MethodViewWorkDiagnoz();
                                      break;
                                  case 6:
                                      // Просмотр действующих диагнозов в библиотеке диагонозов системы
                                      CheckLoadKabinetLikar();
                                      if (_kodDoctor == "") MethodloadProfilLikar();
                                      if (_kodDoctor == "") return;
                                      MethodViewLIbDiagnoz();
                                      break;
                              }
                              break;
                          // Закладка Администрирование
                          case 3:
                              switch (WindowProfilPacient.ControlAdmin.SelectedIndex)
                              {
                                  case 0:
                                      // Введення нового облікового записів користувачів
                                      MethodLoadDefaultLanguageUI();
                                      break;
                                  case 1:
                                      // Завантаження облікових записів користувачів
                                      if (loadboolProfilLikar == true && boolSetAccountUser == false)
                                      {
                                          ProfilLikarAdminMessageError();
                                          return;
                                      }
                                      if (loadboolPacientProfil == true && boolSetAccountUser == false)
                                      {
                                          PacientProfilAdminMessageError();
                                          return;
                                      }
                                      MethodLoadAccountUser();
                                      break;
                                  case 2:
                                      // Завантаження статусів користувачів
                                      if (loadboolProfilLikar == true && boolSetAccountUser == false)
                                      {
                                          ProfilLikarAdminMessageError();
                                          return;
                                      }
                                      if (loadboolPacientProfil == true && boolSetAccountUser == false)
                                      {
                                          PacientProfilAdminMessageError();
                                          return;
                                      }
  
                                      MethodLoadNsiStatusUser();
                                      break;
                              }
                              break;
                      }

                  }));
            }
        }

        private void CheckLoadKabinetPacient()
        {
            if (loadboolProfilLikar == true && boolSetAccountUser == false)
            {
                MessageOnOffKabinetPacient();
                if (MapOpisViewModel.DeleteOnOff == false) return;
                ExitCabinetLikar();
            }
 
        }

        private void CheckLoadKabinetLikar()
        {
            if (loadboolPacientProfil == true && boolSetAccountUser == false)
            {
                MessageOnOffKabinetLikar();
                if (MapOpisViewModel.DeleteOnOff == false) return;
                ExitCabinetLikar();
            }

 
        }

        // Додавання інформації по нажатию клавиши Додати
        private RelayCommand? addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {

                      // Додавання данных о гостях, пациентах, докторах, учетных записях
                      switch (WindowProfilPacient.ControlMain.SelectedIndex)
                      {
                          // Закладка Гость
                          case 0:
                              switch (WindowProfilPacient.ControlGuest.SelectedIndex)
                              {
                                  case 0:
                                      // Додати  інтервью
                                      MethodStartInterview();
                                      break;
                                  case 1:
                                      // Додати запис на прийом до лікаря
                                      MethodAddReceptionLIkarGuest();
                                      break;
                              }
                              break;
                          // Закладка Пациент
                          case 1:
 
                              switch (WindowProfilPacient.ControlPacient.SelectedIndex)
                              {
                                  case 0:
                                      // Додати новий профіль пацієнта
                                      if (loadboolProfilLikar == true)
                                      {
                                          
                                          MessageOnOffKabinetPacient();
                                          if (MapOpisViewModel.DeleteOnOff == false) return;
                                          ExitCabinetLikar();
                                      }
                                      if (loadboolAccountUser == true)
                                      {
                                          MessageOnOffKabinetAdmin();
                                          if (MapOpisViewModel.DeleteOnOff == false) return;
                                          ExitCabinetLikar();
                                      }
                                      MethodaddcomPacientProfil();
                                      break;
                                  case 1:
                                      // Провести нове інтервью
                                      if (_pacientProfil == "") MethodLoadPacientProfil();
                                      if (_pacientProfil == "") return;
                                      MethodStartInteviewPacient(); // WarningMessageOfProfilPacient();
                                      break;
                                  case 3:
                                      // Запис на прийом до лікаря
                                      if (_pacientProfil == "") MethodLoadPacientProfil();
                                      if (_pacientProfil == "") return;
                                      MethodAddReceptionPacient(); // WarningMessageLoadProfilPacient();
                                      break;
                                  case 4:
                                      // ДОдати процедура введення нових показників стану здоовья
                                      if (_pacientProfil == "") MethodLoadPacientProfil();
                                      if (_pacientProfil == "") return;
                                      MethodAddStanHealthPacient(); // WarningMessageLoadProfilPacient();
                                      break;
                              }
                              break;
                          // Закладка Доктор
                          case 2:

                              switch (WindowProfilPacient.ControlLikar.SelectedIndex)
                              {
                                  case 0:
                                      // Введення нового профілю лікаря
                                      if (loadboolPacientProfil == true)
                                      {
                                          MessageOnOffKabinetLikar();
                                          if (MapOpisViewModel.DeleteOnOff == false) return;
                                          ExitCabinetLikar();
                                      }
                                      if (loadboolAccountUser == true)
                                      {
                                          MessageOnOffKabinetAdmin();
                                          if (MapOpisViewModel.DeleteOnOff == false) return;
                                          ExitCabinetLikar();
                                      }
                                      MessageAddOffProfilLikar();
                                      //MethodloadProfilLikar();
                                      //MethodAddNewProfilLIkar();
                                      break;
                                  case 1:
                                      // Старт інтервью яке проводить лікар
                                      if (_kodDoctor == "") MethodloadProfilLikar();
                                      if (_kodDoctor == "") return;
                                      MethodStartInterviewLikar();
                                      break;
                                  case 3:
                                      // Метод введення нового дозапису в чергу прийомів пацієнтів самим лікарем
                                      if (_kodDoctor == "") MethodloadProfilLikar();
                                      if (_kodDoctor == "") return;
                                      AddComandReceptionLikar();
                                      break;
                                  case 4:
                                      // Метод введення нового дозапису в розклад прийому пацієнтів самим лікарем
                                      if (_kodDoctor == "") MethodloadProfilLikar();
                                      if (_kodDoctor == "") return;
                                      MethodAddVisitingDays();
                                      break;
                                  case 5:
                                      // Метод введення нового дозапису в гурт напрямків лікаря
                                      if (_kodDoctor == "") MethodloadProfilLikar();
                                      if (_kodDoctor == "") return;
                                      MethodaddcomWorkDiagnoz();
                                      break;


                              }
                              break;
                          // Закладка Администрирование
                          case 3:

                              switch (WindowProfilPacient.ControlAdmin.SelectedIndex)
                              {
  
                                  case 1:
                                      // Введення нового облікового записів користувачів
                                      AddComandAccountUser();
                                      break;
                                  case 2:
                                      // Введення нового статусу користувачів
                                      AddComandNsiStatusUser();
                                      break;
                              }
                              break;
                      }

                  }));
            }
        }

        // Коригування інформації по нажатию клавиши Змінити
        private RelayCommand? ediCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return ediCommand ??
                  (ediCommand = new RelayCommand(obj =>
                  {

                      // Коригування данных про гостях, пациентах, докторах, учетных записях
                      switch (WindowProfilPacient.ControlMain.SelectedIndex)
                      {
                          // Закладка Гость
                          case 0:
                              switch (WindowProfilPacient.ControlGuest.SelectedIndex)
                              {
                                  case 0:
                                      // Коригування  запису інтервью
                                      MethodEditCompletedInterview();
                                      break;
                                  case 1:
                                      // Коригування  запису на прийом до лікаря
                                      MethodEditReceptionLIkarGuest();
                                      break;
                              }
                              break;
                          // Закладка Пациент
                          case 1:
                              switch (WindowProfilPacient.ControlPacient.SelectedIndex)
                              {
                                  case 0:
                                      // Коригування профіля пацієнта
                                      MethodEditProfilPacient();
                                      break;
                                  case 1:
                                      // Коригування  запису інтервью
                                      MethodEditInteviewPacient();
                                      break;
                                  //case 2:
                                  //    // Коригування показників проведеного інтервью

                                  //    break;
                                  case 3:
                                      // Коригування Запису на прийом до лікаря
                                      MethodEditReceptionPacient();
                                      break;
                                  case 4:
                                      // Коригування показників стану здоовья
                                      MethodEditStanHealthPacient();
                                      break;
                              }
                              break;
                          // Закладка Доктор
                          case 2:
                              switch (WindowProfilPacient.ControlLikar.SelectedIndex)
                              {
                                  case 0:
                                      // Коригування профілю лікаря
                                      MethodEditProfilLikar();
                                      break;
                                  case 1:
                                      // Коригування  запису інтервью
                                      MethodEditInterviewLikar();
                                      break;
                                  case 2:
                                      // Коригування інтервью яке проводить лікар
                                      MethodEditCompletedInterviewLikar();
                                      break;
                                  case 3:
                                      // Метод Коригування запису в чергу прийомів пацієнтів самим лікарем
                                      MethodEditReceptinLikar();
                                      break;
                                  case 4:
                                      // Метод Коригування запису в розкладі прийому пацієнтів самим лікарем
                                      MethodEditVisitingDays();
                                      break;
                              }
                              break;
                          // Закладка Администрирование
                          case 3:
                              switch (WindowProfilPacient.ControlAdmin.SelectedIndex)
                              {
                                  case 1:
                                      // Коригування облікових записів користувачів
                                      MethodEditAccountUser();
                                      break;
                                  case 2:
                                      // Коригування статусу користувачів
                                      MethodEditStatusUser();
                                      break;
                              }
                              break;
                      }

                  }));
            }
        }

        // Видалення інформації по нажатию клавиши Стерти
        private RelayCommand? removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      MainWindow.MessageError = "Увага!" + Environment.NewLine +
                        "Ви дійсно бажаєте стерти облікові данні?" ;
                      SelectedRemove();
                      // Видалення данных о гостях, пациентах, докторах, учетных записях
                      if (MapOpisViewModel.DeleteOnOff == true)
                      { 
                          switch (WindowProfilPacient.ControlMain.SelectedIndex)
                          {
                              // Закладка Гость
                              case 0:
                                  switch (WindowProfilPacient.ControlGuest.SelectedIndex)
                                  {
                                      case 0:
                                          // Видалення  інтервью
                                          MethodRemoveCompletedGuestInterview();
                                          break;
                                      case 1:
                                          // Видалення  запису на прийом до лікаря
                                          MethodRemoveReceptionLIkarGuest();
                                          break;
                                  }
                                  break;
                              // Закладка Пациент
                              case 1:
                                  switch (WindowProfilPacient.ControlPacient.SelectedIndex)
                                  {
                                      case 0:
                                          // Видалення профіля пацієнта
                                          MethodRemovePacientProfil();
                                          break;
                                      case 1:
                                          // Видалення інтервью
                                          MethodRemoveCompletedPacient();
                                          break;
                                      case 2:
                                          // Видалення запису проведеного інтервью із реєстра проведених інтервью
                                          MethodRemoveColectionIntevPacient();
                                          break;
                                      case 3:
                                          // Видалення Запису на прийом до лікаря
                                          MethodRemoveReceptionPacient();
                                          break;
                                      case 4:
                                          // Видалення Запису показників стану здоовья
                                          MethodRemoveStanHealthPacient();
                                          break;
                                  }
                                  break;
                              // Закладка Доктор
                              case 2:
                                  switch (WindowProfilPacient.ControlLikar.SelectedIndex)
                                  {
                                      case 0:
                                          // Видалення  профілю лікаря
                                          MethodRemoveProfilLikar();
                                          break;
                                      case 1:
                                          // Видалення поточного інтервью яке провів лікар
                                          MethodRemoveCompletedInterviewLikar();
                                          break;
                                      case 2:
                                          // Видалення інтервью яке провів лікар із реєстра проведених інтервью
                                          MethodRemoveColectionIntevLikar();
                                          break;
                                      case 3:
                                           // Видалення запису в чергу прийомів пацієнтів самим лікарем
                                          MethodRemoveReceptionLikar();
                                          break;
                                      case 4:
                                          // Видалення запису в чергу прийомів пацієнтів самим лікарем
                                          MethodRemoveVisitingDays();
                                          break;
                                  }
                                  break;
                              // Закладка Администрирование
                              case 3:
                                  switch (WindowProfilPacient.ControlAdmin.SelectedIndex)
                                  {
                                      case 1:
                                          // Видалення  облікового запису
                                          MethodRemoveAccountUser();
                                          break;
                                      case 2:
                                          // Видалення  статусу користувача
                                          MethodRemoveNsiStatusUser();
                                          break;
                                  }
                                  break;
                          }
                      }
                      

                  }));
            }
        }

        // Збереження інформації по нажатию клавиши Зберегти
        private RelayCommand? saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {

                      // Видалення данных о гостях, пациентах, докторах, учетных записях
                      switch (WindowProfilPacient.ControlMain.SelectedIndex)
                      {
                          // Закладка Гость
                          case 0:
                              switch (WindowProfilPacient.ControlGuest.SelectedIndex)
                              {
                                  case 0:
                                      // Збереження  інтервью
                                      MethodSaveCompletedGuestInterview();
                                      break;
                                  case 1:
                                      // Збереження  запису на прийом до лікаря
                                      MethodSaveReceptionLIkarGuest();
                                      break;
 
                              }
                              break;
                          // Закладка Пациент
                          case 1:
                              switch (WindowProfilPacient.ControlPacient.SelectedIndex)
                              {
                                  case 0:
                                      // Збереження профіля пацієнта
                                      MethodSavePacientProfil();
                                      break;
                                  case 1:
                                      // Збереження інтервью
                                      MethodSaveInterviewPacient();
                                      break;
                                  case 2:
                                      // Збереження запису проведеного інтервью
                                      MethodSaveColectionIntevPacient();
                                      break;
                                  case 3:
                                      // Збереження нових записів у чергу до лікаря
                                      MethodSaveReceptionPacient();
                                      break;
                                  case 4:
                                      // Збереження нових показників стану здоовья
                                      MethodSaveStanHealthPacient();
                                      break;
                              }
                              break;
                          // Закладка Доктор
                          case 2:
                              switch (WindowProfilPacient.ControlLikar.SelectedIndex)
                              {
                                  case 0:
                                      // Збереження  профілю лікаря
                                      MethodSaveProfilLikar();
                                      break;
                                  case 1:
                                      // Збереження інтервью яке проводить лікар
                                      MethodSaveInterviewLikar();
                                      break;
                                  case 2:
                                      // Збереження запису проведеного інтервью яке проводить лікар
                                      MethodSaveColectionIntevLikar();
                                      break;
                                  case 3:
                                      // Збереження запису в чергу прийомів пацієнтів самим лікарем
                                      MethodSaveReceptionLikar();
                                      break;
                                  case 4:
                                      // Збереження запису в чергу прийомів пацієнтів самим лікарем
                                      MethodSaveVisitingDays();
                                      break;
                              }
                              break;
                          // Закладка Администрирование
                          case 3:
                              switch (WindowProfilPacient.ControlAdmin.SelectedIndex)
                              {
                                  case 0:
                                      // Збереження  мови за умовчанням
                                      SaveDefaultLang();
                                      break;
                                  case 1:
                                      // Збереження  облікового запису
                                      MethodSaveAccountUser();
                                      break;
                                  case 2:
                                      // Збереження статусу користувача
                                      MethodSaveNsiStatusUser();
                                      break;
                              }
                              break;
                      }

                  }));
            }
        }

        // Друк інформації по нажатию клавиши Зберегти
        private RelayCommand? printCommand;
        public RelayCommand PrintCommand
        {
            get
            {
                return printCommand ??
                  (printCommand = new RelayCommand(obj =>
                  {

                      // Видалення данных о гостях, пациентах, докторах, учетных записях
                      switch (WindowProfilPacient.ControlMain.SelectedIndex)
                      {
                          // Закладка Гость
                          case 0:
                              switch (WindowProfilPacient.ControlGuest.SelectedIndex)
                              {
                                  case 0:
                                      // Друк  інтервью
                                      if(modelColectionInterview !=null) MethodPrintCompletedInterviewGuest();
                                      break;
                                  case 1:
                                      // Друк  запису на прийом до лікаря
                                      if (_kodDoctor != "") MethodPrintReceptionLIkarGuest();
                                      break;
                                  
                                     
                                     
                                     
                              }
                              break;
                          // Закладка Пациент
                          case 1:
                              switch (WindowProfilPacient.ControlPacient.SelectedIndex)
                              {
                                  case 0:
                                      // Друк  профіля пацієнта
                                      if (_pacientProfil != "") MethodPrintPacientProfil();
                                      break;
                                  case 1:
                                      // Друк  інтервью
                                      if (_pacientProfil != "") MethodPrintCompletedInterviewPacient();
                                      break;
                                  case 2:
                                      // Друк результатів проведеного інтервью
                                      if (_pacientProfil != "") MethodPrintColectionIntevPacient();
                                      break;
                                  case 3:
                                      // Друк  запису на прийом до лікаря
                                      if (_pacientProfil != "") MethodPrintReceptionPacient();
                                      break;
                                  case 4:
                                      // Друк  нових показників стану здоовья
                                      if (_pacientProfil != "") MethodPrintStanHealthPacient();
                                      break;
                              }
                              break;
                          // Закладка Доктор
                          case 2:
                              switch (WindowProfilPacient.ControlLikar.SelectedIndex)
                              {
                                  case 0:
                                      // Друк   профілю лікаря
                                      if (_kodDoctor != "") MethodPrintProfilLikar();
                                      break;
                                  case 1:
                                      // Друк інтервью яке проводить лікар
                                      if (_kodDoctor != "") MethodPrintCompletedInterviewLikar();
                                      break;
                                  case 2:
                                      // Друк  проведених інтервью лікарем
                                      if (_kodDoctor != "") MethodPrintColectionIntevLikar();
                                      break;
                                  case 3:
                                      // Друк  запису в чергу прийомів пацієнтів самим лікарем
                                      if (_kodDoctor != "") MethodPrintReceptionLikar();
                                      break;
                                  case 4:
                                      // Друк  запису в чергу прийомів пацієнтів самим лікарем
                                      if (_kodDoctor != "") MethodPrintVisitingDays();
                                      break;
                              }
                              break;
                          // Закладка Администрирование
                          case 3:
                              switch (WindowProfilPacient.ControlAdmin.SelectedIndex)
                              {
                                  case 1:
                                      // Друк   облікового запису
                                      if(loadboolAccountUser == true) MethodPrintAccountUser();
                                      break;
                                  case 2:
                                      // Друк  статусу користувача
                                      if (loadboolAccountUser == true) MethodPrintNsiStatusUser();
                                      break;
                              }
                              break;
                      }

                  }));
            }
        }

        // запуск потока слежения за пасивностью клиента
        public static void RunGifWait()
        {
 
            MainWindow.RenderInfo Arguments01 = new MainWindow.RenderInfo();
            Arguments01.argument1 = "1";
            Thread thread = new Thread(RunWinGifWait);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true; // Фоновый поток
            thread.Start(Arguments01);
        }
            


        // 
        public static void RunWinGifWait(object ThreadObj)
        {
            WaitWindow NewOrder = new WaitWindow("", 2, 240);
            NewOrder.Left = (MainWindow.ScreenWidth / 2);
            NewOrder.Top = (MainWindow.ScreenHeight / 2);
            NewOrder.ShowDialog();
            NewOrder.Close();

        }

    }

}
