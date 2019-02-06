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
            Guide.initLineArray(); //Invokes Guide.initLineArray() Method

        }


        private void btnSearch_Click(object sender, RoutedEventArgs e) //event that happens when button is clicked
        {
            string bStatCode;
            string aStatCode;
            if (radBStatName.IsChecked == true) //if Station Name radiobutton is checked
            {
                bStatCode = Guide.SearchByStationName(cmbxBStationStrChooser.Text).StationCode[0]; //Search for the input Station Code based on user input
            }
            else //else if Station Code radiobutton is checked
            {
                bStatCode = cmbxBStationStrChooser.Text;
            }

            if (radAStatName.IsChecked == true) //if Station Name radiobutton is checked
            {
                aStatCode = Guide.SearchByStationName(cmbxAStationStrChooser.Text).StationCode[0]; //search for the input Station Code based on user input
            }
            else //else if Station Code radiobutton is checked
            {
                aStatCode = cmbxAStationStrChooser.Text;
            }

            string cardFare = "Stored Value Card Fare -- " + "$" + DBGuide.QueryFareFromDatabase(bStatCode, aStatCode)[0];
            string ticketFare = "Adult Standard Ticket -- " + "$" + DBGuide.QueryFareFromDatabase(bStatCode, aStatCode)[1];
            string timeTaken = "Time Taken -- " + DBGuide.QueryFareFromDatabase(bStatCode, aStatCode)[2] +" minutes";
            bool cardFareValue = radCardFare.IsChecked.Value;
            bool ticketFareValue = radTicketFare.IsChecked.Value;
            bool time = radTime.IsChecked.Value;
            bool fare = radFare.IsChecked.Value;
            string output = string.Empty;
            if (chkAdvFeature.IsChecked.Value)
            {
                if (time)
                {
                    output = Guide.FindPathV2(bStatCode, aStatCode, chkAdvFeature.IsChecked.Value,'T');
                }
                else if (fare)
                {
                    output = Guide.FindPathV2(bStatCode, aStatCode, chkAdvFeature.IsChecked.Value,'F');
                }
                else
                {
                    output = "Error";
                }
            }
            else
            {
                output = Guide.FindPathV2(bStatCode, aStatCode, chkAdvFeature.IsChecked.Value);
            }
           
            
            DisplayResults Results = new DisplayResults(); //create new instance of Results form
            if (cardFareValue)
            {
                Results.Show(); //show Results form
                Results.txtBoxDisplay.Text = "Displaying Route : " + "\n" + output; //calls Guide.FindPathV2 and Displays Output in textbox in DirectionsResults window
                Results.tripDetails.Text = "-- Fare Details and Time -- \n" + cardFare + "\n" + timeTaken;
                this.Hide(); //hides current window
                DBGuide.InsertFareDataIntoHistory(bStatCode, aStatCode,'C');//Insert Query into database
            }
            else if (ticketFareValue)
            {
                Results.Show(); //show Results form
                Results.txtBoxDisplay.Text = "Displaying Route : " + "\n" + output; //calls Guide.FindPathV2 and Displays Output in textbox in DirectionsResults window
                Results.tripDetails.Text = "-- Fare Details and Time -- \n" + ticketFare + "\n" + timeTaken;
                this.Hide(); //hides current window
                DBGuide.InsertFareDataIntoHistory(bStatCode, aStatCode, 'T');//Insert Query into database
            }
            else
            {
                Results.Show(); //show Results form
                Results.txtBoxDisplay.Text = "Displaying Route : " + "\n" + output; //calls Guide.FindPathV2 and Displays Output in textbox in DirectionsResults window
                Results.tripDetails.Text = "-- Fare Details and Time -- \n" + cardFare + "\n" + ticketFare + "\n" + timeTaken+"\nThis fare record will not be inserted into the database as Fare Type has not been selected";
                this.Hide(); //hides current window
            }

                       
            
        }

        private void Button_Click(object sender, RoutedEventArgs e) //event that happens when button is clicked
        {
            MainWindow Home = new MainWindow(); //create new instance of MainWindow object
            Home.Show(); //Show Home Window
            this.Close(); //close Current Window
        }

        private void BoardingStationIdentifier_CheckChanged(object sender, RoutedEventArgs e) //for boarding station groupbox
        {
            if (radBStatName.IsChecked.Value == true) //if radiobutton for station name is checked
            {
                cmbxBStationStrChooser.Items.Clear(); //clear the combobox
                foreach (string stationName in Guide.StationNameStringList()) //foreach loop
                {
                    cmbxBStationStrChooser.Items.Add(stationName); //add station names to the combobox
                }
            }
            else if (radBStatCode.IsChecked.Value == true) //if radiobutton for station code is checked
            {
                cmbxBStationStrChooser.Items.Clear(); //clear the combobox
                foreach (string stationCode in Guide.StationCodeStringList()) //foreach loop
                {
                    cmbxBStationStrChooser.Items.Add(stationCode); //add station codes to the combobox
                }
            }
        }
        private void AlightingStationIdentifier_CheckChanged(object sender, RoutedEventArgs e) //for alighting station groupbox
        {
            if (radAStatName.IsChecked.Value == true) //if radiobutton for station name is checked
            {
                cmbxAStationStrChooser.Items.Clear(); //clear the combobox
                foreach (string stationName in Guide.StationNameStringList()) //foreach loop
                {
                    cmbxAStationStrChooser.Items.Add(stationName); //add station names to the combobox
                }

            }
            else if (radAStatCode.IsChecked.Value == true) //if radiobutton for station code is checked
            {
                cmbxAStationStrChooser.Items.Clear(); //clear the combobox
                foreach (string stationCode in Guide.StationCodeStringList()) //foreach loop
                {
                    cmbxAStationStrChooser.Items.Add(stationCode); //add station codes to the combobox
                }
            }
        }
    }
}
