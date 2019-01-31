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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileIO.textMRTFileReaderToDB(txtMRT.Text);
            FileIO.textFareFileReaderToDB(txtFare.Text);
            MessageBox.Show("Database has been initialized");
            this.Hide();
            MainWindow Display = new MainWindow();
            Display.Show();
            this.Hide();
        }

        private void ofdMRT_Click(object sender, RoutedEventArgs e)
        {
            string mrtFilePath = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\Users\\$USERNAME\\Documents";
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 0;
            ofd.RestoreDirectory = true;
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                mrtFilePath = ofd.FileName;
                txtMRT.Text = mrtFilePath;
            }
        }

        private void ofdFare_Click(object sender, RoutedEventArgs e)
        {
            string fareFilePath = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\Users\\$USERNAME\\Documents";
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 0;
            ofd.RestoreDirectory = true;
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                fareFilePath = ofd.FileName;
                txtFare.Text = fareFilePath;
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Home = new MainWindow();
            Home.Show();
            this.Close();
        }
    }
}

