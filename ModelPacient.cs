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
    
    public partial class ListModelPacient
    {

        [JsonProperty("list")]
        public ModelPacient[] ViewPacient { get; set; }

    }
    public class ModelPacient : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        // команда закрытия окна
        RelayCommand? checkKeyTextTel;
        public RelayCommand CheckKeyTextTel
        {
            get
            {
                return checkKeyTextTel ??
                  (checkKeyTextTel = new RelayCommand(obj =>
                  {
                      if (MapOpisViewModel.WindowProfilPacient.ControlMain.SelectedIndex != 3)
                      {
                          IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowProfilPacient.PacientProfilt8, 12);
                      }
                      IdCardKeyUp.CheckKeyUpIdCard(ViewModelWinProfilPacient.WindowResult.PacientProfilt8, 12);
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? checkKeyTextPind;
        public RelayCommand CheckKeyTextPind
        {
            get
            {
                return checkKeyTextPind ??
                  (checkKeyTextPind = new RelayCommand(obj =>
                  {
                      if (MapOpisViewModel.WindowProfilPacient.ControlMain.SelectedIndex != 3)
                      {
                          IdCardKeyUp.CheckKeyUpIdCard(MapOpisViewModel.WindowProfilPacient.PacientProfilt13, 5);
                      }
                      IdCardKeyUp.CheckKeyUpIdCard(ViewModelWinProfilPacient.WindowResult.PacientProfilt13, 5);

                  }));
            }
        }
        public static List<string> Units { get; set; } = new List<string> { "чол.", "жін." };

        private string _SelectedUnit;
        public string SelectedUnit
        {
            get => _SelectedUnit;
            set
            {
                //// сохраняем старое значение
                //var origValue = _SelectedUnit;

                //меняем значение в обычном порядке
                _SelectedUnit = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedUnit)));
                //OnPropertyChanged(nameof(SelectedUnit));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewUnit(_SelectedUnit);
            }
        }

        public void SetNewUnit(string selected = "")
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("WindowMain");
            WindowMen.PacientProfilt7.Text = selected == "0"? "чол.": "жін.";
        }

        private string _SelectCombProfil;
        public string SelectCombProfil
        {
            get => _SelectCombProfil;
            set
            {
                //меняем значение в обычном порядке
                _SelectCombProfil = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectCombProfil)));
                //OnPropertyChanged(nameof(SelectedComb));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewComboProfil(_SelectCombProfil);
            }
        }

        public void SetNewComboProfil(string selected = "")
        {
            WinProfilPacient WindowResult = MainWindow.LinkMainWindow("WinProfilPacient");
            WindowResult.PacientProfilt7.Text = selected == "0" ? "чол." : "жін.";
        }




        private int Id;
        private string KodPacient;
        private string IdCabinet;
        private int Age;
        private decimal Weight;
        private int Growth;
        private string Gender;
        private string Tel;
        private string Email;
        private string Name;
        private string Surname;
        private string Login;
        private string Password;
        private string Pind;
        private string Profession;


        public ModelPacient(int Id = 0, string KodPacient = "", string IdCabinet = "", int Age = 0, decimal Weight = 0, int Growth = 0, string Gender = "",
             string Tel = "", string Email = "", string Name = "", string Surname = "", string Login = "", string Password = "", string Pind = "", string Profession="")
        {
            this.Id = Id;
            this.KodPacient = KodPacient;
            this.IdCabinet = IdCabinet;
            this.Age = Age;
            this.Weight = Weight;
            this.Growth = Growth;
            this.Gender = Gender;
            this.Tel = Tel;
            this.Email = Email;
            this.Name = Name;
            this.Surname = Surname;
            this.Login = Login;
            this.Password = Password;
            this.Pind = Pind;
            this.Profession = Profession;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        [JsonProperty("kodPacient")]
        public string kodPacient
        {
            get { return KodPacient; }
            set { KodPacient = value; OnPropertyChanged("kodPacient"); }
        }
        [JsonProperty("idCabinet")]
        public string idCabinet
        {
            get { return IdCabinet; }
            set { IdCabinet = value; OnPropertyChanged("idCabinet"); }
        }

        [JsonProperty("age")]
        public int age
        {
            get { return Age; }
            set { Age = value; OnPropertyChanged("age"); }
        }

        [JsonProperty("weight")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal weight
        {
            get { return Weight; }
            set { Weight = value; OnPropertyChanged("weight"); }
        }

        [JsonProperty("growth")]
        public int growth
        { 
            get { return Growth; }
            set { Growth = value; OnPropertyChanged("growth"); }
        }

        [JsonProperty("gender")]
        public string gender
        {
            get { return Gender; }
            set { Gender = value; OnPropertyChanged("gender"); }
        }

 
        [JsonProperty("tel")]
        public string tel
        {
            get { return Tel; }
            set { Tel = value; OnPropertyChanged("tel"); }
        }
        [JsonProperty("email")]
        public string email
        {
            get { return Email; }
            set { Email = value; OnPropertyChanged("email"); }
        }
        [JsonProperty("name")]
        public string name
        {
            get { return Name; }
            set { Name = value; OnPropertyChanged("name"); }
        }
        [JsonProperty("surname")]
        public string surname
        {
            get { return Surname; }
            set { Surname = value; OnPropertyChanged("surname"); }
        }
        [JsonProperty("login")]
        public string login
        {
            get { return Login; }
            set { Login = value; OnPropertyChanged("login"); }
        }
        [JsonProperty("password")]
        public string password
        {
            get { return Password; }
            set { Password = value; OnPropertyChanged("password"); }
        }
        [JsonProperty("pind")]
        public string pind
        {
            get { return Pind; }
            set { Pind = value; OnPropertyChanged("pind"); }
        }
        [JsonProperty("profession")]
        public string profession
        {
            get { return Profession; }
            set { Profession = value; OnPropertyChanged("profession"); }
        }

    }
   
}
