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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Windows.Media;
using System.Diagnostics;

namespace FrontSeam
{
    public partial class MapOpisViewModel : BaseViewModel
    {

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// "Диференційна діагностика стану нездужання людини-SEAM" 
        /// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        /// 
        public static MainWindow WindowGrupDiagnoz = MainWindow.LinkNameWindow("WindowMain");
        private static bool loadboolGrupDiagnoz = false, activeditViewGroupDiagnoz = false;
        public static string controlerGrupDiagnoz = "/api/GrupDiagnozController/";
        public static string controlerGrDiagnoz = "/api/MedGrupDiagnozController/";

        private ModelGrupDiagnoz selectedViewGrupDiagnoz;

        public static ObservableCollection<ModelGrupDiagnoz> ViewGrupDiagnozs { get; set; }
        public static ObservableCollection<ModelLikarGrupDiagnoz> tmpGrupDiagnozs { get; set; }

        public static ObservableCollection<ModelMedGrupDiagnoz> medGrupDiagnozs { get; set; } 
        public ModelGrupDiagnoz SelectedViewGrupDiagnoz
        { get { return selectedViewGrupDiagnoz; } set { selectedViewGrupDiagnoz = value; OnPropertyChanged("SelectedViewGrupDiagnoz"); } }

        public static void ObservableViewGrupDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelGrupDiagnoz>(CmdStroka);
            List<ModelGrupDiagnoz> res = result.ModelGrupDiagnoz.ToList();
            ViewGrupDiagnozs = new ObservableCollection<ModelGrupDiagnoz>((IEnumerable<ModelGrupDiagnoz>)res);
            WindowMen.GrDiagnozTablGrid.ItemsSource = ViewGrupDiagnozs.OrderBy(x=> x.icdGrDiagnoz);
            loadboolGrupDiagnoz = true;
        }

        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>



        // загрузка справочника по нажатию клавиши Завантажити
        private RelayCommand? loadViewGrupDiagnoz;
        public RelayCommand LoadViewGrupDiagnoz
        {
            get
            {
                return loadViewGrupDiagnoz ??
                  (loadViewGrupDiagnoz = new RelayCommand(obj =>
                  {
                      if (CheckStatusUser() == false) return;
                      MethodLoadGrupDiagnoz();
                  }));
            }
        }


        // команда добавления нового объекта
        bool activViewGrupDiagnoz = false;
        private RelayCommand addViewGrupDiagnoz;
        public RelayCommand AddViewGrupDiagnoz
        {
            get
            {
                return addViewGrupDiagnoz ??
                  (addViewGrupDiagnoz = new RelayCommand(obj =>
                  { if (CheckStatusUser() == false) return; AddComViewGrupDiagnoz(); }));
            }
        }

        private void AddComViewGrupDiagnoz()
        {
            if (loadboolGrupDiagnoz == false) MethodLoadGrupDiagnoz();
            MethodAddGrupDiagnoz();

        }

        private void MethodAddGrupDiagnoz()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            
            if (activViewGrupDiagnoz == false) BoolTrueGrupDiagnoz();
            else BoolFalseGrupDiagnoz();
            WindowMen.GrDiagnozTablGrid.SelectedItem = null;

        }

        private static void MethodLoadGrupDiagnoz()
        {
            WindowMen.GrDiagnozload.Visibility = Visibility.Hidden;
            ViewGrupDiagnozs = new ObservableCollection<ModelGrupDiagnoz>();

            string json = controlerLikarGrDiagnoz ;
            CallServer.PostServer(controlerLikarGrDiagnoz, json, "GET");
            ////CallServer.PostServer(controlerGrupDiagnoz, controlerGrupDiagnoz, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewLikarGrDiagnoz(CmdStroka);
            PackLikarGrupDiagnozs();
            WindowMen.GrDiagnozTablGrid.ItemsSource = LikarGrupDiagnozs;
        }


        private static void PackLikarGrupDiagnozs()
        {
            tmpGrupDiagnozs = new ObservableCollection<ModelLikarGrupDiagnoz>();
            foreach (ModelLikarGrupDiagnoz modelLikarGrupDiagnoz in LikarGrupDiagnozs)
            {
                bool add = true;
                foreach (ModelLikarGrupDiagnoz model in tmpGrupDiagnozs)
                {
                    if (model.icdGrDiagnoz == modelLikarGrupDiagnoz.icdGrDiagnoz) add = false;
                }
                if(add == true) tmpGrupDiagnozs.Add(modelLikarGrupDiagnoz);
            }
            LikarGrupDiagnozs = tmpGrupDiagnozs;
        }
        private void BoolTrueGrupDiagnoz()
        {
            if (IndexAddEdit == "addCommand")
            { 
                WindowMen.GrDiagnozt1.IsEnabled = true;
                WindowMen.GrDiagnozt1.Background = Brushes.AntiqueWhite;           
            }
            WindowMen.GrDiagnozt2.IsEnabled = true;
            WindowMen.GrDiagnozt2.Background = Brushes.AntiqueWhite;
            WindowMen.GrDiagnozt5.IsEnabled = true;
            WindowMen.GrDiagnozt5.Background = Brushes.AntiqueWhite;
            WindowMen.GrDiagnozt6.IsEnabled = true;
            WindowMen.GrDiagnozt6.Background = Brushes.AntiqueWhite;
            WindowMen.FolderGroupDiagnoz.Visibility = Visibility.Visible;
            WindowMen.FolderGrMKX.Visibility = Visibility.Visible;
            WindowMen.GrDiagnozTablGrid.IsEnabled = false;
            activViewGrupDiagnoz = true;
            ////if (IndexAddEdit == "addCommand")
            ////{
            ////    WindowMen.BorderLoadGrMKX.IsEnabled = false;
            ////    WindowMen.BorderGhangeGrMKX.IsEnabled = false;
            ////    WindowMen.BorderDeleteGrMKX.IsEnabled = false;
            ////    WindowMen.BorderPrintGrMKX.IsEnabled = false;
            ////}
            ////if (IndexAddEdit == "editCommand")
            ////{
            ////    WindowMen.BorderLoadGrMKX.IsEnabled = false;
            ////    WindowMen.BorderAddGrMKX.IsEnabled = false;
            ////    WindowMen.BorderDeleteGrMKX.IsEnabled = false;
            ////    WindowMen.BorderPrintGrMKX.IsEnabled = false;
            ////}


        }

        private void BoolFalseGrupDiagnoz()
        {
            WindowMen.GrDiagnozt1.IsEnabled = false;
            WindowMen.GrDiagnozt1.Background = Brushes.White;
            WindowMen.GrDiagnozt2.IsEnabled = false;
            WindowMen.GrDiagnozt2.Background = Brushes.White;
            WindowMen.GrDiagnozt5.IsEnabled = false;
            WindowMen.GrDiagnozt5.Background = Brushes.White;
            WindowMen.GrDiagnozt6.IsEnabled = false;
            WindowMen.GrDiagnozt6.Background = Brushes.White;
            WindowMen.FolderGroupDiagnoz.Visibility = Visibility.Hidden;
            WindowMen.FolderGrMKX.Visibility = Visibility.Hidden;
            WindowMen.GrDiagnozTablGrid.IsEnabled = true;
            activViewGrupDiagnoz = false;
            ////WindowMen.BorderLoadGrMKX.IsEnabled = true;
            ////WindowMen.BorderGhangeGrMKX.IsEnabled = true;
            ////WindowMen.BorderDeleteGrMKX.IsEnabled = true;
            ////WindowMen.BorderPrintGrMKX.IsEnabled = true;
            ////WindowMen.BorderAddGrMKX.IsEnabled = true;
        }

        // команда удаления
        private RelayCommand? removeViewGroupDiagnoz;
        public RelayCommand RemoveViewGroupDiagnoz
        {
            get
            {
                return removeViewGroupDiagnoz ??
                  (removeViewGroupDiagnoz = new RelayCommand(obj =>
                  {
                      if (selectedViewGrupDiagnoz != null)
                      {
                          ////if (selectedViewGrupDiagnoz.idUser != RegIdUser && RegUserStatus != "1")
                          ////{
                          ////    InfoRemoveZapis();
                          ////    return;
                          ////}
                          ////SelectedRemove();
                          ////// Видалення данных о гостях, пациентах, докторах, учетных записях
                          ////if (MapOpisViewModel.DeleteOnOff == true)
                          ////{
                          ////    string json = controlerGrupDiagnoz + selectedViewGrupDiagnoz.id.ToString();
                          ////    CallServer.PostServer(controlerGrupDiagnoz, json, "DELETE");
                          ////    ViewGrupDiagnozs.Remove(selectedViewGrupDiagnoz);
                          ////    WindowMen.GrDiagnozTablGrid.ItemsSource = ViewGrupDiagnozs.OrderBy(x => x.icdGrDiagnoz);

                          ////    selectedViewGrupDiagnoz = new  ModelGrupDiagnoz();
                          ////    BoolFalseQualification();
                          ////    WindowMen.GrDiagnozTablGrid.SelectedItem = null;
                          ////}
                      }
                      IndexAddEdit = "";
                  },
                 (obj) => ViewGrupDiagnozs != null));
            }
        }


        // команда  редактировать
       
        private RelayCommand? editViewGroupDiagnoz;
        public RelayCommand? EditViewGroupDiagnoz
        {
            get
            {
                return editViewGroupDiagnoz ??
                  (editViewGroupDiagnoz = new RelayCommand(obj =>
                  {
                      IndexAddEdit = "editCommand";
                      if (selectedViewGrupDiagnoz != null && selectedViewGrupDiagnoz.id !=0)
                      {
                          ////if (selectedViewGrupDiagnoz.idUser != RegIdUser && RegUserStatus != "1")
                          ////{
                          ////    InfoEditZapis();
                          ////    return;
                          ////}
                          if (activeditViewGroupDiagnoz == false)
                          {
                              BoolTrueGrupDiagnoz();
                              activeditViewGroupDiagnoz = true;
                          }
                          else
                          {
                              BoolFalseGrupDiagnoz();
                              activeditViewGroupDiagnoz = false;
                              WindowMen.GrDiagnozTablGrid.SelectedItem = null;
                          }
                      }
                  }));
            }
        }

        // команда сохранить редактирование
        RelayCommand? saveViewGroupDiagnoz;
        public RelayCommand SaveViewGroupDiagnoz
        {
            get
            {
                return saveViewGroupDiagnoz ??
                  (saveViewGroupDiagnoz = new RelayCommand(obj =>
                  {
                      string json = "";
                      if (WindowMen.GrDiagnozt2.Text.Trim().Length != 0)
                      {
                          if (IndexAddEdit == "addCommand")
                          {
                              ////selectedViewGrupDiagnoz.idUser = RegIdUser;
                              selectedViewGrupDiagnoz.nameGrDiagnoz = WindowMen.GrDiagnozt2.Text;
                              json = JsonConvert.SerializeObject(selectedViewGrupDiagnoz);
                              CallServer.PostServer(controlerGrupDiagnoz, json, "POST");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer.Replace("/", "*").Replace("?", "_");
                              ModelGrupDiagnoz Idinsert = JsonConvert.DeserializeObject<ModelGrupDiagnoz>(CallServer.ResponseFromServer);
                              ViewGrupDiagnozs.Add(Idinsert);
                              WindowMen.GrDiagnozTablGrid.ItemsSource = ViewGrupDiagnozs.OrderBy(x => x.icdGrDiagnoz);
                          }
                          else
                          {
                              json = JsonConvert.SerializeObject(selectedViewGrupDiagnoz);
                              CallServer.PostServer(controlerGrupDiagnoz, json, "PUT");
                              CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                              json = CallServer.ResponseFromServer.Replace("/", "*").Replace("?", "_");
                          }

                          if (json.Length > 1024)
                          {
                              selectedViewGrupDiagnoz.uriDiagnoza = "";
                              json = JsonConvert.SerializeObject(selectedViewGrupDiagnoz);
                              if (json.Length > 1024)
                              {
                                  selectedViewGrupDiagnoz.opisDiagnoza = selectedViewGrupDiagnoz.opisDiagnoza.Substring(0, selectedViewGrupDiagnoz.opisDiagnoza.Length - (json.Length - 1024));
                                  json = JsonConvert.SerializeObject(selectedViewGrupDiagnoz);
                              }
                          }
                          CallServer.PostServer(Controlleroutfile, Controlleroutfile + "GrupDiagnoz/" + json + "/0", "GETID");
                          //UnloadCmdStroka("GrupDiagnoz/", json);
                      }
                      WindowMen.GrDiagnozTablGrid.SelectedItem = null;
                      IndexAddEdit = "";
                      BoolFalseGrupDiagnoz();

                  }));
            }
        }

 
        // команда печати
        RelayCommand? printViewGrupDiagnoz;
        public RelayCommand PrintViewGrupDiagnoz
        {
            get
            {
                return printViewGrupDiagnoz ??
                  (printViewGrupDiagnoz = new RelayCommand(obj =>
                  {
                     MessageBox.Show("Груповання діагнозів :" + ViewGrupDiagnozs[0].nameGrDiagnoz.ToString());
                     
                  },
                 (obj) => ViewGrupDiagnozs != null));
            }
        }

        
        // команда загрузки справочника міжнародний класифікатор захворювань 11
        private RelayCommand? addLoadGrMkx;
        public RelayCommand AddLoadGrMkx
        {
            get
            {
                return addLoadGrMkx ??
                  (addLoadGrMkx = new RelayCommand(obj =>
                  { ComandAddLoadGrMkxMkx(); }));
            }
        }

        private void ComandAddLoadGrMkxMkx()
        {
            ////MapOpisViewModel.GrupDiagnoz = "";
            ////WinNsiIcd NewOrder = new WinNsiIcd();
            ////NewOrder.Left = (MainWindow.ScreenWidth / 2)-100;
            ////NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
            ////NewOrder.ShowDialog();
            ////if (WindowMen.Diagnozt4.Text != "")
            ////{

            ////    string tmpkod = WindowMen.Diagnozt4.Text.Substring(0, WindowMen.Diagnozt4.Text.Length - 1);
            ////    string tmpkod1 = tmpkod.Substring(tmpkod.LastIndexOf(".") + 1, tmpkod.Length - tmpkod.LastIndexOf(".") - 1);
            ////    if (tmpkod.Length - tmpkod1.Length >= 3)
            ////    {
            ////        WindowMen.GrDiagnozt1.Text = WindowMen.Diagnozt4.Text + "01.";
            ////    }
            ////    else
            ////    {
            ////        int number = Convert.ToInt32(tmpkod.Substring(tmpkod.LastIndexOf(".") + 1, tmpkod.Length - tmpkod.LastIndexOf(".") - 1)) + 1;
            ////        string addtext = number >= 10 ? Convert.ToString(number) : "0" + Convert.ToString(number) + ".";
            ////        WindowMen.GrDiagnozt1.Text = tmpkod.Substring(0, tmpkod.LastIndexOf(".") + 1) + addtext;
            ////    }

            ////    if (selectedViewGrupDiagnoz == null) selectedViewGrupDiagnoz = new ModelGrupDiagnoz();
            ////    WindowMen.GrDiagnozt2.Text = WindowMen.Diagnozt3.Text;
            ////    selectedViewGrupDiagnoz.icdGrDiagnoz = WindowMen.GrDiagnozt1.Text;
            ////    selectedViewGrupDiagnoz.nameGrDiagnoz = WindowMen.Diagnozt3.Text;
            ////}
 
            
        }


        

        // Выбор названия интервью диагностики 
        private RelayCommand? searchGrDiagnoz;
        public RelayCommand SearchGrDiagnoz
        {
            get
            {
                return searchGrDiagnoz ??
                  (searchGrDiagnoz = new RelayCommand(obj =>
                  {
                      MetodSearchGrDiagnoz();
                  }));
            }
        }
        // команда контроля нажатия клавиши enter
        RelayCommand? checkKeyGrDiagnoz;
        public RelayCommand CheckKeyGrDiagnoz
        {
            get
            {
                return checkKeyGrDiagnoz ??
                  (checkKeyGrDiagnoz = new RelayCommand(obj =>
                  {
                      MetodSearchGrDiagnoz();
                  }));
            }
        }


        private void MetodSearchGrDiagnoz()
        {
            if (CheckStatusUser() == false) return;
            if (WindowGrupDiagnoz.PoiskGrDiagnoz.Text.Trim() != "")
            {
                string jason = controlerGrupDiagnoz + "0/" + WindowGrupDiagnoz.PoiskGrDiagnoz.Text;
                CallServer.PostServer(Interviewcontroller, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableViewGrupDiagnoz(CmdStroka);
            }

        }

        // команда загрузки  строки исх МКХ11 по указанному коду для вівода наименования болезни
        private RelayCommand? findNameGrDiagnoz;
        public RelayCommand FindNameGrDiagnoz
        {
            get
            {
                return findNameWorkMkx ??
                  (findNameWorkMkx = new RelayCommand(obj =>
                  { ComandNameGrDiagnoz(); }));
            }
        }

        private void ComandNameGrDiagnoz()
        {
 
            if (LikarGrupDiagnozs != null && WindowMen.GrDiagnozTablGrid.SelectedIndex >= 0)
            {
                    
                 ModelLikarGrupDiagnoz ViewGrupDiagnoz = LikarGrupDiagnozs[WindowMen.GrDiagnozTablGrid.SelectedIndex];

                MapOpisViewModel.selectIcdGrDiagnoz = ViewGrupDiagnoz.icdGrDiagnoz;
                ViewModelAnalogDiagnoz.Likar = "";
                WinNsiMedZaklad MedZaklad = new WinNsiMedZaklad();
                MedZaklad.ShowDialog();
                MapOpisViewModel.EdrpouMedZaklad = WindowIntevLikar.Likart8.Text.ToString();
                if (MapOpisViewModel.EdrpouMedZaklad.Length > 0)
                {
                    MapOpisViewModel.ModelCall = "ReceptionLIkar";
                    WinNsiLikar NewOrder = new WinNsiLikar();
                    NewOrder.ShowDialog();
                    
                }

                   
             }


  
        }
    }
    #endregion
    #endregion
}
