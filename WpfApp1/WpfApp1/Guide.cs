using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Guide
    {
        private static List<Line> MRT = new List<Line>();
        private static bool GraphInitialized = false;

        public static List<Line> MRTLine //MRTLine list
        {
            get { return MRT; }
        }

        public static void initLineArray() //Initializes LineArray
        {
            string FilePath = "..\\..\\resources\\MRT.txt"; //original file path in resources folder under the project folder            
            //FilePath Points to the MRT.txt file
            MRT = FileIO.textFileReader(FilePath);
        }

        public static List<string> StationNameStringList()
        {
            List<string> stationNameStrLst = new List<string>(); //new list
            foreach (Line line in MRT) //foreach loop
            {
                foreach (Station stat in line.StationList) //inner foreach loop
                {
                    string statName = stat.StationName;
                    if (!stationNameStrLst.Contains(statName)) //if stationNameStrLst list does not contain statName
                    {
                        stationNameStrLst.Add(statName); //add station name to stationNameStrLst list
                    }
                }
            }
            stationNameStrLst.Sort(); //sort the list
            return stationNameStrLst; //return the list
        }

        public static List<string> StationCodeStringList()
        {
            List<string> stationCodeStrLst = new List<string>(); //new list
            foreach (Line line in MRT) //foreach loop
            {
                foreach (Station stat in line.StationList) //inner foreach loop
                {
                    foreach (string statCd in stat.StationCode) //innermost foreach loop
                    {
                        if (!stationCodeStrLst.Contains(statCd)) //if stationCodeStrLst does not contain statCd
                        {
                            stationCodeStrLst.Add(statCd); //add station code to the stationCodeStrLst list
                        }
                    }
                }
            }
            stationCodeStrLst.Sort(); //sort the list
            return stationCodeStrLst; //return the list
        }


        public static Station SearchByStationCd(string searchInput) //This method scans through the MRT List of Line Objects then the List of Station Objects within it to find the specific station object and return it.
        {
            searchInput = searchInput.ToUpper(); //Makes the input case insensitive
            string lineCd = searchInput.Substring(0, 2);

            int ResultLineIndex = GetIndexOfLine(lineCd); // Sets ResultLineIndex to index of input.

            Station ResultStation = new Station(); //creates new Station Object
                                                   //foreach (Station sta in MRT[ResultLineIndex].StationList)
            for (int i = 0; i < MRT[ResultLineIndex].StationList.Count; i++)
            {
                if (MRT[ResultLineIndex].StationList[i].IsInterchange) //if input station is a Interchange
                {
                    for (int j = 0; j < MRT[ResultLineIndex].StationList[i].StationCode.Count; j++) //for loop
                    {
                        if (searchInput.Equals(MRT[ResultLineIndex].StationList[i].StationCode[j])) // if searchInput matches StationCode in Station List
                        {
                            ResultStation = MRT[ResultLineIndex].StationList[i]; //sets Station Object name to Station Name
                            break;
                        }
                    }
                }
                else
                {
                    if (searchInput.Equals(MRT[ResultLineIndex].StationList[i].StationCode[0])) //if input station is not a Interchange
                    {
                        ResultStation = MRT[ResultLineIndex].StationList[i]; //sets Station Object name to Station Name
                        break;
                    }
                }

            }
            return ResultStation; //return Station object
        }

        public static Station SearchByStationName(string searchInput)
        {
            Station ResultStation = new Station(); //creates new Station Object
            for (int i = 0; i < MRT.Count; i++) //for loop
            {
                for (int j = 0; j < MRT[i].StationList.Count; j++)
                {
                    if (searchInput.Equals(MRT[i].StationList[j].StationName)) //if search input equals to station names in MRT list
                    {
                        ResultStation = MRT[i].StationList[j]; //sets Station Object code to matching Station Code
                        break;
                    }
                }
            }
            return ResultStation; //return Station object
        }

        public static int GetIndexOfLine(string lineCd) //method to find index of line
        {
            int ResultLineIndex = -1;

            for (int index = 0; index < MRT.Count; index++) //for loop
            {
                if (lineCd.Equals(MRT[index].LineCd)) //checks if input line code matches LineCd in MRT
                {
                    ResultLineIndex = index; //sets ResultLineIndex to index where input lineCd was found
                    break;
                }
            }
            return ResultLineIndex;
        }

        public static int GetStationIndexFromLine(int lineIndex, string StationName) //gets Station Index using Line Index
        {
            int index = -1;
            for (int i = 0; i < MRT[lineIndex].StationList.Count; i++) //for loop
            {
                if (MRT[lineIndex].StationList[i].StationName.Equals(StationName)) //checks to see if Station Name in MRT List matches the input station name
                {
                    index = i; //sets index to the index where the Station was found.
                    break;
                }
            }
            return index; //returns the index
        }

        public static string DisplayRoute(string StationCd)
        {
            string output = "";
            StationCd = StationCd.ToUpper(); //makes input not case sensitive
            string InputLineCd = StationCd.Substring(0, 2);//extracts first 2 characters from StationCd and assigns it to InputLineCd
            int LineIndex = GetIndexOfLine(InputLineCd);
            output += string.Format("Listing station for {0} Line\r\n", InputLineCd);
            for (int StationIndex = 0; StationIndex < MRT[LineIndex].StationList.Count; StationIndex++) //cycles through the StationList for the line stationCd is in
            {
                if (MRT[LineIndex].StationList[StationIndex].IsInterchange)
                {
                    bool IsStation = false; //boolean variable for checking if station is the station in the input
                    for (int i = 0; i < MRT[LineIndex].StationList[StationIndex].StationCode.Count; i++)//cycles through the different station codes a interchange may have.
                    {
                        if (MRT[LineIndex].StationList[StationIndex].StationCode[i].Equals(StationCd)) //checks if the current Station Code in MRT Station List matches input stationCd
                        {
                            IsStation = true;//if this station matches the stationCd, sets IsStation to true;
                        }
                    }

                    if (IsStation) //if true
                    {
                        output += string.Format("#{0} - {1} - {2}\r\n", StationCd, MRT[LineIndex].StationList[StationIndex].StationName, "Interchange"); //Add info the string output variable
                    }
                    else //else if false
                    {
                        for (int i = 0; i < MRT[LineIndex].StationList[StationIndex].StationCode.Count; i++)//cycles through the different station codes a interchange may have.
                        {
                            if (MRT[LineIndex].StationList[StationIndex].StationCode[i].StartsWith((InputLineCd))) //checks if the station code have the correct line code
                            {
                                output += string.Format("{0} - {1} - {2}\r\n", MRT[LineIndex].StationList[StationIndex].StationCode[i], MRT[LineIndex].StationList[StationIndex].StationName, "Interchange"); // add info to the string output variable
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (MRT[LineIndex].StationList[StationIndex].StationCode[0].Equals(StationCd))  //checks if the current Station Code in MRT Station List matches input stationCd
                    {
                        output += string.Format("#{0} - {1}\r\n", StationCd, MRT[LineIndex].StationList[StationIndex].StationName);//if this station matches the stationCd,add info to the string output variable
                    }
                    else
                    {
                        output += string.Format("{0} - {1}\r\n", MRT[LineIndex].StationList[StationIndex].StationCode[0], MRT[LineIndex].StationList[StationIndex].StationName);//otherwise, add this info to the string output variable
                    }
                }
            }
            return output; //return string
        }

        public static string DisplayFindPath(int lineIndex, int ssIndex, int esIndex) //method to display entire line
        {
            string lineCd = MRT[lineIndex].LineCd; //sets LineCd to LineCd in MRT List
            string output = string.Empty;
            if (esIndex > ssIndex) //if ending index is greater than the starting index
            {
                for (int i = ssIndex; i <= esIndex; i++) //for loop
                {
                    string extractedStatCd = "";
                    if (MRT[lineIndex].StationList[i].IsInterchange) //if statement to check if Station is an Interchange
                    {
                        foreach (string statCd in MRT[lineIndex].StationList[i].StationCode) //foreach loop  
                        {
                            if (statCd.Contains(lineCd)) //to check if StationCode in MRT contains input LineCd
                            {
                                extractedStatCd = statCd; //set extracted Station Code to be equals to Station Code

                            }
                        }
                    }
                    else //else
                    {
                        extractedStatCd = MRT[lineIndex].StationList[i].StationCode[0]; //sets extracted Station Code to be equals to StationCode in MRT
                    }
                    output += extractedStatCd + " - " + MRT[lineIndex].StationList[i].StationName + "\n"; //output string
                } //end of for loop


            }
            else if (ssIndex > esIndex) //if starting index is greater than the ending index
            {
                for (int i = ssIndex; i >= esIndex; i--) //for loop
                {
                    string extractedStatCd = "";
                    if (MRT[lineIndex].StationList[i].IsInterchange) //check if Station is an Interchange
                    {
                        foreach (string statCd in MRT[lineIndex].StationList[i].StationCode) //foreach loop
                        {
                            if (statCd.Contains(lineCd)) //check if StationCode in MRT contains input line code
                            {
                                extractedStatCd = statCd; //set extracted Station Code to be equals to Station Code

                            }
                        }

                    }
                    else //else
                    {
                        extractedStatCd = MRT[lineIndex].StationList[i].StationCode[0]; //sets extracted station code to be equals to StationCode in MRT
                    }
                    output += extractedStatCd + " - " + MRT[lineIndex].StationList[i].StationName + "\n"; //output string
                } //end of for loop
            }
            else //else statement
            {
                output = "Same Station for Start and End";
            }
            return output;//returns output string
        }

        public static string FindPathV2(string StartingStatCd, string EndingStatCd, bool useAdvFeature) //method to find route between 2 stations
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
            if (useAdvFeature) //if advanced feature (dijkstra) is used
            {
                routeAvailable = true;
            }
            else
            {
                if (!directRoute) //if a direct route is not available
                {

                    for (int i = 0; i < statCdPair.Count; i++) //for loop
                    {
                        int lineIndex = GetIndexOfLine((statCdPair[i])[0]); //gets the index of the line of the first pair of station codes

                        foreach (Station stat in MRT[lineIndex].StationList) //foreach loop
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
            }

            if (routeAvailable) //if a route is available
            {
                string output = "";
                if (directRoute) //if a direct route is available
                {
                    int lineIndex = GetIndexOfLine(statCdPair[selectedStatCdPairIndex][0]); //sets LineIndex to Line Index of the selected Station Code Pair 

                    int ssIndex = GetStationIndexFromLine(lineIndex, StartStat.StationName); //sets Starting Index to the Station Index of the Starting Station
                    int esIndex = GetStationIndexFromLine(lineIndex, EndStat.StationName); //sets Ending Index to the Station Index of the Ending Station
                    output = string.Format("Display Route from {0} to {1} - Taking {2} stations\r\n-- Start of Route --\r\n{3}-- End of Route --", StartStat.StationName, EndStat.StationName, (Math.Abs(esIndex - ssIndex)), DisplayFindPath(lineIndex, ssIndex, esIndex)); //invokes DisplayFindPath and returns route 
                }
                else if (useAdvFeature) //else if true
                {
                    if (!GraphInitialized) //if Graph is not initialized
                    {
                        GraphRoute.initStationIndex(); //initialize station index
                        GraphRoute.initGraph(); //initialize Graph
                    }
                    output = GraphRoute.initTraverseDijkstra(StartStat.GraphIndex, EndStat.GraphIndex); //invokes GraphRoute.initTraverseDijkstra and stores return value to string output
                }
                else//else if a direct route is not available (need to change trains at an interchange)
                {
                    int lineIndex = GetIndexOfLine(statCdPair[selectedStatCdPairIndex][0]); //sets Line Index to Line Index of the selected Station Code Pair

                    int ssIndex = GetStationIndexFromLine(lineIndex, StartStat.StationName); //sets Starting Index to the Station Index of the Starting Station
                    int icsArrvIndex = GetStationIndexFromLine(lineIndex, interchangeName); //sets Ending Index to the Station Index of the Interchange                                            

                    int lineIndex2 = GetIndexOfLine(statCdPair[selectedStatCdPairIndex][1]); //sets Line Index to Line Index of the second selected Station Code Pair

                    int icsDeprtIndex = GetStationIndexFromLine(lineIndex2, interchangeName); //sets departing index to the Station Index of the Interchange
                    int esIndex = GetStationIndexFromLine(lineIndex2, EndStat.StationName); //sets Final ending Index to the Station Index of the Ending Station

                    output = output = string.Format("Display Route from {0} to {1} - Taking {2} stations\r\n-- Start of Route --\r\n{3}-- End of Route --", StartStat.StationName, EndStat.StationName, (Math.Abs(icsArrvIndex - ssIndex) + Math.Abs(esIndex - icsDeprtIndex)), DisplayFindPath(lineIndex, ssIndex, icsArrvIndex) + DisplayFindPath(lineIndex2, icsDeprtIndex, esIndex)); //invokes DisplayFindPath and returns it
                }
                return output;
            }
            else //else if no route is available between both stations
            {
                return "No route available";
            }
        }
    }
}
