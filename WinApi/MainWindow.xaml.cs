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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //  var SignProgramPath = @"c:\program Files (x86)\Notepad++\notepad++.exe";
            //  var SignPrograms = @"c:\program Files (x86)\adobe\acrobat Reader DC\reader\AcroRd32.exe";
            //    var FilePath = @"c:\users\jurco\desktop\test.txt";
            //  string proceName = @FilePath + " - Notepad++";


            Signature test = new Signature(@"c:\program Files (x86)\Notepad++\notepad++.exe", @"c:\users\jurco\desktop\test.txt", @"c:\users\jurco\desktop\test.txt - Notepad++");

            bool t = test.SignFile();
        }
    }
}
