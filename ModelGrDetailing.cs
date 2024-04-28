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
    // Список содержимого групп детализаций т.е. какя детализация входит в соства каждой групы

    public partial class ListModelGrDetailing
    {

        [JsonProperty("list")]
        public ModelGrDetailing[] ViewGrDetailing { get; set; }

    }

   

    public class ModelGrDetailing : INotifyPropertyChanged
    {

        private int Id;
        private string KeyGrDetailing;
        private string KodGroupQualification;
        private string NameGrDetailing;
        private string KodDetailing;

        public ModelGrDetailing(int Id = 0, string KeyGrDetailing = "", string KodDetailing="", string KodGroupQualification = "", string NameGrDetailing = "")
        {
            this.Id = Id;
            this.KeyGrDetailing = KeyGrDetailing;
            this.KodGroupQualification = KodGroupQualification;
            this.NameGrDetailing = NameGrDetailing;
            this.KodDetailing = KodDetailing;

        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
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
        [JsonProperty("kodGroupQualification")]
        public string kodGroupQualification
        {
            get { return KodGroupQualification; }
            set { KodGroupQualification = value; OnPropertyChanged("kodGroupQualification"); }
        }
        [JsonProperty("nameGrDetailing")]
        public string nameGrDetailing
        {
            get { return NameGrDetailing; }
            set { NameGrDetailing = value; OnPropertyChanged("nameGrDetailing"); }
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
    // Содержание груповых детализаций грид экранной формы вывода данных объеденены  модели
    // ViewGrDetailing и ViewGroupQualification
    public partial class ListDetailingQualification
    {
        [JsonProperty("list")]
        public ViewDetailingQualification[] ViewDetailingQualification { get; set; }
    }
    public class ViewDetailingQualification : INotifyPropertyChanged
    {

        private int Id;
        private string KeyGrDetailing;
        private string IdQualification;
        private string NameGrup;
        private string NameGroupQualification;
        private string NnameGrDetailing;

        public ViewDetailingQualification(int Id = 0, string KeyGrDetailing = "", string IdQualification = "", string NameGrup = "", string NameGroupQualification = "", string NnameGrDetailing = "")
        {
            this.Id = Id;
            this.KeyGrDetailing = KeyGrDetailing;
            this.IdQualification = IdQualification;
            this.NameGrup = NameGrup;
            this.NameGroupQualification = NameGroupQualification;
            this.NnameGrDetailing = NnameGrDetailing;

        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        public string keyGrDetailing
        {
            get { return KeyGrDetailing; }
            set { KeyGrDetailing = value; OnPropertyChanged("keyGrDetailing"); }
        }
        [JsonProperty("idQualification")]
        public string idQualification
        {
            get { return IdQualification; }
            set { IdQualification = value; OnPropertyChanged("idQualification"); }
        }
        [JsonProperty("nameGrup")]
        public string nameGrup
        {
            get { return NameGrup; }
            set { NameGrup = value; OnPropertyChanged("nameGrup"); }
        }
        [JsonProperty("nameGroupQualification")]
        public string nameGroupQualification
        {
            get { return NameGroupQualification; }
            set { NameGroupQualification = value; OnPropertyChanged("nameGroupQualification"); }
        }
        [JsonProperty("nameGrDetailing")]
        public string nameGrDetailing
        {
            get { return NnameGrDetailing; }
            set { NnameGrDetailing = value; OnPropertyChanged("nameGrDetailing"); }
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
