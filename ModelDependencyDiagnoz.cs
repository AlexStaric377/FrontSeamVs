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

namespace FrontSeam
{
    // модель взаимосвязи диагонозов рекомендаций и протоколов интервью 

    public partial class ListModelDependencyDiagnoz
    {

        [JsonProperty("list")]
        public ModelDependencyDiagnoz[] ModelDependencyDiagnoz { get; set; }

    }
    public class ModelDependencyDiagnoz : INotifyPropertyChanged
    {

        private int Id;
        private string KodDiagnoz;
        private string KodProtokola;
        private string KodRecommend;
        private string ContentRecommendation;
        private string NameDiagnoza;

        public ModelDependencyDiagnoz(int Id = 0, string KodDiagnoz = "", string KodProtokola = "", string KodRecommend ="", string NameDiagnoza = "", string ContentRecommendation ="")
        {
            this.Id = Id;
            this.NameDiagnoza = NameDiagnoza;
            this.KodProtokola = KodProtokola;
            this.KodDiagnoz = KodDiagnoz;
            this.KodRecommend = KodRecommend;
            this.ContentRecommendation = ContentRecommendation;
        }


        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodDiagnoz")]
        public string kodDiagnoz
        {
            get { return KodDiagnoz; }
            set { KodDiagnoz = value; OnPropertyChanged("kodDiagnoz"); }
        }

        [JsonProperty("nameDiagnoza")]
        public string nameDiagnoza
        {
            get { return NameDiagnoza; }
            set { NameDiagnoza = value; OnPropertyChanged("nameDiagnoza"); }
        }
        [JsonProperty("kodRecommend")]
        public string kodRecommend
        
        { get { return KodRecommend; }
          set { KodRecommend = value; OnPropertyChanged("kodRecommend"); }
        }
        [JsonProperty("kodProtokola")]
        public string kodProtokola
        {
            get { return KodProtokola; }
            set { KodProtokola = value; OnPropertyChanged("kodProtokola"); }
        }

        [JsonProperty("contentRecommendation")]
        public string contentRecommendation
        {
            get { return ContentRecommendation; }
            set { ContentRecommendation = value; OnPropertyChanged("contentRecommendation"); }
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

    public partial class ListModelDependency
    {

        [JsonProperty("list")]
        public ModelDependency[] ModelDependency { get; set; }

    }

    public class ModelDependency : INotifyPropertyChanged
    {
        [JsonProperty("id")]
        public int id { get ;  set ; }
       
        [JsonProperty("kodDiagnoz")]
        public string kodDiagnoz {  get ;  set ; }
       
        [JsonProperty("kodRecommend")]
        public string kodRecommend  {  get ;   set ; }
       
        [JsonProperty("kodProtokola")]
        public string kodProtokola   {get;  set ; }
        

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
