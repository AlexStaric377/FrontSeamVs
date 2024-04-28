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
    class ViewModelNsiRecommen : INotifyPropertyChanged
    {
        private string pathcontrapirecom = "/api/RecommendationController/";
        private ModelRecommendation selectedRecommendation;
        public static ObservableCollection<ModelRecommendation> NsiModelRecommendations { get; set; }
        public static ObservableCollection<ModelRecommendation> ModelRecommendations { get; set; }

        public ModelRecommendation SelectedModelRecommendation
        { get { return selectedRecommendation; } set { selectedRecommendation = value; OnPropertyChanged("SelectedModelRecommendation"); } }
        // конструктор класса
        public ViewModelNsiRecommen()
        {
            if (ModelRecommendations == null)
            {
                CallServer.PostServer(pathcontrapirecom,   pathcontrapirecom, "GET");
                string CmdStroka = CallServer.ServerReturn();
                if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
                else ObservableNsiModelRecommendation(CmdStroka);

            }
            else { NsiModelRecommendations = ModelRecommendations; }

        }

        public static void ObservableNsiModelRecommendation(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelRecommendation>(CmdStroka);
            List<ModelRecommendation> res = result.ModelRecommendation.ToList();
            NsiModelRecommendations = new ObservableCollection<ModelRecommendation>((IEnumerable<ModelRecommendation>)res);

        }


        // команда закрытия окна
        RelayCommand? closeModelRecommendation;
        public RelayCommand CloseModelRecommendation
        {
            get
            {
                return closeModelRecommendation ??
                  (closeModelRecommendation = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
                      WinNsiListRecommen WindowMen = MainWindow.LinkMainWindow("WinNsiListRecommen");
                      if (SelectedModelRecommendation != null)
                      {
                          WindowMain.LikarInterviewt5.Text = SelectedModelRecommendation.kodRecommendation.ToString() + ": " + SelectedModelRecommendation.contentRecommendation;
                      }
                      WindowMen.Close();
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
}
