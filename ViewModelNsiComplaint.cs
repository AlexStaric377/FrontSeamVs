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

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com

namespace FrontSeam
{
    public class ViewModelNsiComplaint : BaseViewModel
    {
        private NsiComplaint WindowMen = MainWindow.LinkMainWindow("NsiComplaint");
        public static string pathComplaint = "/api/ApiControllerComplaint/";
        public  static ModelComplaint selectedComplaint;
        public static ModelComplaint remComplaint;
        public static ObservableCollection<ModelComplaint> NsiComplaints { get; set; }
        public static ObservableCollection<ModelComplaint> tmpComplaints { get; set; }
        public static int CountComplaint = 0;
        
        public ModelComplaint SelectedComplaint
        { get { return selectedComplaint; } set { selectedComplaint = value; OnPropertyChanged("SelectedComplaint"); } }
        // конструктор класса
        public ViewModelNsiComplaint()
        {

            CallServer.PostServer(pathComplaint, pathComplaint, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewComplaints(CmdStroka);
        }

        public static void ObservableViewComplaints(string CmdStroka)
        {
            List<ModelComplaint> res = JsonConvert.DeserializeObject<ListModelComplaint>(CmdStroka).ViewComplaint.ToList();
            NsiComplaints = new ObservableCollection<ModelComplaint>((IEnumerable<ModelComplaint>)res);

        }

        // команда закрытия окна
        RelayCommand? closeComplaint;
        public RelayCommand CloseComplaint
        {
            get
            {
                return closeComplaint ??
                  (closeComplaint = new RelayCommand(obj =>
                  { 
                      //NsiComplaint WindowMen = MainWindow.LinkMainWindow("NsiComplaint");
                      WindowMen.Close();
                  }));
            }
        }

        // команда поиска симптома
        RelayCommand? searchComplaint;
        public RelayCommand SearchComplaint
        {
            get
            {
                return searchComplaint ??
                  (searchComplaint = new RelayCommand(obj =>
                  {
                      
                      string jason = pathComplaint + "0/" + WindowMen.PoiskComplaints.Text;
                      CallServer.PostServer(pathComplaint, jason, "GETID");
                      string CmdStroka = CallServer.ServerReturn();
                      if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                      else ObservableViewComplaints(CmdStroka);
                      WindowMen.TablComplaints.ItemsSource = NsiComplaints;

                  }));
            }
        }

        // команда выбора строки из списка жалоб
        RelayCommand? selectCompl;
        public RelayCommand SelectComplaint
        {
            get
            {
                return selectCompl ??
                  (selectCompl = new RelayCommand(obj =>
                  {
                      if (selectedComplaint != null)
                      {
                          MapOpisViewModel.nameFeature3 = selectedComplaint.keyComplaint + ": " + selectedComplaint.name.ToString();
                          
                          WindowMen.TablComplaints.ItemsSource = NsiComplaints;
                          MapOpisViewModel.SelectContentCompleted();
                          Checkmixcompl(selectedComplaint.keyComplaint);
                          if (tmpComplaints.Count == 1) { WindowMen.Close(); return; }
                          NsiComplaints = tmpComplaints;
                      }
                  }));
            }
        }


        private void Checkmixcompl(string keyComplaint = "")
        {
            string listkeyComplaint = keyComplaint;
            tmpComplaints = new ObservableCollection<ModelComplaint>();
            foreach (ModelComplaint modelComplaint in NsiComplaints)
            {
                tmpComplaints.Add(modelComplaint);
            }

            foreach (ModelComplaint modelComplaint in NsiComplaints)
            {
                if (keyComplaint != modelComplaint.keyComplaint)
                { 
                    listkeyComplaint = keyComplaint +";" + modelComplaint.keyComplaint + ";";
                    CallServer.PostServer(MapOpisViewModel.Interviewcontroller, MapOpisViewModel.Interviewcontroller + "0/0/0/0/" + listkeyComplaint, "GETID");
                    string CmdStroka = CallServer.ServerReturn();
                    var result = JsonConvert.DeserializeObject<ListModelInterview>(CmdStroka);
                    List<ModelInterview> res = result.ModelInterview.ToList();
                    MapOpisViewModel.ModelInterviews = new ObservableCollection<ModelInterview>((IEnumerable<ModelInterview>)res);

                    if (MapOpisViewModel.ModelInterviews.Count == 0)
                    {
                        remComplaint = new ModelComplaint();
                        remComplaint = modelComplaint;
                        tmpComplaints.Remove(remComplaint);
                        
                    }
                }
                

            }
        }

    }
}
