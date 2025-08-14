using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {
        private static MainWindow VisitngDays = MainWindow.LinkNameWindow("WindowMain");
        public static bool loadboolVisitingDays = false, addboolVisitingDays = false, editboolVisitingDays = false, loadthisMonth = false;
        public static string pathcontrolerVisitingDays = "/api/ApiControllerVisitingDays/";
        public static ModelVisitingDays selectModelVisitingDays;
        public static ViewModelVisitingDays selectViewModelVisitingDays;

        public ViewModelVisitingDays SelectedViewModelVisitingDays
        {
            get { return selectViewModelVisitingDays; }
            set { selectViewModelVisitingDays = value; OnPropertyChanged("SelectedViewModelVisitingDays"); }
        }
        public static ObservableCollection<ModelVisitingDays> ViewVisitingDays { get; set; }
        public static ObservableCollection<ViewModelVisitingDays> ViewModeVisitingDays { get; set; }

        public static void ObservableModelVisitingDays(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelVisitingDays>(CmdStroka);
            List<ModelVisitingDays> res = result.ModelVisitingDays.ToList();
            ViewVisitingDays = new ObservableCollection<ModelVisitingDays>((IEnumerable<ModelVisitingDays>)res);
            IndexAddEdit = "";
            MetodLoadGridViewModelVisitingDays();
        }

        public static void MetodLoadGridViewModelVisitingDays()
        {
            ViewModeVisitingDays = new ObservableCollection<ViewModelVisitingDays>();
            foreach (ModelVisitingDays modelVisitingDays in ViewVisitingDays)
            {
                selectModelVisitingDays = modelVisitingDays;
                selectViewModelVisitingDays = new ViewModelVisitingDays();
                selectViewModelVisitingDays.kodDoctor = modelVisitingDays.kodDoctor;
                selectViewModelVisitingDays.id = modelVisitingDays.id;
                selectViewModelVisitingDays.daysOfTheWeek = modelVisitingDays.daysOfTheWeek;
                selectViewModelVisitingDays.dateVizita = modelVisitingDays.dateVizita;
                selectViewModelVisitingDays.timeVizita = modelVisitingDays.timeVizita;
                selectViewModelVisitingDays.onOff = modelVisitingDays.onOff;

                if (modelVisitingDays.kodDoctor != "")
                {
                    string json = pathcontrolerProfilLikar + modelVisitingDays.kodDoctor.ToString() + "/0/0";
                    CallServer.PostServer(pathcontrolerMedZakladProfilLikar, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    ModelDoctor Idinsert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                    if (Idinsert != null)
                    {
                        selectViewModelVisitingDays.nameDoctor = Idinsert.name + Idinsert.telefon;
                        selectViewModelVisitingDays.edrpou = Idinsert.edrpou;
                        json = pathcontrolerMedZakladProfilLikar + Idinsert.edrpou.ToString() + "/0/0/0";
                        CallServer.PostServer(pathcontrolerMedZakladProfilLikar, json, "GETID");
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        MedicalInstitution Idzaklad = JsonConvert.DeserializeObject<MedicalInstitution>(CallServer.ResponseFromServer);

                        if (Idzaklad != null)
                        {
                            selectViewModelVisitingDays.nameZaklad = Idzaklad.name;
                        }
                    }
                }
                ViewModeVisitingDays.Add(selectViewModelVisitingDays);
            }
            VisitngDays.ReseptionPacientTablGrid.ItemsSource = ViewModeVisitingDays;
        }

        #region Команды вставки, удаления и редектирования справочника "расписаний приёмов пациентов"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника 
        /// </summary>
        /// 

        public void MethodLoadVisitingDays()
        {
            LoadVisitingDays();
        }

        private void LoadVisitingDays()
        {


            VisitngDays.NameMedZaklad.Text = VisitngDays.Likart9.Text;
            VisitngDays.ReseptionLikar.Text = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - (MapOpisViewModel.nameDoctor.IndexOf(":") + 1));
            VisitngDays.CabinetReseptionPacientLab.Visibility = Visibility.Hidden;
            CallServer.PostServer(pathcontrolerVisitingDays, pathcontrolerVisitingDays + MapOpisViewModel._kodDoctor + "/0", "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableModelVisitingDays(CmdStroka);
            loadboolVisitingDays = true;

            VisitngDays.DayoftheWeek.ItemsSource = ViewModelVisitingDays.DayWeeks;
            VisitngDays.DayoftheWeek.SelectedIndex = Convert.ToInt32(ViewModelVisitingDays.selectedIndexDayWeek);
            VisitngDays.TimeofDay.ItemsSource = ViewModelVisitingDays.TimeVizits;
            VisitngDays.TimeofDay.SelectedIndex = Convert.ToInt32(ViewModelVisitingDays.selectedIndexTimeVizita);
            VisitngDays.ComboBoxOnoff.ItemsSource = ViewModelVisitingDays.VizitsOnOff;
            VisitngDays.ComboBoxOnoff.SelectedIndex = Convert.ToInt32(ViewModelVisitingDays.selectedIndexVizitsOnOff);

        }


        // команда добавления нового объекта

        public void MethodAddVisitingDays()
        {
            if (loadboolAccountUser == false)
            {
                if (RegSetAccountUser() == false) return;
            }
            AddComandVisitingDays();
        }


        private void AddComandVisitingDays()
        {
            if (loadboolVisitingDays == false) MethodLoadVisitingDays();
            MethodaddcomVisitingDays();
        }

        private void MethodaddcomVisitingDays()
        {
            IndexAddEdit = IndexAddEdit == "addCommand" ? "" : "addCommand";
            selectModelVisitingDays = new ModelVisitingDays();
            selectViewModelVisitingDays = new ViewModelVisitingDays();
            selectViewModelVisitingDays.nameZaklad = VisitngDays.Likart9.Text;
            selectViewModelVisitingDays.nameDoctor = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":") + 1, MapOpisViewModel.nameDoctor.Length - (MapOpisViewModel.nameDoctor.IndexOf(":") + 1));
            SelectedViewModelVisitingDays = selectViewModelVisitingDays;
            if (addboolVisitingDays == false) BoolTrueVisitingDays();
            else BoolFalseVisitingDays();
            VisitngDays.ReseptionPacientTablGrid.SelectedItem = null;
            VisitngDays.DayoftheWeek.SelectedIndex = 0;
            VisitngDays.TimeofDay.SelectedIndex = 0;
            VisitngDays.ComboBoxOnoff.SelectedIndex = 0;

        }



        private void BoolTrueVisitingDays()
        {
            addboolVisitingDays = true;
            editboolVisitingDays = true;
            VisitngDays.ReseptionTime.IsEnabled = true;
            VisitngDays.ReseptionTime.Background = Brushes.AntiqueWhite;
            VisitngDays.DayoftheWeek.IsEnabled = true;
            VisitngDays.DatePicker.IsEnabled = true;
            VisitngDays.TimeofDay.IsEnabled = true;
            VisitngDays.ComboBoxOnoff.IsEnabled = true;
            VisitngDays.CabinetDayoftheMonth.IsEnabled = true;
            VisitngDays.CabinetReseptionTimeOn.IsEnabled = true;
            VisitngDays.CabinetReseptionDayBoxLast.IsEnabled = true;
            VisitngDays.CabinetReseptionDayBoxLast.Background = Brushes.AntiqueWhite;
            VisitngDays.CabinetReseptionDayOn.IsEnabled = true;
            VisitngDays.CabinetReseptionDayOn.Background = Brushes.AntiqueWhite;
            VisitngDays.CabinetReseptionTimeOn.Background = Brushes.AntiqueWhite;
            VisitngDays.CabinetReseptionTimeBoxLast.IsEnabled = true;
            VisitngDays.CabinetReseptionTimeBoxLast.Background = Brushes.AntiqueWhite;

        }


        private void BoolFalseVisitingDays()
        {

            addboolVisitingDays = false;
            editboolVisitingDays = false;
            VisitngDays.ReseptionTime.IsEnabled = false;
            VisitngDays.ReseptionTime.Background = Brushes.White;
            VisitngDays.DayoftheWeek.IsEnabled = false;
            VisitngDays.DatePicker.IsEnabled = false;
            VisitngDays.TimeofDay.IsEnabled = false;
            VisitngDays.ComboBoxOnoff.IsEnabled = false;
            VisitngDays.CabinetDayoftheMonth.IsEnabled = false;
            VisitngDays.CabinetReseptionTimeOn.IsEnabled = false;
            VisitngDays.CabinetReseptionDayBoxLast.IsEnabled = false;
            VisitngDays.CabinetReseptionDayBoxLast.Background = Brushes.White;
            VisitngDays.CabinetReseptionDayOn.IsEnabled = false;
            VisitngDays.CabinetReseptionDayOn.Background = Brushes.White;
            VisitngDays.CabinetReseptionTimeOn.Background = Brushes.White;
            VisitngDays.CabinetReseptionTimeBoxLast.IsEnabled = false;
            VisitngDays.CabinetReseptionTimeBoxLast.Background = Brushes.White;
            VisitngDays.DayoftheWeek.SelectedIndex = 0;
            VisitngDays.TimeofDay.SelectedIndex = 0;
            VisitngDays.ComboBoxOnoff.SelectedIndex = 0;
            VisitngDays.CabinetDayoftheMonth.SelectedIndex = 0;
            VisitngDays.CabinetReseptionBoxMonth.Text = "";
        }

        // команда  редактировать

        public void MethodEditVisitingDays()
        {
            if (selectViewModelVisitingDays != null)
            {
                IndexAddEdit = "editCommand";
                if (editboolVisitingDays == false) { BoolTrueVisitingDays(); }
                else
                {
                    BoolFalseVisitingDays();
                    VisitngDays.ReseptionPacientTablGrid.SelectedItem = null;
                    IndexAddEdit = "";
                }
            }
        }

        // команда удаления

        public void MethodRemoveVisitingDays()
        {
            if (VisitngDays.ReseptionPacientTablGrid.SelectedIndex >= 0)
            {
                int RemoveIndex = VisitngDays.ReseptionPacientTablGrid.SelectedIndex;
                // Видалення данных о гостях, пациентах, докторах, учетных записях
                if (MapOpisViewModel.DeleteOnOff == true)
                {

                    string json = pathcontrolerVisitingDays + selectViewModelVisitingDays.id.ToString();

                    CallServer.PostServer(pathcontrolerVisitingDays, json, "DELETE");
                    ViewVisitingDays.Remove(ViewVisitingDays[RemoveIndex]);
                    ViewModeVisitingDays.Remove(ViewModeVisitingDays[RemoveIndex]);
                    VisitngDays.ReseptionPacientTablGrid.SelectedItem = null;
                    selectViewModelVisitingDays = new ViewModelVisitingDays();
                    SelectedViewModelVisitingDays = selectViewModelVisitingDays;
                }
            }
            IndexAddEdit = "";
        }

        // команда сохранить редактирование
        public void MethodSaveVisitingDays()
        {
            if (selectModelVisitingDays != null)
            {
                if (loadthisMonth == true)
                {
                    AddAppointments();
                    MetodLoadGridViewModelVisitingDays();


                }
                else
                {
                    if (WindowMen.ReseptionPacient.Text == "" || WindowMen.ReseptionPacientVisit.Text == "" || WindowMen.ReseptionTime.Text == "" || WindowMen.ReseptionTextBoxOnoff.Text == "")
                    {
                        MainWindow.MessageError = " Не вказані всі показники що визначають дату та час прийому. ";
                        MapOpisViewModel.SelectedWirning(0);
                        return;
                    }
                    string json = "";
                    VisitngDays.ReseptionPacientVisit.Text = VisitngDays.ReseptionPacientVisit.Text.Substring(0, 10);
                    selectModelVisitingDays = new ModelVisitingDays();
                    selectModelVisitingDays.kodDoctor = MapOpisViewModel.nameDoctor.Substring(0, MapOpisViewModel.nameDoctor.IndexOf(":"));
                    selectModelVisitingDays.daysOfTheWeek = VisitngDays.ReseptionPacient.Text;
                    selectModelVisitingDays.dateVizita = VisitngDays.ReseptionPacientVisit.Text;
                    selectModelVisitingDays.timeVizita = VisitngDays.ReseptionTime.Text;
                    selectModelVisitingDays.onOff = VisitngDays.ReseptionTextBoxOnoff.Text;

                    if (IndexAddEdit == "addCommand")
                    {


                        json = JsonConvert.SerializeObject(selectModelVisitingDays);
                        CallServer.PostServer(pathcontrolerVisitingDays, json, "POST");
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelVisitingDays Idinsert = JsonConvert.DeserializeObject<ModelVisitingDays>(CallServer.ResponseFromServer);
                        int Countins = ViewVisitingDays != null ? ViewVisitingDays.Count : 0;
                        ViewVisitingDays.Insert(Countins, Idinsert);
                        selectViewModelVisitingDays = new ViewModelVisitingDays();
                        selectViewModelVisitingDays.kodDoctor = Idinsert.kodDoctor;
                        selectViewModelVisitingDays.id = Idinsert.id;
                        selectViewModelVisitingDays.daysOfTheWeek = Idinsert.daysOfTheWeek;
                        selectViewModelVisitingDays.dateVizita = Idinsert.dateVizita;
                        selectViewModelVisitingDays.timeVizita = Idinsert.timeVizita;
                        selectViewModelVisitingDays.onOff = Idinsert.onOff;
                        ViewModeVisitingDays.Add(selectViewModelVisitingDays);
                        VisitngDays.ReseptionPacientTablGrid.ItemsSource = ViewVisitingDays;

                    }
                    else
                    {
                        ViewVisitingDays[VisitngDays.ReseptionPacientTablGrid.SelectedIndex] = selectModelVisitingDays;
                        selectModelVisitingDays.id = selectViewModelVisitingDays.id;
                        json = JsonConvert.SerializeObject(selectModelVisitingDays);
                        CallServer.PostServer(pathcontrolerVisitingDays, json, "PUT");
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        json = CallServer.ResponseFromServer;
                    }
                    UnloadCmdStroka("VisitingDays/", json);
                }
                BoolFalseVisitingDays();
                VisitngDays.ReseptionPacientTablGrid.SelectedItem = null;
                IndexAddEdit = "";
            }

        }

        public void AddAppointments()
        {
            MainWindow WindowMen = MainWindow.LinkNameWindow("WindowMain");
            ObservableCollection<ModelVisitingDays> ViewLikarAppointments = new ObservableCollection<ModelVisitingDays>();
            WindowMen.CabinetReseptionBoxMonth.Text = ViewModelVisitingDays.MonthYear[Convert.ToInt32(ViewModelVisitingDays.selectedIndexMonthYear)];
            MapOpisViewModel.selectModelVisitingDays.kodDoctor = MapOpisViewModel.nameDoctor.Substring(0, MapOpisViewModel.nameDoctor.IndexOf(":"));
            int nawday = System.DateTime.Now.Day;
            int nawmonth = System.DateTime.Now.Month;
            int beginmonth = Convert.ToInt32(ViewModelVisitingDays.selectedIndexMonthYear);
            int beginind = Convert.ToInt32(WindowMen.CabinetReseptionDayOn.Text);
            if (beginmonth == nawmonth && beginind < nawday )
            {
                MainWindow.MessageError = " Перший день прийому меньше поточного дня календарного місяця обраного вами. ";
                MapOpisViewModel.SelectedWirning(0);
                return;
            }
            int Daymonth = System.DateTime.DaysInMonth(DateTime.Now.Year, Convert.ToInt32(ViewModelVisitingDays.selectedIndexMonthYear));
            int lastDay = Convert.ToInt32(WindowMen.CabinetReseptionDayBoxLast.Text);
            if (lastDay > Daymonth)
            {
                MainWindow.MessageError = " Крайній день прийому більше останнього дня календарного місяця обраного вами. ";
                MapOpisViewModel.SelectedWirning(0);
                return;
            }
            if (lastDay < beginind)
            {
                MainWindow.MessageError = " Перший день прийому більше останнього дня прийому. ";
                MapOpisViewModel.SelectedWirning(0);
                return;
            }
            MapOpisViewModel.RunGifWait();

            string ThisDay = "", ThisMonth = "", json = "";
            int itime = 1;
            

            decimal TimeOn = Convert.ToDecimal(WindowMen.CabinetReseptionTimeOn.Text.Replace(".", ","));
            decimal TimeLast = Convert.ToDecimal(WindowMen.CabinetReseptionTimeBoxLast.Text.Replace(".", ","));
            if (TimeOn > TimeLast)
            {
                MainWindow.MessageError = " Час закінчення прийому меньше часу початку прийому";
                MapOpisViewModel.SelectedWirning(0);
                return;
            }

            for (decimal ind = TimeOn; ind <= TimeLast; ind++)
            {
                string stringTime = ind <= 9 ? "0" + Convert.ToString(ind) : Convert.ToString(ind);
                ViewModelVisitingDays.TimeVizits[itime] = stringTime;
                if (ind < TimeLast)
                {
                    itime++;
                    stringTime = ind <= 9 ? "0" + Convert.ToString(ind + 0.3m) : Convert.ToString(ind + 0.3m);
                    ViewModelVisitingDays.TimeVizits[itime] = stringTime;
                    itime++;
                }

            }
            for (int ind = itime; ind < 19; ind++)
            { ViewModelVisitingDays.TimeVizits[ind] = ""; }

            //}
            // выбранный месяц и год 
            string ThisYear = DateTime.Now.ToShortDateString().Substring(DateTime.Now.ToShortDateString().LastIndexOf(".") + 1, DateTime.Now.ToShortDateString().Length - (DateTime.Now.ToShortDateString().LastIndexOf(".") + 1));
            ThisMonth = ViewModelVisitingDays.selectedIndexMonthYear.Length > 1 ? ViewModelVisitingDays.selectedIndexMonthYear : "0" + ViewModelVisitingDays.selectedIndexMonthYear;
            json = MapOpisViewModel.pathcontrolerVisitingDays + MapOpisViewModel.selectModelVisitingDays.kodDoctor + "/" + ThisMonth + "." + ThisYear;
            CallServer.PostServer(MapOpisViewModel.pathcontrolerVisitingDays, json, "GETID");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]") == false)
            {
                MainWindow.MessageError = "Увага!" + Environment.NewLine +
                "Розклад по місяцю " + WindowMen.CabinetReseptionBoxMonth.Text + " вже сформовано. Ви бажаєте стерти існуючий розклад?";
                MapOpisViewModel.SelectedDelete();

                if (MapOpisViewModel.DeleteOnOff == false)
                {
                    WindowMen.CabinetDayoftheMonth.SelectedIndex = 0;
                    return;
                }
                MapOpisViewModel.LoadInfoPacient("розкладу прийому пацієнтів на " + WindowMen.CabinetReseptionBoxMonth.Text);
                json = MapOpisViewModel.pathcontrolerVisitingDays + "0/" + MapOpisViewModel.selectModelVisitingDays.kodDoctor + "/" + ThisMonth + "." + ThisYear;
                CallServer.PostServer(MapOpisViewModel.pathcontrolerVisitingDays, json, "DELETE");
                MapOpisViewModel.ViewVisitingDays = new ObservableCollection<ModelVisitingDays>();
                MapOpisViewModel.ViewModeVisitingDays = new ObservableCollection<ViewModelVisitingDays>();
                WindowMen.ReseptionPacientTablGrid.ItemsSource = MapOpisViewModel.ViewModeVisitingDays;

            }
            if (Convert.ToInt32(ViewModelVisitingDays.selectedIndexMonthYear) == 0) return;
            // количество дней в месяце


            for (int i = beginind; i <= lastDay; i++)
            {
                ThisDay = i > 9 ? Convert.ToString(i) : "0" + Convert.ToString(i);
                string dateVisit = ThisDay + "." + ThisMonth + "." + ThisYear;
                MapOpisViewModel.selectModelVisitingDays.dateVizita = dateVisit;
                DateTime convertedDate = Convert.ToDateTime(dateVisit);
                int theweek = (int)convertedDate.DayOfWeek;
                MapOpisViewModel.selectModelVisitingDays.daysOfTheWeek = ViewModelVisitingDays.DayWeeks[theweek];
                MapOpisViewModel.selectModelVisitingDays.onOff = "Так";
                if (MapOpisViewModel.selectModelVisitingDays.daysOfTheWeek != "Субота" && MapOpisViewModel.selectModelVisitingDays.daysOfTheWeek != "Неділя" && theweek != 0)
                {
                    for (int indtime = 1; indtime < itime; indtime++)  //18
                    {
                        ModelVisitingDays VisitingDays = MapOpisViewModel.selectModelVisitingDays;
                        VisitingDays.timeVizita = ViewModelVisitingDays.TimeVizits[indtime];
                        json = JsonConvert.SerializeObject(VisitingDays);
                        CallServer.PostServer(MapOpisViewModel.pathcontrolerVisitingDays, json, "POST");
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        ModelVisitingDays Idinsert = JsonConvert.DeserializeObject<ModelVisitingDays>(CallServer.ResponseFromServer);
                        MapOpisViewModel.ViewVisitingDays.Add(Idinsert);
                    }
                }
            }
            MessageWarning Info = MainWindow.LinkMainWindow("MessageWarning");
            if (Info != null) Info.Close();
            endUnload = 1;

        }


        // команда печати
        public void MethodPrintVisitingDays()
        {
            if (selectModelVisitingDays != null)
            {
                MessageBox.Show("Прийом :" + selectModelVisitingDays.dateVizita.ToString() + " : " + selectModelVisitingDays.timeVizita);
            }
        }

        private RelayCommand? onVisibleObjVisitingDays;
        public RelayCommand OnVisibleObjVisitingDays
        {
            get
            {
                return onVisibleObjVisitingDays ??
                  (onVisibleObjVisitingDays = new RelayCommand(obj =>
                  {
                      if (ViewVisitingDays != null)
                      {
                          if (VisitngDays.ReseptionPacientTablGrid.SelectedIndex == -1) return;
                          selectViewModelVisitingDays = ViewModeVisitingDays[VisitngDays.ReseptionPacientTablGrid.SelectedIndex];
                          SelectedViewModelVisitingDays = selectViewModelVisitingDays;
                          VisitngDays.DayoftheWeek.SelectedIndex = 0;
                          VisitngDays.TimeofDay.SelectedIndex = 0;
                          VisitngDays.ComboBoxOnoff.SelectedIndex = 0;
                      }
                  }));
            }
        }
        #endregion
    }

}
