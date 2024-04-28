using System;
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


namespace FrontSeam
{



    // Рекомендации о дальнейших действиях, в том числе, и по адресному обращению для оказания медицинской помощи к врачам - специалистам медицинских учреждений.

    public partial class ListModelRecommendation
    {
        [JsonProperty("list")]
        public ModelRecommendation[] ModelRecommendation { get; set; }
    }


    public class ModelRecommendation : INotifyPropertyChanged
    {
        public int Id;
        public string KodRecommendation;
        public string ContentRecommendation;


        public ModelRecommendation(int Id = 0, string KodRecommendation = "", string ContentRecommendation = "")
        {
            this.Id = Id;
            this.KodRecommendation = KodRecommendation;
            this.ContentRecommendation = ContentRecommendation;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("kodRecommendation")]
        public string kodRecommendation
        {
            get { return KodRecommendation; }
            set { KodRecommendation = value; OnPropertyChanged("kodRecommendation"); }
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
}
