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
namespace FrontSeam
{
    public partial class ViewModelCreatInterview : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }


        bool endwhile = false;
        //public static  WinCreatIntreview WindowUri = MainWindow.LinkMainWindow("WinCreatIntreview");
        public static MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
        public static int Numberstroka = 0, IdItemContentInterv = 0, IndexContentInterv=0;
        private bool booladdprotokol = false, booladdContent = false;
        public static string pathcontroler = "/api/ContentInterviewController/";
        public static string Completedcontroller = "/api/CompletedInterviewController/";
        public static ModelContentInterv selectedContentInterv;
        public static ModelInterview selectedInterview;
        public static ObservableCollection<ModelInterview> ModelInterviews { get; set; }
        public static ObservableCollection<ModelContentInterv> ContentIntervs { get; set; }
        public static ObservableCollection<ModelContentInterv> TmpContentIntervs = new ObservableCollection<ModelContentInterv>();

        public ModelContentInterv SelectedContentInterv
        { get { return selectedContentInterv; } set { selectedContentInterv = value; OnPropertyChanged("SelectedContentInterv"); } }
        // конструктор класса
        public ViewModelCreatInterview()
        {
            LoadCreatInterview();
        }

        public static void LoadCreatInterview()
        { 
           string CmdStroka = "";
            MapOpisViewModel.ActCompletedInterview = null;
            switch (MapOpisViewModel.IndexAddEdit)
            {
                case "":

                    CallServer.PostServer(pathcontroler, pathcontroler + MapOpisViewModel.GetidkodProtokola, "GETID");
                    CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]")) selectedContentInterv = new ModelContentInterv();
                    else ObservableContentInterv(CmdStroka);
                    break;
                case "editCommand":
                    if (MapOpisViewModel.ModelCall == "ModelColectionInterview")
                    {
                        pathcontroler = Completedcontroller;
                    }
                    CallServer.PostServer(pathcontroler, pathcontroler + MapOpisViewModel.GetidkodProtokola, "GETID");
                    CmdStroka = CallServer.ServerReturn();
                    if (CmdStroka.Contains("[]")) selectedContentInterv = new ModelContentInterv();
                    else ObservableContentInterv(CmdStroka);
                    break;
                case "addCommand":
                    ContentIntervs = new ObservableCollection<ModelContentInterv>();
                    break;
            }        
        }


        public static void ObservableContentInterv(string CmdStroka)
        {

            var result = JsonConvert.DeserializeObject<ListModelContentInterv>(CmdStroka);
            List<ModelContentInterv> res = result.ModelContentInterv.ToList();
            ContentIntervs = new ObservableCollection<ModelContentInterv>((IEnumerable<ModelContentInterv>)res);
 
        }
        // команда закрытия окна
        RelayCommand? closeCreatInterview;
        public RelayCommand CloseCreatInterview
        {
            get
            {
                return closeCreatInterview ??
                  (closeCreatInterview = new RelayCommand(obj =>
                  {
                      WinCreatIntreview WindowUri = MainWindow.LinkMainWindow("WinCreatIntreview");
                      WindowUri.Close();
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? saveCreatInterview;
        public RelayCommand SaveCreatInterview
        {
            get
            {
                return saveCreatInterview ??
                  (saveCreatInterview = new RelayCommand(obj =>
                  {
                      if (MapOpisViewModel.ModelCall == "ModelColectionInterview" || MapOpisViewModel.ModelCall == "ModelInterview" || MapOpisViewModel.ModelCall == "")
                      {
                          WinCreatIntreview WindowCreat = MainWindow.LinkMainWindow("WinCreatIntreview");
                          WindowCreat.BorderPlus.Visibility = Visibility.Hidden;
                          WindowCreat.BorderDelete.Visibility = Visibility.Hidden;
                          WindowCreat.BorderSave.Visibility = Visibility.Hidden;
                          return;
                      }

                      WinCreatIntreview WindowUri = MainWindow.LinkMainWindow("WinCreatIntreview");
                      if (ContentIntervs == null) WindowUri.Close();

                      string json = pathcontroler + MapOpisViewModel.GetidkodProtokola + "/0"; //selectedContentInterv.kodProtokola +
                      CallServer.PostServer(pathcontroler, json, "DELETE");
                      selectedInterview.detailsInterview = "";
                      Numberstroka = 0;
                      // ОБращение к серверу добавляем запись в соответствии с сформированным списком
                      foreach (ModelContentInterv modelContentInterv in ContentIntervs.OrderBy(x => x.kodDetailing))
                      {
                          selectedInterview.detailsInterview = selectedInterview.detailsInterview.Length == 0
                          ? modelContentInterv.kodDetailing + ";" : selectedInterview.detailsInterview + modelContentInterv.kodDetailing + ";";
                      }
                      foreach (ModelContentInterv modelContentInterv in ContentIntervs)
                      {

                          modelContentInterv.id = 0;
                          modelContentInterv.numberstr = Numberstroka++;
                          json = JsonConvert.SerializeObject(modelContentInterv);
                          CallServer.PostServer(pathcontroler, json, "POST");
                      }
                      switch (MapOpisViewModel.IndexAddEdit)
                      {
                          case "addCommand":
                              AddInterviewProtokol();
                              break;
                          case "editCommand":
                              EdiInterviewProtokol();
                              break;
                      }
                      WindowUri.Close();
                  }));
            }
        }

        // команда удаления строки интервью
        RelayCommand? deletestrokaInterview;
        public RelayCommand DeletestrokaInterview
        {
            get
            {
                return deletestrokaInterview ??
                  (deletestrokaInterview = new RelayCommand(obj =>
                  {
                      if (MapOpisViewModel.ModelCall == "ModelColectionInterview" || MapOpisViewModel.ModelCall == "ModelInterview")
                      {
                          WinCreatIntreview WindowCreat = MainWindow.LinkMainWindow("WinCreatIntreview");
                          WindowCreat.BorderPlus.Visibility = Visibility.Hidden;
                          WindowCreat.BorderDelete.Visibility = Visibility.Hidden;
                          WindowCreat.BorderSave.Visibility = Visibility.Hidden;
                          return;
                      }
                      if (selectedContentInterv != null)
                      {
                          if (selectedContentInterv.id != 0)
                          {
                              string json = pathcontroler + "0/" + selectedContentInterv.id.ToString(); //selectedContentInterv.kodProtokola +
                              CallServer.PostServer(pathcontroler, json, "DELETE");
                          }
                          ContentIntervs.Remove(selectedContentInterv);
                      }
                  }));
            }
        }

        // команда вывзова окна со списком жалоб для выбора строки  и записи в интервью
        RelayCommand? addstrokaInterview;
        public RelayCommand AddstrokaInterview
        {
            get
            {
                return addstrokaInterview ??
                  (addstrokaInterview = new RelayCommand(obj =>
                  {
                      WinCreatIntreview WindowUri = MainWindow.LinkMainWindow("WinCreatIntreview");
                      if (MapOpisViewModel.ModelCall == "ModelColectionInterview" || MapOpisViewModel.ModelCall == "ModelInterview")
                      {
                          WindowUri.BorderPlus.Visibility = Visibility.Hidden;
                          WindowUri.BorderDelete.Visibility = Visibility.Hidden;
                          WindowUri.BorderSave.Visibility = Visibility.Hidden;
                          return;
                      }
                      if (MapOpisViewModel.IndexAddEdit == "editCommand") booladdprotokol = true;

                      IdItemContentInterv = WindowUri.TablInterviews.SelectedIndex;
                      //IndexContentInterv++;
                      if (selectedContentInterv != null && IdItemContentInterv >= 0)
                      {
                          switch (selectedContentInterv.kodDetailing.Length)
                          {
                              case 5:

                                  WinNsiFeature NewOrder = new WinNsiFeature();
                                  NewOrder.Left = 750;
                                  NewOrder.Top = 300;
                                  NewOrder.ShowDialog();
                                  break;
                              case 9:
                                  NsiDetailing NewNsi = new NsiDetailing();
                                  NewNsi.Left = 750;
                                  NewNsi.Top = 300;
                                  NewNsi.ShowDialog();
                                  break;
                          }
                      }
                      else
                      {
                          NsiComplaint NewOrder = new NsiComplaint();
                          NewOrder.Left = 750;
                          NewOrder.Top = 300;
                          NewOrder.ShowDialog();
                          if (MapOpisViewModel.nameFeature3.Length != 0)WindowUri.TablInterviews.ItemsSource = ContentIntervs;
                      }

                      WindowUri.TablInterviews.SelectedItem = null;
                  }));
            }
        }

 
  


        // метод дозаписи выбранной строки жалобы в общий контекст интервью.
        public static void SelectContentCompl()
        {
            if (MapOpisViewModel.ModelCall == "ModelColectionInterview" || MapOpisViewModel.ModelCall == "ModelInterview")
            {
                WinCreatIntreview WindowCreat = MainWindow.LinkMainWindow("WinCreatIntreview");
                WindowCreat.BorderPlus.Visibility = Visibility.Hidden;
                WindowCreat.BorderDelete.Visibility = Visibility.Hidden;
                WindowCreat.BorderSave.Visibility = Visibility.Hidden;

            }
            int indexcontent = -1;
            bool booladdContent = false, addcontent = false;
             TmpContentIntervs = new ObservableCollection<ModelContentInterv>();
            foreach (ModelContentInterv modelContentInterv in ContentIntervs)   //.OrderBy(x => x.kodDetailing)
            {
                indexcontent++;
                if (IdItemContentInterv == indexcontent && selectedContentInterv != null && addcontent == false) booladdContent = true;
                TmpContentIntervs.Add(modelContentInterv);
                if (booladdContent == true)
                {
                    AddselectedContent();
                    addcontent = true;
                    booladdContent = false;
                    IndexContentInterv = indexcontent;
                }
            }
            if (ContentIntervs.Count == TmpContentIntervs.Count) AddselectedContent();
            ContentIntervs = TmpContentIntervs;
            WinCreatIntreview WindowUri = MainWindow.LinkMainWindow("WinCreatIntreview");
            WindowUri.TablInterviews.ItemsSource = ContentIntervs;
        }

  

        private static void AddselectedContent()
        {
            ModelContentInterv selectedaddContent = new ModelContentInterv();
            selectedaddContent.kodProtokola = selectedInterview.kodProtokola;
            selectedaddContent.kodDetailing = MapOpisViewModel.nameFeature3.Substring(0, MapOpisViewModel.nameFeature3.IndexOf(":"));

            selectedaddContent.detailsInterview = MapOpisViewModel.nameFeature3.Substring(MapOpisViewModel.nameFeature3.IndexOf(":") + 1, MapOpisViewModel.nameFeature3.Length - (MapOpisViewModel.nameFeature3.IndexOf(":") + 1));
            TmpContentIntervs.Add(selectedaddContent);
            IdItemContentInterv++;
        }



        // метод дозаписи данных формируемого интервью в таблицу сформированных интервью
        public void AddInterviewProtokol()
        {
            booladdprotokol = true;
            var json = JsonConvert.SerializeObject(selectedInterview);
            CallServer.PostServer(MapOpisViewModel.Interviewcontroller, json, "POST");
            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
            ModelInterview Idinsert = JsonConvert.DeserializeObject<ModelInterview>(CallServer.ResponseFromServer);
            int Countins = ModelInterviews != null ? ModelInterviews.Count : 0;
            ModelInterviews.Insert(Countins, Idinsert);
            MesageEnd();
        }

        public void EdiInterviewProtokol()
        {
            var json = JsonConvert.SerializeObject(selectedInterview);
            CallServer.PostServer(MapOpisViewModel.Interviewcontroller, json, "PUT");
            MesageEnd();
        }

        private void MesageEnd()
        {
            MainWindow.MessageError = "Збереження складеного інтерв'ю завершено." + Environment.NewLine +
           "Для встановлення відповідного діагнозу та рекомендації за цим інтерв'ю " + Environment.NewLine + "необхідно натиснути на малюнок <Діагноз>";
            MessageWarn NewOrder = new MessageWarn(MainWindow.MessageError, 2, 3);
            NewOrder.ShowDialog();
        }



        RelayCommand? selectComplaint;
        public RelayCommand SelectComplaint
        {
            get
            {
                return selectComplaint ??
                  (selectComplaint = new RelayCommand(obj =>
                  {
                      WinCreatIntreview WindowUri = MainWindow.LinkMainWindow("WinCreatIntreview");
                      IdItemContentInterv = WindowUri.TablInterviews.SelectedIndex+1;
                  }));
            }
        }
    }
}
