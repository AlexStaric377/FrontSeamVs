using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    class ViewModelMessageWarn : BaseViewModel
    {
        private MessageWarn WindowWarn = MainWindow.LinkMainWindow("MessageWarn");

        // команда закрытия окна
        RelayCommand? closeWarning;
        public RelayCommand CloseWarning
        {
            get
            {
                return closeWarning ??
                  (closeWarning = new RelayCommand(obj =>
                  {
                      WindowWarn.Close();
                  }));
            }
        }

        // команда закрытия окна
        RelayCommand? exitInterview;
        public RelayCommand ExitInterview
        {
            get
            {
                return exitInterview ??
                  (exitInterview = new RelayCommand(obj =>
                  {
                      MapOpisViewModel.StopDialog = true;
                      WindowWarn.Close();
                  }));
            }
        }
    }
}
