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
    public partial class ListModelContentInterv
    {

        [JsonProperty("list")]
        public ModelContentInterv[] ModelContentInterv { get; set; }

    }

    public partial class ModelContentInterv : INotifyPropertyChanged
    {
        private int Id;
        private string KodProtokola;
        private string DetailsInterview;
        private string KodDetailing;
        private int Numberstr;

        public ModelContentInterv(int Id = 0, int Numberstr=0, string KodProtokola = "", string KodDetailing="", string DetailsInterview = "", string NametInterview = "")
        {
            this.Id = Id;
            this.KodProtokola = KodProtokola;
            this.KodDetailing = KodDetailing;
            this.DetailsInterview = DetailsInterview;
            this.Numberstr = Numberstr;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodProtokola")]
        public string kodProtokola
        {
            get { return KodProtokola; }
            set { KodProtokola = value; OnPropertyChanged("kodProtokola"); }
        }

        [JsonProperty("numberstr")]
        public int numberstr
        {
            get { return Numberstr; }
            set { Numberstr = value; OnPropertyChanged("numberstr"); }
        }

        [JsonProperty("kodDetailing")]
        public string kodDetailing
        {
            get { return KodDetailing; }
            set { KodDetailing = value; OnPropertyChanged("kodDetailing"); }
        }

        [JsonProperty("detailsInterview")]
        public string detailsInterview
        {
            get { return DetailsInterview; }
            set { DetailsInterview = value; OnPropertyChanged("detailsInterview"); }
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
