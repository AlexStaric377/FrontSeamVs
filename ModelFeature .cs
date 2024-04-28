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
    // Колекция характеров жалоб 
    public partial class ListModelFeature
    {

        [JsonProperty("list")]
        public ModelFeature[] ModelFeature { get; set; }

    }

    public class ModelFeature : INotifyPropertyChanged
    {



        private int Id;
        private string KeyComplaint;
        private string KeyFeature;
        private string Name;

        public ModelFeature(int Id = 0, string KeyComplaint = "", string KeyFeature = "", string Name = "")
        {
            this.Id = Id;
            this.KeyComplaint = KeyComplaint;
            this.KeyFeature = KeyFeature;
            this.Name = Name;

        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("keyComplaint")]
        public string keyComplaint
        { get { return KeyComplaint; } set { KeyComplaint = value; OnPropertyChanged("keyComplaint"); } }

        [JsonProperty("KeyFeature")]
        public string keyFeature
        {
            get { return KeyFeature; }
            set { KeyFeature = value; OnPropertyChanged("keyFeature"); }
        }

        [JsonProperty("name")]
        public string name
        { get { return Name; } set { Name = value; OnPropertyChanged("name"); } }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }




    }

    public partial class ListModelFeatureComplaint
    {

        [JsonProperty("list")]
        public ModelFeatureComplaint[] ModelFeatureComplaint { get; set; }

    }

    public class ModelFeatureComplaint : INotifyPropertyChanged
    {

        private int Id;
        private int IdComplaint;
        private string NameFeature;
        private string NameComplaint;

        public ModelFeatureComplaint(int Id = 0, int IdComplaint = 0, string NameFeature = "", string NameComplaint = "")
        {
            this.Id = Id;
            this.IdComplaint = IdComplaint;
            this.NameFeature = NameFeature;
            this.NameComplaint = NameComplaint;
        }


        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("idComplaint")]
        public int idComplaint
        {
            get { return IdComplaint; }
            set { IdComplaint = value; OnPropertyChanged("idComplaint"); }
        }

        [JsonProperty("name")]
        public string nameFeature
        { get { return NameFeature; } set { NameFeature = value; OnPropertyChanged("name"); } }

        [JsonProperty("nameComplaint")]
        public string nameComplaint
        { get { return NameComplaint; } set { NameComplaint = value; OnPropertyChanged("nameComplaint"); } }


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
