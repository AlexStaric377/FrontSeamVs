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

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {
        private static MainWindow VisitngDays = MainWindow.LinkNameWindow("WindowMain");
        private bool loadboolVisitingDays = false, addboolVisitingDays = false, editboolVisitingDays = false;
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
                    string json = pathcontrolerProfilLikar + modelVisitingDays.kodDoctor.ToString()+"/0/0";
                    CallServer.PostServer(pathcontrolerMedZakladProfilLikar, json, "GETID");
                    CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                    ModelDoctor Idinsert = JsonConvert.DeserializeObject<ModelDoctor>(CallServer.ResponseFromServer);
                    if (Idinsert != null)
                    {
                        selectViewModelVisitingDays.nameDoctor = Idinsert.name+ Idinsert.telefon;
                        selectViewModelVisitingDays.edrpou = Idinsert.edrpou;
                        json = pathcontrolerMedZakladProfilLikar + Idinsert.edrpou.ToString() +"/0/0";
                        CallServer.PostServer(pathcontrolerMedZakladProfilLikar, json, "GETID");
                        CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                        MedicalInstitution Idzaklad = JsonConvert.DeserializeObject<MedicalInstitution>(CallServer.ResponseFromServer);

                        if (Idinsert != null)
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
                    VisitngDays.ReseptionLikar.Text = MapOpisViewModel.nameDoctor.Substring(MapOpisViewModel.nameDoctor.IndexOf(":")+1, MapOpisViewModel.nameDoctor.Length- (MapOpisViewModel.nameDoctor.IndexOf(":")+1));
                    VisitngDays.ReseptionPacientLab.Visibility = Visibility.Hidden;
                    CallServer.PostServer(pathcontrolerVisitingDays, pathcontrolerVisitingDays+ MapOpisViewModel._kodDoctor +"/0", "GETID");
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
                BoolFalseVisitingDays();
                VisitngDays.ReseptionPacientTablGrid.SelectedItem = null;
                IndexAddEdit = "";            
            }
 
        }

        // команда печати
        public void MethodPrintVisitingDays()
        {
            if (selectModelVisitingDays != null)
            {
                MessageBox.Show("Прийом :" + selectModelVisitingDays.dateVizita.ToString()+" : "+ selectModelVisitingDays.timeVizita);
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
