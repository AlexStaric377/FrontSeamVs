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

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    public class ViewModelNsiDetailing : BaseViewModel
    {

        private string pathcontroller = "/api/DetailingController/";
        public static ModelDetailing selectedDetailing;
        public static ObservableCollection<ModelDetailing> NsiModelDetailings { get; set; }
        public ModelDetailing SelectedModelDetailing
        { get { return selectedDetailing; } set { selectedDetailing = value; OnPropertyChanged("SelectedModelDetailing"); } }
        // конструктор класса
        public ViewModelNsiDetailing()
        {
            //NsiDetailing WindowNsiDetailing = MainWindow.LinkMainWindow("NsiDetailing");
            string jason = "";
            if (ViewModelNsiDetailing.NsiModelDetailings == null)
            {
                switch (MapOpisViewModel.ActCompletedInterview)
                {
                    case "FeatureGET":
                        jason = pathcontroller + "0/0/0";
                        break;
                    default:
                        jason = pathcontroller + "0/" + MapOpisViewModel.selectedGuestInterv.kodDetailing + "/0";
                        break;
                }
                CallServer.PostServer(pathcontroller, jason, "GETID");
                string CmdStroka = CallServer.ServerReturn();
                ObservableNsiModelFeatures(CmdStroka);
            }
 
        }
        public static void ObservableNsiModelFeatures(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDetailing>(CmdStroka);
            List<ModelDetailing> res = result.ViewDetailing.ToList();
            NsiModelDetailings = new ObservableCollection<ModelDetailing>((IEnumerable<ModelDetailing>)res);

        }

        // команда закрытия окна
        RelayCommand? closeModelDetailing;
        public RelayCommand CloseModelDetailing
        {
            get
            {
                return closeModelDetailing ??
                  (closeModelDetailing = new RelayCommand(obj =>
                  {
                      NsiDetailing WindowMen = MainWindow.LinkMainWindow("NsiDetailing");
                      WindowMen.Close();
                  }));
            }
        }

        // команда выбора строки харакутера жалобы
        RelayCommand? selectModelDetailing;
        public RelayCommand SelectModelDetailing
        {
            get
            {
                return selectModelDetailing ??
                  (selectModelDetailing = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
                      NsiDetailing WindowMen = MainWindow.LinkMainWindow("NsiDetailing");
                      if (selectedDetailing != null)
                      {
                          bool keyGr = false;

                          if (selectedDetailing.keyGrDetailing != null)
                          {
                              if (selectedDetailing.keyGrDetailing.Length != 0)keyGr = true;
                          }
                          if(keyGr == true && MapOpisViewModel.ActCompletedInterview != "FeatureGET"
                          && MapOpisViewModel.ActCompletedInterview != "Feature")
                          {
                              MapOpisViewModel.selectGrDetailing = selectedDetailing.nameDetailing.ToString().ToUpper();
                              WinNsiGrDetailing NewOrder = new WinNsiGrDetailing();
                              NewOrder.Left = (MainWindow.ScreenWidth / 2)+50;
                              NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350; //350;
                              NewOrder.ShowDialog();
                          }
                          else
                          {
                              MapOpisViewModel.nameFeature3 = selectedDetailing.kodDetailing.ToString() + ":        " + selectedDetailing.nameDetailing.ToString();
                              if (keyGr == false) MapOpisViewModel.SelectContentCompleted();
                          }
                      }
                      WindowMen.TablDeliting.SelectedItem = null;
                  }));
            }
        }

        RelayCommand? viewGrDetaling;
        public RelayCommand ViewGrDetaling
        {
            get
            {
                return viewGrDetaling ??
                  (viewGrDetaling = new RelayCommand(obj =>
                  {
                      NsiDetailing WindowMen = MainWindow.LinkMainWindow("NsiDetailing");
                      if (WindowMen.TablDeliting.SelectedIndex != -1 && MapOpisViewModel.ActCreatInterview != "ActCreatInterview ")
                      {
                          selectedDetailing = NsiModelDetailings[WindowMen.TablDeliting.SelectedIndex];
                          if (selectedDetailing.keyGrDetailing != null && selectedDetailing.keyGrDetailing != "")
                          {
                              string pathcontroller = "/api/GrDetalingController/";
                              string jason = pathcontroller + "0/" + selectedDetailing.keyGrDetailing + "/0";
                              CallServer.PostServer(pathcontroller, jason, "GETID");
                              string CmdStroka = CallServer.ServerReturn();
                              if (CmdStroka.Contains("[]") == false)
                              {
                                  MapOpisViewModel.selectedComplaintname = selectedDetailing.nameDetailing;
                                  MapOpisViewModel.selectGrDetailing = selectedDetailing.nameDetailing.ToString().ToUpper();
                                  WinNsiGrDetailing NewOrder = new WinNsiGrDetailing();
                                  NewOrder.Left = (MainWindow.ScreenWidth / 2);
                                  NewOrder.Top = (MainWindow.ScreenHeight / 2) - 350;
                                  NewOrder.ShowDialog();
                              }
                          }
                      }



                  },
                 (obj) => NsiModelDetailings != null));
            }
        }

    }
}
