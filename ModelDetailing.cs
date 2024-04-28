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
    // Детализация особености жалобы 
    public partial class ListModelDetailing
    {

        [JsonProperty("list")]
        public ModelDetailing[] ViewDetailing { get; set; }

    }

 
    public class ModelDetailing : INotifyPropertyChanged
    {

        private int Id;
        private string KeyFeature;
        private string KeyGrDetailing;
        private string NameDetailing;
        private string KodDetailing;


        public ModelDetailing(int Id = 0, string KeyFeature = "", string KodDetailing="", string KeyGrDetailing = "", string NameDetailing = "")
        {
            this.Id = Id;
            this.KeyFeature = KeyFeature;
            this.KeyGrDetailing = KeyGrDetailing;
            this.NameDetailing = NameDetailing;
            this.KodDetailing = KodDetailing;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("keyFeature")]
        public string keyFeature
        {
            get { return KeyFeature; }
            set { KeyFeature = value; OnPropertyChanged("keyFeature"); }
        }
        [JsonProperty("kodDetailing")]
        public string kodDetailing
        {
            get { return KodDetailing; }
            set { KodDetailing = value; OnPropertyChanged("kodDetailing"); }
        }

        [JsonProperty("keyGrDetailing")]
        public string keyGrDetailing
        {
            get { return KeyGrDetailing; }
            set { KeyGrDetailing = value; OnPropertyChanged("keyGrDetailing"); }
        }

        [JsonProperty("nameDetailing")]
        public string nameDetailing
        { get { return NameDetailing; } set { NameDetailing = value; OnPropertyChanged("nameDetailing"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

    }

    // Модель экранной формы объеденяющая ViewDetailing и Feature
    public partial class ListViewDetailingFeature
    {

        [JsonProperty("list")]
        public ViewDetailingFeature[] ViewDetailingFeature { get; set; }

    }


    public class ViewDetailingFeature : INotifyPropertyChanged
    {

        private int Id;
        private int KeyFeature;
        private string NameFeature;
        private int KeyGrDetailing;
        private string NameDetailing;


        public ViewDetailingFeature(int Id = 0, int KeyFeature = 0, string NameFeature = "", int KeyGrDetailing = 0, string NameDetailing = "")
        {
            this.Id = Id;
            this.KeyFeature = KeyFeature;
            this.NameFeature = NameFeature;
            this.KeyGrDetailing = KeyGrDetailing;
            this.NameDetailing = NameDetailing;

        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("keyFeature")]
        public int keyFeature
        {
            get { return KeyFeature; }
            set { KeyFeature = value; OnPropertyChanged("keyFeature"); }
        }

        [JsonProperty("nameFeature")]
        public string nameFeature
        { get { return NameFeature; } set { NameFeature = value; OnPropertyChanged("nameFeature"); } }

        [JsonProperty("keyGrDetailing")]
        public int keyGrDetailing
        {
            get { return KeyGrDetailing; }
            set { KeyGrDetailing = value; OnPropertyChanged("keyGrDetailing"); }
        }

        [JsonProperty("nameDetailing")]
        public string nameDetailing
        { get { return NameDetailing; } set { NameDetailing = value; OnPropertyChanged("nameDetailing"); } }

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
