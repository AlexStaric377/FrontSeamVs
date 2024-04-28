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
    // Список групп детализации Групи деталізацій

    public partial class ListModelListGrDetailing
    {

        [JsonProperty("list")]
        public ModelListGrDetailing[] ViewListGrDetailing { get; set; }

    }
    public class ModelListGrDetailing : INotifyPropertyChanged
    {

        private int Id;
        private string KeyGrDetailing;
        private string NameGrup;


        public ModelListGrDetailing(int Id = 0, string KeyGrDetailing ="", string NameGrup = "")
        {
            this.Id = Id;
            this.KeyGrDetailing = KeyGrDetailing;
            this.NameGrup = NameGrup;

        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("keyGrDetailing")]
        public string keyGrDetailing
        {
            get { return KeyGrDetailing; }
            set { KeyGrDetailing = value; OnPropertyChanged("keyGrDetailing"); }
        }

        [JsonProperty("nameGrup")]
        public string nameGrup
        {
            get { return NameGrup; }
            set { NameGrup = value; OnPropertyChanged("nameGrup"); }
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
