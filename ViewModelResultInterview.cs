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
using System.ServiceProcess;
using System.Diagnostics;

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    public class ViewModelResultInterview : BaseViewModel
    {
        private string pathcontroller = "/api/InterviewController/";
        public static ModelInterview selectedResultInterview;
        public static ModelResultInterview selectItogInterview;
        public static ObservableCollection<ModelResultInterview> ResultInterviews { get; set; }
        public ModelResultInterview SelectedResultInterview
        { get { return selectItogInterview; } set { selectItogInterview = value; OnPropertyChanged("SelectedResultInterview"); } }
        // конструктор класса
        public ViewModelResultInterview()
        {

            CallServer.PostServer(pathcontroller, pathcontroller + MapOpisViewModel.modelColectionInterview.kodProtokola + "/0/0/0", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]") == false)
            {
                CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                ModelInterview Interview = JsonConvert.DeserializeObject<ModelInterview>(CallServer.ResponseFromServer);
                selectedResultInterview = Interview;
                         
            }

            selectItogInterview = new ModelResultInterview();
            selectItogInterview.dateInterview = MapOpisViewModel.modelColectionInterview.dateInterview;
            selectItogInterview.nameDiagnoza = MapOpisViewModel.NameDiagnoz;
            selectItogInterview.nameRecommendation = MapOpisViewModel.NameRecomendaciya;
            selectItogInterview.nametInterview = MapOpisViewModel.modelColectionInterview.nameInterview;
            selectItogInterview.opistInterview = selectedResultInterview.opistInterview;
            selectItogInterview.uriInterview = selectedResultInterview.uriInterview;
            SelectedResultInterview = selectItogInterview;

            MapOpisViewModel.modelColectionInterview.nameDiagnoz= MapOpisViewModel.NameDiagnoz;
            MapOpisViewModel.modelColectionInterview.nameRecomen = MapOpisViewModel.NameRecomendaciya;
            MapOpisViewModel.OpistInterview = selectItogInterview.opistInterview;
            MapOpisViewModel.UriInterview = selectItogInterview.uriInterview;
            MapOpisViewModel.NameDiagnoz = selectItogInterview.nameDiagnoza;
            MapOpisViewModel.NameRecomendaciya = selectItogInterview.nameRecommendation;


            ResultInterviews = new ObservableCollection<ModelResultInterview>();
            ResultInterviews.Add(selectItogInterview);
        }


    }
}
