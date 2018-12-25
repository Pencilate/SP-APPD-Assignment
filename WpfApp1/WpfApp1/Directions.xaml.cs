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
            if (chkBName.IsChecked == true) //if Station Name CheckBox is checked
            {
                bStatCode = Guide.SearchByStationName(txtBoarding.Text).StationCode[0]; //Search for the input Station Code based on user input
            }
            else //else if Station Code CheckBox is checked
            {
                bStatCode = txtBoarding.Text;
            }

            if (chkAName.IsChecked == true) //if Station Name CheckBox is checked
            {
                aStatCode = Guide.SearchByStationName(txtAlighting.Text).StationCode[0]; //search for the input Station Code based on user input
            }
            else //else if Station Code CheckBox is checked
            {
                aStatCode = txtAlighting.Text;
            }

            DirectionsResults Results = new DirectionsResults(); //create new instance of Results form
            Results.Show(); //show Results form
            Results.txtBoxDisplay.Text = "Displaying Route : " + "\n" + Guide.FindPathV2(bStatCode, aStatCode); //calls Guide.FindPathV2 and Displays Output in textbox in DirectionsResults window
            this.Hide(); //hides current window

        }

        public class Station //Station class
        {
            private string stationName;
            private bool isInterchange = false;

            private List<string> stationCode = new List<string>();

            public Station(string stationCd, string stationName) //Station constructor with parameters
            {
                this.stationName = stationName;
                StationCode.Add(stationCd);
            }

            public Station() { } //Station constructor

            public List<string> StationCode //StationCode property of Station object
            {
                get { return stationCode; } //get method
                set { this.stationCode = value; } //set method
            }

            public string StationName //StationName property of Station object
            {
                get { return stationName; } //get method
                set { this.stationName = value; } //set method
            }

            public bool IsInterchange //Determines whether Station object is an interchange or not
            {
                get { return isInterchange; } //get method
                set { this.isInterchange = value; } //set method
            }
        }

        public class Line //Line class
        {
            private string lineCd;
            private List<Station> StationLine = new List<Station>();

            public Line(string lineCd) //Line constructor with parameters
            {
                this.lineCd = lineCd;
            }
            public Line() //Line constructor
            {

            }

            public void AddStationToLine(string StationCd, string StationName) //Adds a Station to the Line
            {
                StationLine.Add(new Station(StationCd, StationName));
            }

            internal Station getStation(int index) //Returns station object based on its index 
            {
                return StationLine[index];
            }

            internal List<Station> getStationList() //Returns Station Line
            {
                return StationLine;
            }

            public string LineCd //Line Code property of Line object
            {
                get { return lineCd; } //get method
                set { this.lineCd = value; } //set method
            }

        }

        public class FileIO //File input and output 
        {
            public static List<Line> textFileReader(string FilePath)
            {
                List<Line> MRT = new List<Line>();
                string StationName;
                int LineNo = 0;
                using (StreamReader reader = new StreamReader(FilePath)) //reads the text file
                {
                    string lineData; //lineData variable
                    while ((lineData = reader.ReadLine()) != null)
                    {
                        switch (lineData)
                        {
                            case "(start)":
                                string LineStationCdStr = reader.ReadLine();
                                string LineCd = LineStationCdStr.Substring(0, 2);
                                Console.WriteLine(LineCd);

                                MRT.Add(new Line(LineCd));
                                if (LineCd.Equals("CG"))
                                { //Special Case for Tanah Merah
                                    MRT[LineNo].AddStationToLine("CG0", "Tanah Merah");
                                }
                                StationName = reader.ReadLine();
                                MRT[LineNo].AddStationToLine(LineStationCdStr, StationName);
                                break;
                            case "(end)":
                                LineNo++;
                                break;
                            default:
                                StationName = reader.ReadLine();
                                MRT[LineNo].AddStationToLine(lineData, StationName);
                                break;
                        }
                    }

                }

                //Comparing station to set them as interchange
                for (int count = (MRT.Count - 1); count > 0; count--)
                {
                    int CurrentLine = count;
                    int ComparedLine = count - 1;
                    for (int i = ComparedLine; i >= 0; i--)
                    {

                        for (int j = 0; j < MRT[CurrentLine].getStationList().Count(); j++)
                        {

                            for (int h = 0; h < MRT[i].getStationList().Count(); h++)
                            {
                                //Compare name of stations between CurrentLine and ComparedLine
                                Console.WriteLine("{0} {1} VS {2} {3}", MRT[CurrentLine].LineCd, MRT[CurrentLine].getStationList()[j].StationName, MRT[i].LineCd, MRT[i].getStationList()[h].StationName);
                                if (MRT[CurrentLine].getStationList()[j].StationName.Equals(MRT[i].getStationList()[h].StationName))
                                {
                                    //if the station names match,set variable to identify it as a interchange as true                              
                                    MRT[CurrentLine].getStationList()[j].IsInterchange = true;
                                    foreach (string str in MRT[CurrentLine].getStationList()[j].StationCode)
                                    {
                                        Console.Write(str + " ");
                                    }
                                    Console.WriteLine();
                                    foreach (string str in MRT[i].getStationList()[h].StationCode)
                                    {
                                        Console.Write(str + " ");
                                    }
                                    Console.WriteLine();

                                    if ((MRT[CurrentLine].getStationList()[j].StationCode) != (MRT[i].getStationList()[h].StationCode))
                                    {
                                        MRT[CurrentLine].getStationList()[j].StationCode.AddRange(MRT[i].getStationList()[h].StationCode);
                                    }

                                    foreach (string str in MRT[CurrentLine].getStationList()[j].StationCode)
                                    {
                                        Console.Write(str + " ");
                                    }
                                    Console.WriteLine();

                                    MRT[i].getStationList()[h] = MRT[CurrentLine].getStationList()[j];

                                }//end of if statement

                            }
                        }

                    }

                }

                return MRT; //return MRT List
            }
        }

        public class Guide
        {
            private static List<Line> MRT = new List<Line>();

            public static void initLineArray() //Initializes LineArray
            {
                string FilePath = "..\\..\\resources\\MRT.txt";
                //FilePath Points to the MRT.txt file
                MRT = FileIO.textFileReader(FilePath);

            }

            public static Station SearchByStationCd(string searchInput) //This method scans through the MRT List of Line Objects then the List of Station Objects within it to find the specific station object and return it.
            {
                searchInput = searchInput.ToUpper(); //Makes the input case insensitive
                string lineCd = searchInput.Substring(0, 2);

                int ResultLineIndex = GetIndexOfLine(lineCd); // Sets ResultLineIndex to index of input.

                Station ResultStation = new Station(); //creates new Station Object
                //foreach (Station sta in MRT[ResultLineIndex].getStationList())
                for (int i = 0; i < MRT[ResultLineIndex].getStationList().Count; i++)
                {
                    if (MRT[ResultLineIndex].getStationList()[i].IsInterchange) //if input station is a Interchange
                    {
                        for (int j = 0; j < MRT[ResultLineIndex].getStationList()[i].StationCode.Count; j++) //for loop
                        {
                            if (searchInput.Equals(MRT[ResultLineIndex].getStationList()[i].StationCode[j])) // if searchInput matches StationCode in Station List
                            {
                                ResultStation = MRT[ResultLineIndex].getStationList()[i]; //sets Station Object name to Station Name
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (searchInput.Equals(MRT[ResultLineIndex].getStationList()[i].StationCode[0])) //if input station is not a Interchange
                        {
                            ResultStation = MRT[ResultLineIndex].getStationList()[i]; //sets Station Object name to Station Name
                            break;
                        }
                    }

                }
                return ResultStation;
            }

            public static Station SearchByStationName(string searchInput)
            {
                Station ResultStation = new Station(); //creates new Station Object
                for (int i = 0; i < MRT.Count; i++) //for loop
                {
                    for (int j = 0; j < MRT[i].getStationList().Count; j++)
                    {
                        if (searchInput.Equals(MRT[i].getStation(j).StationName)) //if search input equals to station names in MRT list
                        {
                            ResultStation = MRT[i].getStation(j); //sets Station Object code to matching Station Code
                            break;
                        }
                    }
                }
                return ResultStation;
            }

            public static int GetIndexOfLine(string lineCd) //method to find index of line
            {
                int ResultLineIndex = -1;

                for (int index = 0; index < MRT.Count; index++) //for loop
                {
                    if (lineCd.Equals(MRT[index].LineCd)) //if input LineCd equals to MRT list linecd
                    {
                        ResultLineIndex = index; //sets ResultLineIndex to index of inputted LineCd
                        break;
                    }
                }
                return ResultLineIndex;
            }


            public static string DisplayFindPath(int lineIndex, int ssIndex, int esIndex) //method to display entire line
            {
                string lineCd = MRT[lineIndex].LineCd; //sets LineCd to LineCd in MRT List
                string output1 = string.Empty;
                string output2 = string.Empty;
                if (esIndex > ssIndex) //if ending index is greater than the starting index
                {
                    for (int i = ssIndex; i <= esIndex; i++) //for loop
                    {
                        string extractedStatCd = "";
                        if (MRT[lineIndex].getStationList()[i].IsInterchange) //if statement to check if Station is an Interchange
                        {
                            foreach (string statCd in MRT[lineIndex].getStationList()[i].StationCode) //foreach loop  
                            {
                                if (statCd.Contains(lineCd)) //to check if StationCode in MRT contains input LineCd
                                {
                                    extractedStatCd = statCd; //set extracted Station Code to be equals to Station Code

                                }
                            }
                        }
                        else //else
                        {
                            extractedStatCd = MRT[lineIndex].getStationList()[i].StationCode[0]; //sets extracted Station Code to be equals to StationCode in MRT
                        }
                        output1 += "\n" + extractedStatCd + " - " + MRT[lineIndex].getStationList()[i].StationName; //output string
                    } //end of for loop
                    return output1; //returns output string

                }
                else if (ssIndex > esIndex) //if starting index is greater than the ending index
                {
                    for (int i = ssIndex; i >= esIndex; i--) //for loop
                    {
                        string extractedStatCd = "";
                        if (MRT[lineIndex].getStationList()[i].IsInterchange) //check if Station is an Interchange
                        {
                            foreach (string statCd in MRT[lineIndex].getStationList()[i].StationCode) //foreach loop
                            {
                                if (statCd.Contains(lineCd)) //check if StationCode in MRT contains input line code
                                {
                                    extractedStatCd = statCd; //set extracted Station Code to be equals to Station Code

                                }
                            }

                        }
                        else //else
                        {
                            extractedStatCd = MRT[lineIndex].getStationList()[i].StationCode[0]; //sets extracted station code to be equals to StationCode in MRT
                        }
                        output2 += "\n" + extractedStatCd + " - " + MRT[lineIndex].getStationList()[i].StationName; //output string
                    } //end of for loop
                    return output2; //returns output string
                } 
                else //else statement
                {
                    return "Same Station for Start and End";
                }
            }



            public static string FindPathV2(string StartingStatCd, string EndingStatCd) //method to find route between 2 stations
            {
                Station StartStat = SearchByStationCd(StartingStatCd); //searches for station by Station Code, then returns and stores station object in StartStat object
                Station EndStat = SearchByStationCd(EndingStatCd); //searches for station by Station Code, then returns and stores station object in EndStat object

                List<string[]> statCdPair = new List<string[]>(); //creates a List with String Array

                for (int i = 0; i < StartStat.StationCode.Count; i++) //nested loop
                {
                    for (int j = 0; j < EndStat.StationCode.Count; j++) //inner for loop
                    {

                        string ssLineCd = StartStat.StationCode[i].Substring(0, 2); //sets starting LineCd to extracted Starting StationCode (first 2 characters)
                        string esLineCd = EndStat.StationCode[j].Substring(0, 2); //sets ending LineCd to extracted Ending StationCode (first 2 characters)
                        string[] cdPair = { ssLineCd, esLineCd }; //creates array of starting and ending pair of station codes
                        statCdPair.Add(cdPair); //adds array to List with String Array


                    }
                }

                bool directRoute = false; //boolean variable to check if a directRoute is available
                bool routeAvailable = false; //boolean variable to check if a route is available
                string interchangeName = "";
                int selectedStatCdPairIndex = -1;
                for (int i = 0; i < statCdPair.Count; i++) //for loop
                {
                    if ((statCdPair[i])[0].Equals((statCdPair[i])[1])) //checks to see if first StationCodePair is equal to the second StationCodePair
                    {
                        directRoute = true; //a direct route between both stations is available.
                        routeAvailable = true; //a route between both stations is available.
                        selectedStatCdPairIndex = i;
                    }
                }

                if (!directRoute) //if a direct route is not available
                {

                    for (int i = 0; i < statCdPair.Count; i++) //for loop
                    {
                        int lineIndex = GetIndexOfLine((statCdPair[i])[0]); //gets the index of the line of the first pair of station codes

                        foreach (Station stat in MRT[lineIndex].getStationList()) //foreach loop
                        {
                            if (stat.IsInterchange) //if Station is an Interchange
                            {
                                List<string> lineOffered = new List<string>(); //creates a string list
                                foreach (string statCd in stat.StationCode) //foreach loop
                                {
                                    lineOffered.Add(statCd.Substring(0, 2)); //add Station Code to string list
                                }

                                if ((lineOffered.Contains(statCdPair[i][0])) && (lineOffered.Contains(statCdPair[i][1]))) //checks to see if a common Station (interchange) is found 
                                {
                                    routeAvailable = true; // a route between both stations is available.
                                    selectedStatCdPairIndex = i; //sets index of interchange Station to selectedStatCdPairIndex
                                    interchangeName = stat.StationName; //sets name of the Interchange Station to interchangeName
                                    break;
                                }


                            }
                        }


                    }
                }

                if (routeAvailable) //if a route is available
                {
                    if (directRoute) //if a direct route is available
                    {
                        int lineIndex = GetIndexOfLine(statCdPair[selectedStatCdPairIndex][0]); //sets LineIndex to Line Index of the selected Station Code Pair 

                        int ssIndex = GetStationIndexFromLine(lineIndex, StartStat.StationName); //sets Starting Index to the Station Index of the Starting Station
                        int esIndex = GetStationIndexFromLine(lineIndex, EndStat.StationName); //sets Ending Index to the Station Index of the Ending Station
                        return DisplayFindPath(lineIndex, ssIndex, esIndex); //invokes DisplayFindPath and returns route 
                    }
                    else //else if a direct route is not available (need to change trains at an interchange)
                    {
                        int lineIndex = GetIndexOfLine(statCdPair[selectedStatCdPairIndex][0]); //sets Line Index to Line Index of the selected Station Code Pair

                        int ssIndex = GetStationIndexFromLine(lineIndex, StartStat.StationName); //sets Starting Index to the Station Index of the Starting Station
                        int icsArrvIndex = GetStationIndexFromLine(lineIndex, interchangeName); //sets Ending Index to the Station Index of the Interchange                                            

                        int lineIndex2 = GetIndexOfLine(statCdPair[selectedStatCdPairIndex][1]); //sets Line Index to Line Index of the second selected Station Code Pair

                        int icsDeprtIndex = GetStationIndexFromLine(lineIndex2, interchangeName); //sets departing index to the Station Index of the Interchange
                        int esIndex = GetStationIndexFromLine(lineIndex2, EndStat.StationName); //sets Final ending Index to the Station Index of the Ending Station

                        return DisplayFindPath(lineIndex, ssIndex, icsArrvIndex) + DisplayFindPath(lineIndex2, icsDeprtIndex, esIndex); //invokes DisplayFindPath and returns it
                    }
                }
                else //else if no route is available between both stations
                {
                    return "No route available";
                }
            }


            public static int GetStationIndexFromLine(int lineIndex, string StationName) //gets Station Index using Line Index
            {
                int index = -1;
                for (int i = 0; i < MRT[lineIndex].getStationList().Count; i++) //for loop
                {
                    if (MRT[lineIndex].getStationList()[i].StationName.Equals(StationName)) //checks to see if Station Name in MRT List matches the input station name
                    {
                        index = i; //sets index to the index where the Station was found.
                        break;
                    }
                }
                return index; //returns the index
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e) //event that happens when button is clicked
        {
            MainWindow Home = new MainWindow(); //create new instance of MainWindow object
            Home.Show(); //Show Home Window
            this.Close(); //close Current Window
        }
    }
}
