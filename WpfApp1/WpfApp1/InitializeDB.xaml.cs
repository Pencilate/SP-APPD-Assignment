using Microsoft.Win32;
using System;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for InitializeDB.xaml
    /// </summary>
    public partial class InitializeDB : Window
    {
        public InitializeDB()
        {
            InitializeComponent();
        }

        

        private void ofdMRT_Click(object sender, RoutedEventArgs e)
        {
            string mrtFilePath = ""; //string mrt file Path
            OpenFileDialog ofd = new OpenFileDialog(); //open file dialog
            ofd.InitialDirectory = "C:\\Users\\$USERNAME\\Documents"; //directory of open file dialog
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"; //filter files that can be opened
            ofd.FilterIndex = 0;
            ofd.RestoreDirectory = true;
            Nullable<bool> result = ofd.ShowDialog(); //show openfiledialog
            if (result == true) //if true
            {
                mrtFilePath = ofd.FileName; //set mrtfilepath to filename selected
                txtMRT.Text = mrtFilePath; //set txtmrt to filepath
            }
        }

        private void ofdFare_Click(object sender, RoutedEventArgs e)
        {
            string fareFilePath = ""; //string fare file path
            OpenFileDialog ofd = new OpenFileDialog(); //open file dialog
            ofd.InitialDirectory = "C:\\Users\\$USERNAME\\Documents"; //directory of open file dialog
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"; //filter files that can be opened
            ofd.FilterIndex = 0;
            ofd.RestoreDirectory = true;
            Nullable<bool> result = ofd.ShowDialog(); //show openfiledialog 
            if (result == true) //if true
            {
                fareFilePath = ofd.FileName; //set farefilepath to filename selected
                txtFare.Text = fareFilePath; //set txtfare to filepath
            }
        }
        private void MRTButton_Click(object sender, RoutedEventArgs e)
        {
            FileIO.textMRTFileReaderToDB(txtMRT.Text); //initialize DB 
            
            MessageBox.Show("MRT database has been initialized");
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Home = new MainWindow();
            Home.Show();
            this.Close();
        }

        private void FareButton_Click(object sender, RoutedEventArgs e)
        {
            FileIO.textFareFileReaderToDB(txtFare.Text); //initialize DB
            MessageBox.Show("Fare database has been initialized");            
        }
    }
}

