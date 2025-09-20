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

    public partial class ListModelVisitingDays
    {
        [JsonProperty("list")]
        public ModelVisitingDays[] ModelVisitingDays { get; set; }
    }

    public partial class ModelVisitingDays : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        
        private int Id;
        private string KodDoctor;
        private string DaysOfTheWeek;
        private string DateVizita;
        private string TimeVizita;
        private string OnOff; 
        private DateTime DateWork;


        public ModelVisitingDays(int Id = 0, string KodDoctor = "", string DaysOfTheWeek = "", string DateVizita = "",
           string TimeVizita = "", string OnOff = "", DateTime DateWork  = new DateTime())
        {

            this.Id = Id;
            this.KodDoctor = KodDoctor;
            this.DaysOfTheWeek = DaysOfTheWeek;
            this.DateVizita = DateVizita;
            this.TimeVizita = TimeVizita;
            this.OnOff = OnOff;
            this.DateWork = DateWork;
        }

        [JsonProperty("id")]
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }
        [JsonProperty("kodDoctor")]
        public string kodDoctor
        {
            get { return KodDoctor; }
            set { KodDoctor = value; OnPropertyChanged("kodDoctor"); }
        }
        [JsonProperty("daysOfTheWeek")]
        public string daysOfTheWeek
        {
            get { return DaysOfTheWeek; }
            set { DaysOfTheWeek = value; OnPropertyChanged("daysOfTheWeek"); }
        }
        [JsonProperty("dateVizita")]
        public string dateVizita
        {
            get { return DateVizita; }
            set { DateVizita = value; OnPropertyChanged("dateVizita"); }
        }

        [JsonProperty("timeVizita")]
        public string timeVizita
        {
            get { return TimeVizita; }
            set { TimeVizita = value; OnPropertyChanged("timeVizita"); }
        } 

        [JsonProperty("onOff")]
        public string onOff
        {
            get { return OnOff; }
            set { OnOff = value; OnPropertyChanged("onOff"); }
        }

        [JsonProperty("dateWork")]
        public DateTime dateWork
        {
            get { return DateWork; }
            set { DateWork = value; OnPropertyChanged("dateWork"); }
        }
    }

    public class ViewModelVisitingDays : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        private int Id;
        private string KodDoctor;
        private string DaysOfTheWeek;
        private string DateVizita;
        private string TimeVizita;
        private string OnOff;
        private string NameDoctor;
        private string Edrpou;
        private string NameZaklad;


        public ViewModelVisitingDays(int Id = 0, string KodDoctor = "", string DaysOfTheWeek = "", string DateVizita = "",
           string TimeVizita = "", string OnOff = "", string NameDoctor = "", string Edrpou = "", string NameZaklad = "")

        {

            this.Id = Id;
            this.KodDoctor = KodDoctor;
            this.DaysOfTheWeek = DaysOfTheWeek;
            this.DateVizita = DateVizita;
            this.TimeVizita = TimeVizita;
            this.OnOff = OnOff;
            this.NameDoctor = NameDoctor;
            this.Edrpou = Edrpou;
            this.NameZaklad = NameZaklad;
        }
        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        public string kodDoctor
        {
            get { return KodDoctor; }
            set { KodDoctor = value; OnPropertyChanged("kodDoctor"); }
        }
        public string daysOfTheWeek
        {
            get { return DaysOfTheWeek; }
            set { DaysOfTheWeek = value; OnPropertyChanged("daysOfTheWeek"); }
        }
        public string dateVizita
        {
            get { return DateVizita; }
            set { DateVizita = value; OnPropertyChanged("dateVizita"); }
        }

        public string timeVizita
        {
            get { return TimeVizita; }
            set { TimeVizita = value; OnPropertyChanged("timeVizita"); }
        }

        public string onOff
        {
            get { return OnOff; }
            set { OnOff = value; OnPropertyChanged("onOff"); }
        }

        public string nameDoctor
        {
            get { return NameDoctor; }
            set { NameDoctor = value; OnPropertyChanged("nameDoctor"); }
        }

        public string edrpou
        {
            get { return Edrpou; }
            set { Edrpou = value; OnPropertyChanged("edrpou"); }
        }

        public string nameZaklad
        {
            get { return NameZaklad; }
            set { NameZaklad = value; OnPropertyChanged("nameZaklad"); }
        }


        public static List<string> DayWeeks { get; set; } = new List<string> { "Дні неділі","Понеділок", "Вівторок", "Середа", "Четверг", "П'ятниця", "Субота", "Неділя" };
        public static string selectedIndexDayWeek = "0";
        private string _SelectedDayWeek;
        public string SelectedDayWeek
        {
            get => _SelectedDayWeek;
            set
            {
                //// сохраняем старое значение
                //var origValue = _SelectedUnit;

                //меняем значение в обычном порядке
                _SelectedDayWeek = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDayWeek)));
                //OnPropertyChanged(nameof(SelectedUnit));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewDayWeek(_SelectedDayWeek);
            }
        }

        public void SetNewDayWeek(string selected = "")
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("WindowMain");
            WindowMen.ReseptionPacient.Text = (selected == "0" ) ? WindowMen.ReseptionPacient.Text : DayWeeks[Convert.ToInt32(selected)];
            selectedIndexDayWeek = selected;
        }

        public static List<string> TimeVizits { get; set; } = new List<string> {"Час приому", "08.00", "08.30", "09.00", "09.30", "10.00", "10.30", "11.00", "11.30", "12.00", "12.30", "13.00", "13.30", "14.00", "14.30", "15.00", "15.30", "16.00", "16.30", "17.00", "17.30", "18.00", "18.30", "19.00", "19.30" };
        public static string selectedIndexTimeVizita = "0";

        private string _SelectedTimeVizita;
        public string SelectedTimeVizita
        {
            get => _SelectedTimeVizita;
            set
            {
                //// сохраняем старое значение
                //var origValue = _SelectedUnit;

                //меняем значение в обычном порядке
                _SelectedTimeVizita = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTimeVizita)));
                //OnPropertyChanged(nameof(SelectedUnit));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewTimeVizita(_SelectedTimeVizita);
            }
        }

        public void SetNewTimeVizita(string selected = "")
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("WindowMain");
            WindowMen.ReseptionTime.Text = selected == "0" ? WindowMen.ReseptionTime.Text : TimeVizits[Convert.ToInt32(selected)];
            selectedIndexTimeVizita = selected;
        }

        public static List<string> VizitsOnOff { get; set; } = new List<string> { "Прийом","Так", "Ні" };
        public static string selectedIndexVizitsOnOff = "0";

        private string _SelectedVizitsOnOff;
        public string SelectedVizitsOnOff
        {
            get => _SelectedVizitsOnOff;
            set
            {
                //// сохраняем старое значение
                //var origValue = _SelectedUnit;

                //меняем значение в обычном порядке
                _SelectedVizitsOnOff = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedVizitsOnOff)));
                //OnPropertyChanged(nameof(SelectedUnit));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewVizitsOnOff(_SelectedVizitsOnOff);
            }
        }

        public void SetNewVizitsOnOff(string selected = "")
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("WindowMain");
            WindowMen.ReseptionTextBoxOnoff.Text = selected == "0" ? WindowMen.ReseptionTextBoxOnoff.Text : VizitsOnOff[Convert.ToInt32(selected)];
            selectedIndexVizitsOnOff = selected;

        }

        public static List<string> MonthYear { get; set; } = new List<string> { "Місяць року", "Січень", "Лютий", "Березень", "Квітень", "Травень", "Червень", "Липень", "Серпень", "Вереснь", "Жовтень", "Листопад", "Грудень" };
        private string _SelectedMonthYear = "0";
        public static string selectedIndexMonthYear = "";
        public string SelectedMonthYear
        {
            get => _SelectedMonthYear;
            set
            {
                //// сохраняем старое значение
                //var origValue = _SelectedUnit;

                //меняем значение в обычном порядке
                _SelectedMonthYear = value;
                //Оповещаем как обычно изменение, сделанное до if (!_mainWindow.ShowYesNo("Изменить значение?"))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMonthYear)));
                //OnPropertyChanged(nameof(SelectedUnit));
                //а здесь уже преобразуем изменившиеся значение
                //в необходимое uint
                SetNewMonthYear(_SelectedMonthYear);
            }
        }

        public void SetNewMonthYear(string selected = "")
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("WindowMain");
            WindowMen.CabinetReseptionBoxMonth.Text = selected == "0" ? WindowMen.CabinetReseptionBoxMonth.Text : MonthYear[Convert.ToInt32(selected)];
            selectedIndexMonthYear = selected;
            MapOpisViewModel.loadthisMonth = true;

        }
    }


}
