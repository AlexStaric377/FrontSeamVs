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
    class ViewModelNsiFeature : INotifyPropertyChanged
    {
        private string controller = "/api/FeatureController/";
        private ModelFeature selectedFeature;
        public static ObservableCollection<ModelFeature> NsiModelFeatures { get; set; }
        public ModelFeature SelectedModelFeature
        { get { return selectedFeature; } set { selectedFeature = value; OnPropertyChanged("SelectedModelFeature"); } }
        // конструктор класса
        public ViewModelNsiFeature()
        {
    
                switch (MapOpisViewModel.ActCompletedInterview)
                {

                    case null:
                        CallServer.PostServer(controller, controller, "GET");
                        break;
                    default:
                        CallServer.PostServer(controller, controller +"0/" + MapOpisViewModel.selectedGuestInterv.kodDetailing, "GETID");
                        break;
                }


                
                string CmdStroka = CallServer.ServerReturn();
                ObservableNsiModelFeatures(CmdStroka);

            
            
        }
        public static void ObservableNsiModelFeatures(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelFeature>(CmdStroka);
            List<ModelFeature> res = result.ModelFeature.ToList();
            NsiModelFeatures = new ObservableCollection<ModelFeature>((IEnumerable<ModelFeature>)res);
        }

        // команда закрытия окна
        RelayCommand? closeModelFeature;
        public RelayCommand CloseModelFeature
        {
            get
            {
                return closeModelFeature ??
                  (closeModelFeature = new RelayCommand(obj =>
                  {
                      WinNsiFeature WindowMen = MainWindow.LinkMainWindow("WinNsiFeature");
                      WindowMen.Close();
                  }));
            }
        }

        // команда выбора строки харакутера жалобы
        RelayCommand? selectModelFeature;
        public RelayCommand SelectModelFeature
        {
            get
            {
                return selectModelFeature ??
                  (selectModelFeature = new RelayCommand(obj =>
                  {
                      MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
                      WinNsiFeature WindowMen = MainWindow.LinkMainWindow("WinNsiFeature");
                      if (selectedFeature != null)
                      {
                         
                          MapOpisViewModel.nameFeature3 = selectedFeature.keyFeature.ToString() + ":    " + selectedFeature.name.ToString();
                          MapOpisViewModel.SelectContentCompleted();
                          if(MapOpisViewModel.CallViewDetailing == "ModelDetailing") WindowMen.Close();
                      }
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
