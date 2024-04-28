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

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    // Уточнение класификация детализации данного характера или особености жалобы 

    public partial class ListModelQualification
    {
        [JsonProperty("list")]
        public ModelQualification[] ViewQualification { get; set; }
    }

    public class ModelQualification : INotifyPropertyChanged
    {
        private int Id;
        private string KodGroupQualification;
        private string NameQualification;
        private string KodQualification;
        public ModelQualification(int Id = 0, string KodQualification="", string KodGroupQualification = "", string NameQualification = "")
        {
            this.Id = Id;
            this.KodQualification = KodQualification;
            this.KodGroupQualification = KodGroupQualification;
            this.NameQualification = NameQualification;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodQualification")]
        public string kodQualification
        {
            get { return KodQualification; }
            set { KodQualification = value; OnPropertyChanged("kodQualification"); }
        }

        [JsonProperty("kodGroupQualification")]
        public string kodGroupQualification
        {
            get { return KodGroupQualification; }
            set { KodGroupQualification = value; OnPropertyChanged("kodGroupQualification"); }
        }
        [JsonProperty("nameQualification")]
        public string nameQualification
        {
            get { return NameQualification; }
            set { NameQualification = value; OnPropertyChanged("nameQualification"); }
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
