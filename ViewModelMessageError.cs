using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace FrontSeam
{
    class ViewModelMessageError : INotifyPropertyChanged
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
        RelayCommand? closeError;
        public RelayCommand CloseError
        {
            get
            {
                return closeError ??
                  (closeError = new RelayCommand(obj =>
                  {
                      MessageError WindowMessageError = MainWindow.LinkMainWindow("MessageError");
                      WindowMessageError.Close();
                  }));
            }
        }
    }
}

