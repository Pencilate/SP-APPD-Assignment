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
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Directions.xaml
    /// </summary>
    public partial class Directions : Window
    {
        public Directions()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) //event that happens when button is clicked
        {
            Guide.initLineArray(); //Invokes Guide.initLineArray() Method
            string bStatCode;
            string aStatCode;
            if (radBStatName.IsChecked == true) //if Station Name CheckBox is checked
            {
                bStatCode = Guide.SearchByStationName(cmbxBStationStrChooser.Text).StationCode[0]; //Search for the input Station Code based on user input
            }
            else //else if Station Code CheckBox is checked
            {
                bStatCode = cmbxBStationStrChooser.Text;
            }

            if (radAStatName.IsChecked == true) //if Station Name CheckBox is checked
            {
                aStatCode = Guide.SearchByStationName(cmbxAStationStrChooser.Text).StationCode[0]; //search for the input Station Code based on user input
            }
            else //else if Station Code CheckBox is checked
            {
                aStatCode = cmbxAStationStrChooser.Text;
            }

            DisplayResults Results = new DisplayResults(); //create new instance of Results form
            Results.Show(); //show Results form
            Results.txtBoxDisplay.Text = "Displaying Route : " + "\n" + Guide.FindPathV2(bStatCode, aStatCode); //calls Guide.FindPathV2 and Displays Output in textbox in DirectionsResults window
            this.Hide(); //hides current window

        }



        private void Button_Click(object sender, RoutedEventArgs e) //event that happens when button is clicked
        {
            MainWindow Home = new MainWindow(); //create new instance of MainWindow object
            Home.Show(); //Show Home Window
            this.Close(); //close Current Window
        }

        private void StationIdentifier_CheckChanged(object sender, RoutedEventArgs e)
        {
            Guide.initLineArray();
            Station resultStat = new Station();
            if (radBStatName.IsChecked.Value == true)
            {
                cmbxBStationStrChooser.Items.Clear();
                foreach (string stationName in Guide.StationNameStringList())
                {
                    cmbxBStationStrChooser.Items.Add(stationName);
                }

            }
            else if (radBStatCode.IsChecked.Value == true)
            {
                cmbxAStationStrChooser.Items.Clear();
                foreach (string stationCode in Guide.StationCodeStringList())
                {
                    cmbxBStationStrChooser.Items.Add(stationCode);
                }
            }

            if (radAStatName.IsChecked.Value == true)
            {
                cmbxAStationStrChooser.Items.Clear();
                foreach (string stationName in Guide.StationNameStringList())
                {
                    cmbxAStationStrChooser.Items.Add(stationName);
                }

            }
            else if (radAStatCode.IsChecked.Value == true)
            {
                cmbxAStationStrChooser.Items.Clear();
                foreach (string stationCode in Guide.StationCodeStringList())
                {
                    cmbxAStationStrChooser.Items.Add(stationCode);
                }
            }

        }
    }
}
