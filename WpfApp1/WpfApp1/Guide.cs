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

        public static void initLineArray() //Initializes LineArray
        {
            string FilePath = "..\\..\\resources\\MRT.txt";
            //FilePath Points to the MRT.txt file
            MRT = FileIO.textFileReader(FilePath);

        }

        public static List<string> StationNameStringList()
        {
            List<string> stationNameStrLst = new List<string>();
            foreach (Line line in MRT)
            {
                foreach (Station stat in line.getStationList())
                {
                    string statName = stat.StationName;
                    if (!stationNameStrLst.Contains(statName))
                    {
                        stationNameStrLst.Add(statName);
                    }
                }
            }
            stationNameStrLst.Sort();
            return stationNameStrLst;
        }

        public static List<string> StationCodeStringList()
        {
            List<string> stationCodeStrLst = new List<string>();
            foreach (Line line in MRT)
            {
                foreach (Station stat in line.getStationList())
                {
                    foreach (string statCd in stat.StationCode)
                    {
                        if (!stationCodeStrLst.Contains(statCd))
                        {
                            stationCodeStrLst.Add(statCd);
                        }
                    }
                }
            }
            stationCodeStrLst.Sort();
            return stationCodeStrLst;
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

        //public static void DisplayRoute(string StationCd)
        //{
        //    StationCd = StationCd.ToUpper();
        //    string InputLineCd = StationCd.Substring(0, 2);
        //    int LineIndex = -1;
        //    for (int index = 0; index < MRT.Count; index++)
        //    {
        //        if (InputLineCd.Equals(MRT[index].LineCd))
        //        {
        //            LineIndex = index;
        //            break;
        //        }
        //    }
        //    Console.WriteLine("Listing station for {0} Line", InputLineCd);
        //    for (int StationIndex = 0; StationIndex < MRT[LineIndex].getStationList().Count; StationIndex++)
        //    {
        //        if (MRT[LineIndex].getStationList()[StationIndex].IsInterchange)
        //        {
        //            bool IsStation = false;
        //            for (int i = 0; i < MRT[LineIndex].getStationList()[StationIndex].StationCode.Count; i++)
        //            {
        //                if (MRT[LineIndex].getStationList()[StationIndex].StationCode[i].Equals(StationCd))
        //                {
        //                    IsStation = true;
        //                }
        //            }

        //            if (IsStation)
        //            {
        //                Console.WriteLine("#{0} - {1} - {2}", StationCd, MRT[LineIndex].getStationList()[StationIndex].StationName, "Interchange");
        //            }
        //            else
        //            {
        //                for (int i = 0; i < MRT[LineIndex].getStationList()[StationIndex].StationCode.Count; i++)
        //                {
        //                    if (MRT[LineIndex].getStationList()[StationIndex].StationCode[i].StartsWith((InputLineCd)))
        //                    {
        //                        Console.WriteLine("{0} - {1} - {2}", MRT[LineIndex].getStationList()[StationIndex].StationCode[i], MRT[LineIndex].getStationList()[StationIndex].StationName, "Interchange");
        //                        break;
        //                    }
        //                }
        //            }



        //        }
        //        else
        //        {
        //            if (MRT[LineIndex].getStationList()[StationIndex].StationCode[0].Equals(StationCd))
        //            {
        //                Console.WriteLine("#{0} - {1}", StationCd, MRT[LineIndex].getStationList()[StationIndex].StationName);
        //            }
        //            else
        //            {
        //                Console.WriteLine("{0} - {1}", MRT[LineIndex].getStationList()[StationIndex].StationCode[0], MRT[LineIndex].getStationList()[StationIndex].StationName);
        //            }
        //        }
        //    }

        //}

        public static string DisplayRoute(string StationCd) //method to DisplayRoute
        {
            string isInterchange = string.Empty;
            string interchange = string.Empty;
            string output = string.Empty;
            string output1 = string.Empty;
            string interchangeOutput = string.Empty;
            string finaloutput = string.Empty;
            StationCd = StationCd.ToUpper(); //makes input not case sensitive
            string InputLineCd = StationCd.Substring(0, 2); //extracts first 2 characters from StationCd and assigns it to InputLineCd
            int LineIndex = -1;
            for (int index = 0; index < MRT.Count; index++) //for loop
            {
                if (InputLineCd.Equals(MRT[index].LineCd)) //checks if input line code matches LineCd in MRT
                {
                    LineIndex = index; //sets LineIndex to Index where input line code was found
                    break;
                }
            }
            for (int StationIndex = 0; StationIndex < MRT[LineIndex].getStationList().Count; StationIndex++) //for loop
            {
                if (MRT[LineIndex].getStationList()[StationIndex].IsInterchange) //checks if Station is an Interchange
                {
                    bool IsStation = false; //boolean variable
                    for (int i = 0; i < MRT[LineIndex].getStationList()[StationIndex].StationCode.Count; i++) //for loop
                    {
                        if (MRT[LineIndex].getStationList()[StationIndex].StationCode[i].Equals(StationCd)) //checks if inputted Station Code matches Station Code in MRT Station List
                        {
                            IsStation = true; //sets boolean variable IsStation to true
                        }
                    }

                    if (IsStation) //if (true)
                    {
                        isInterchange = "#" + StationCd + " - " + MRT[LineIndex].getStationList()[StationIndex].StationName + " - " + "Interchange" + "\n";
                        interchangeOutput += isInterchange; //string output
                    }
                    else //else
                    {
                        for (int i = 0; i < MRT[LineIndex].getStationList()[StationIndex].StationCode.Count; i++) //for loop
                        {
                            if (MRT[LineIndex].getStationList()[StationIndex].StationCode[i].StartsWith((InputLineCd))) //checks if Station Code in MRT Station list starts with InputLineCd
                            {
                                interchange = MRT[LineIndex].getStationList()[StationIndex].StationCode[i] + " - " + MRT[LineIndex].getStationList()[StationIndex].StationName + " - " + "Interchange";
                                interchangeOutput += interchange + "\n"; //string output
                                break;
                            }
                        }
                    }
                }
                else //else
                {
                    if (MRT[LineIndex].getStationList()[StationIndex].StationCode[0].Equals(StationCd)) //checks if inputted Station Code matches Station Code in MRT Station List
                    {
                        output = "#" + StationCd + " - " + MRT[LineIndex].getStationList()[StationIndex].StationName;
                        interchangeOutput += output + "\n"; //string output
                    }
                    else //else
                    {
                        output1 = MRT[LineIndex].getStationList()[StationIndex].StationCode[0] + " - " + MRT[LineIndex].getStationList()[StationIndex].StationName;
                        interchangeOutput += output1 + "\n"; //string output
                    }
                }
                finaloutput = interchangeOutput;
            }
            return finaloutput; //returns string
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


       

    }
}
