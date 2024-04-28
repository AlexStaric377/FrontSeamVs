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
    public partial class ListModelComplaint
    {

        [JsonProperty("list")]
        public ModelComplaint[] ViewComplaint { get; set; }

    }

    public class ModelComplaint : INotifyPropertyChanged
    {

        private bool _treeViewChecked;

        public bool TreeViewChecked
        {
            get => _treeViewChecked;
            set
            {
                _treeViewChecked = value;
                OnPropertyChanged();
                // сюда можно добавить свой код, только осторожно,
                // код не должен выполняться долго
                OnTreeViewChecked();
            }
        }

        private int Id;
        public string ComplaintKey;
        private string Name;

        public ModelComplaint(int Id = 0, string ComplaintKey = "", string Name = "")
        {
            this.Id = Id;
            this.Name = Name;
            this.ComplaintKey = ComplaintKey;
        }


        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("keyComplaint")]
        public string keyComplaint
        { get { return ComplaintKey; } set { ComplaintKey = value; OnPropertyChanged("keyComplaint"); } }

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

        private void OnTreeViewChecked()
        {

            //MessageBox.Show("Новое значение чекбокса: " + TreeViewChecked);

            //MainWindow WindowMain = MainWindow.LinkNameWindow("WindowMain");
            //if (ViewModelNsiComplaint.selectedComplaint != null)
            //{

            //    WindowMain.Featuret4.Text = ViewModelNsiComplaint.selectedComplaint.keyComplaint.ToString() + ": '" + ViewModelNsiComplaint.selectedComplaint.name.ToString();
            //    ViewModelCreatInterview.SelectContentCompl();
            //}
        }


    }














}
