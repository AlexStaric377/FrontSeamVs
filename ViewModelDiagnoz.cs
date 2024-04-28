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
using System.Diagnostics;

namespace FrontSeam
{
    public partial class MapOpisViewModel : INotifyPropertyChanged
    {
        // GroupQualificationViewModel модель ViewQualification
        //  клавиша в окне: "Групы квалифікації"

        #region Обработка событий и команд вставки, удаления и редектирования справочника "Групы квалифікації"
        /// <summary>
        /// Стркутура: Команды, объявления ObservableCollection, загрузка списка всех груп квалифікації из БД
        /// через механизм REST.API
        /// </summary>
        public static MainWindow WindowMen = MainWindow.LinkNameWindow("WindowMain");
        bool activViewDiagnoz = false, loadbooltableDiagnoz = false;
        public static string controlerViewDiagnoz =  "/api/DiagnozController/";
        public static string controlerNsiIcd = "/api/IcdController/";
        public static ModelDiagnoz selectedDiagnoz;

        public static ObservableCollection<ModelDiagnoz> ViewDiagnozs { get; set; }

        public ModelDiagnoz SelectedViewDiagnoz
        { get { return selectedDiagnoz; } set { selectedDiagnoz = value; OnPropertyChanged("SelectedViewDiagnoz"); } }

        public static void ObservableViewDiagnoz(string CmdStroka)
        {
            var result = JsonConvert.DeserializeObject<ListModelDiagnoz>(CmdStroka);
            List<ModelDiagnoz> res = result.ModelDiagnoz.ToList();
            ViewDiagnozs = new ObservableCollection<ModelDiagnoz>((IEnumerable<ModelDiagnoz>)res);
            WindowMen.LibDiagnozTablGrid.ItemsSource = ViewDiagnozs;
        }

        #region Команды вставки, удаления и редектирования справочника "ГРупи кваліфікації"
        /// <summary>
        /// Команды вставки, удаления и редектирования справочника "Жалобы"
        /// </summary>

   
        // загрузка справочника диагнозов
        public void MethodViewLIbDiagnoz()
        {
            MethodloadtablDiagnoz();
        }

        public void MethodloadtablDiagnoz()
        {

            WindowMen.LibLoadDia.Visibility = Visibility.Hidden;
            CallServer.PostServer(controlerViewDiagnoz, controlerViewDiagnoz, "GET");
            string CmdStroka = CallServer.ServerReturn();
            if (CmdStroka.Contains("[]")) CallServer.BoolFalseTabl();
            else ObservableViewDiagnoz(CmdStroka);
        }
  
        // команда загрузки  строки исх МКХ11 по указанному коду для вівода наименования болезни
        private RelayCommand? findNameMkx;
        public RelayCommand FindNameMkx
        {
            get
            {
                return findNameMkx ??
                  (findNameMkx = new RelayCommand(obj =>
                  { ComandFindNameMkx(); }));
            }
        }

        private void ComandFindNameMkx()
        {
            if (selectedDiagnoz != null)
            {
                if (ViewDiagnozs != null)
                {
                    if (WindowMen.LibDiagnozTablGrid.SelectedIndex >= 0)
                    {
                        WindowMen.LibDiagnozt3.Text = "";
                        selectedDiagnoz = ViewDiagnozs[WindowMen.LibDiagnozTablGrid.SelectedIndex];
                        
                        if (selectedDiagnoz.keyIcd != "")
                        {

                            string json = controlerNsiIcd + selectedDiagnoz.keyIcd.ToString() + "/0";
                            CallServer.PostServer(controlerViewDiagnoz, json, "GETID");
                            CallServer.ResponseFromServer = CallServer.ResponseFromServer.Replace("[", "").Replace("]", "");
                            ModelIcd Idinsert = JsonConvert.DeserializeObject<ModelIcd>(CallServer.ResponseFromServer);
                            if (Idinsert != null) WindowMen.LibDiagnozt3.Text = Idinsert.name;
 
                        }
                   
                    }
                }
                

            }
        }

        private RelayCommand? loadUriInterview;
        public RelayCommand LoadUriInterview
        {
            get
            {
                return loadUriInterview ??
                  (loadUriInterview = new RelayCommand(obj =>
                  {
                      if (selectedDiagnoz != null)
                      {
                          if (selectedDiagnoz.uriDiagnoza != null)
                          {
                              MainWindow.MessageError = "Увага!" + Environment.NewLine + "Чекайте завантаження сторінки.";
                              SelectedWirning(3);

                              string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                              string System_path = System.IO.Path.GetPathRoot(System.Environment.SystemDirectory);
                              string Puthgoogle = workingDirectory + @"\Google\Chrome\Application\chrome.exe";
                              Process Rungoogle = new Process();
                              Rungoogle.StartInfo.FileName = Puthgoogle;//C:\Program Files (x86)\Google\Chrome\Application\
                              Rungoogle.StartInfo.Arguments = selectedDiagnoz.uriDiagnoza;
                              Rungoogle.StartInfo.UseShellExecute = false;
                              Rungoogle.EnableRaisingEvents = true;
                              Rungoogle.Start();
                          }

                      }



                  }));
            }
        }

        #endregion
        #endregion

    }
}

