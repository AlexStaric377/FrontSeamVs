﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/// "Диференційна діагностика стану нездужання людини-SEAM" 
/// Розробник Стариченко Олександр Павлович тел.+380674012840, mail staric377@gmail.com
namespace FrontSeam
{
    /// <summary>
    /// Логика взаимодействия для WinRegisterAccountUser.xaml
    /// </summary>
    public partial class WinRegisterAccountUser : Window
    {
        public WinRegisterAccountUser()
        {
            InitializeComponent();
            KodCountry.SelectedIndex = 0;
            StatusUser.Content = MapOpisViewModel.RegStatusUser;
            MapOpisViewModel.VisibleGridLoad();
        }
    }
}
