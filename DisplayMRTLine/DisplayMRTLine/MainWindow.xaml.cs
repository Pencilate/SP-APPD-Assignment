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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace DisplayMRTLine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Guide.initLineArray();
            string station = txtStat.Text;
            if (radStation.IsChecked == true)
            {
                station = Guide.SearchByStationName(txtStat.Text).StationCode[0];
            }

            txtDisplay.Text = Guide.DisplayRoute(station);


            if (chkName.IsChecked == true)
            {
                txtDisplay.Text = Guide.DisplayRoute(txtMRTLine.Text);
            }
        }

        public class Station
        {
            private string stationName;
            private bool isInterchange = false;

            private List<string> stationCode = new List<string>();

            public Station(string stationCd, string stationName)
            {
                this.stationName = stationName;
                StationCode.Add(stationCd);
            }

            public Station() { }

            public List<string> StationCode
            {
                get { return stationCode; }
                set { this.stationCode = value; }
            }

            public string StationName
            {
                get { return stationName; }
                set { this.stationName = value; }
            }

            public bool IsInterchange
            {
                get { return isInterchange; }
                set { this.isInterchange = value; }
            }
        }

        public class Line
        {
            private string lineCd;
            private List<Station> StationLine = new List<Station>();

            public Line(string lineCd)
            {
                this.lineCd = lineCd;
            }
            public Line()
            {

            }

            public void AddStationToLine(string StationCd, string StationName)
            {
                StationLine.Add(new Station(StationCd, StationName));
            }

            internal Station getStation(int index)
            {
                return StationLine[index];
            }

            internal List<Station> getStationList()
            {
                return StationLine;
            }

            public string LineCd
            {
                get { return lineCd; }
                set { this.lineCd = value; }
            }

        }

        public class FileIO
        {
            public static List<Line> textFileReader(string FilePath)
            {
                List<Line> MRT = new List<Line>();
                string StationName;
                int LineNo = 0;
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string lineData;
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
                                //Compare name of stations between CurruntLine and ComparedLine
                                Console.WriteLine("{0} {1} VS {2} {3}", MRT[CurrentLine].LineCd, MRT[CurrentLine].getStationList()[j].StationName, MRT[i].LineCd, MRT[i].getStationList()[h].StationName);
                                if (MRT[CurrentLine].getStationList()[j].StationName.Equals(MRT[i].getStationList()[h].StationName))
                                {
                                    //if the station names match,                              
                                    MRT[CurrentLine].getStationList()[j].IsInterchange = true;
                                    //set varible to identify it as a interchange as true


                                    Console.WriteLine("YES");
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

                                }//end if

                            }
                        }

                    }

                }

                return MRT;
            }
        }

        public class Guide
        {
            private static List<Line> MRT = new List<Line>();

            public static void initLineArray()
            {
                string FilePath = "..\\..\\resources\\MRT.txt";
                //FilePath Points to the MRT.txt file
                MRT = FileIO.textFileReader(FilePath);

            }

            public static Station SearchByStationCd(string searchInput) //This method scans through the MRT List of Line Objects then the List of Station Objects within it to find the specific station object and return it.
            {
                searchInput = searchInput.ToUpper(); //Makes the input case insensitive
                string lineCd = searchInput.Substring(0, 2);

                int ResultLineIndex = GetIndexOfLine(lineCd);

                Station ResultStation = new Station();
                //foreach (Station sta in MRT[ResultLineIndex].getStationList())
                for (int i = 0; i < MRT[ResultLineIndex].getStationList().Count; i++)
                {
                    bool interchange = MRT[ResultLineIndex].getStationList()[i].IsInterchange;
                    if (MRT[ResultLineIndex].getStationList()[i].IsInterchange)
                    {
                        for (int j = 0; j < MRT[ResultLineIndex].getStationList()[i].StationCode.Count; j++)
                        {
                            if (searchInput.Equals(MRT[ResultLineIndex].getStationList()[i].StationCode[j]))
                            {
                                ResultStation = MRT[ResultLineIndex].getStationList()[i];
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (searchInput.Equals(MRT[ResultLineIndex].getStationList()[i].StationCode[0]))
                        {
                            ResultStation = MRT[ResultLineIndex].getStationList()[i];
                            break;
                        }
                    }

                }
                return ResultStation;
            }

            public static Station SearchByStationName(string searchInput)
            {
                Station ResultStation = new Station();
                for (int i = 0; i < MRT.Count; i++)
                {
                    for (int j = 0; j < MRT[i].getStationList().Count; j++)
                    {
                        if (searchInput.Equals(MRT[i].getStation(j).StationName))
                        {
                            ResultStation = MRT[i].getStation(j);
                            break;
                        }
                    }
                }
                return ResultStation;
            }

            public static int GetIndexOfLine(string lineCd)
            {
                int ResultLineIndex = -1;

                for (int index = 0; index < MRT.Count; index++)
                {
                    if (lineCd.Equals(MRT[index].LineCd))
                    {
                        ResultLineIndex = index;
                        break;
                    }
                }
                return ResultLineIndex;
            }


            public static string DisplayRoute(string StationCd)
            {
                string isInterchange = string.Empty;
                string interchange = string.Empty;
                string output = string.Empty;
                string output1 = string.Empty;
                string interchangeOutput = string.Empty;
                string finaloutput = string.Empty;
                StationCd = StationCd.ToUpper();
                string InputLineCd = StationCd.Substring(0, 2);
                int LineIndex = -1;
                for (int index = 0; index < MRT.Count; index++)
                {
                    if (InputLineCd.Equals(MRT[index].LineCd))
                    {
                        LineIndex = index;
                        break;
                    }
                }
                Console.WriteLine("Listing station for {0} Line", InputLineCd);
                for (int StationIndex = 0; StationIndex < MRT[LineIndex].getStationList().Count; StationIndex++)
                {
                    if (MRT[LineIndex].getStationList()[StationIndex].IsInterchange)
                    {
                        bool IsStation = false;
                        for (int i = 0; i < MRT[LineIndex].getStationList()[StationIndex].StationCode.Count; i++)
                        {
                            if (MRT[LineIndex].getStationList()[StationIndex].StationCode[i].Equals(StationCd))
                            {
                                IsStation = true;
                            }
                        }

                        if (IsStation)
                        {
                            isInterchange = "#" + StationCd + " - " + MRT[LineIndex].getStationList()[StationIndex].StationName + " - " + "Interchange" + "\n";
                            interchangeOutput += isInterchange;
                        }
                        else
                        {
                            for (int i = 0; i < MRT[LineIndex].getStationList()[StationIndex].StationCode.Count; i++)
                            {
                                if (MRT[LineIndex].getStationList()[StationIndex].StationCode[i].StartsWith((InputLineCd)))
                                {
                                    interchange = MRT[LineIndex].getStationList()[StationIndex].StationCode[i] + " - " + MRT[LineIndex].getStationList()[StationIndex].StationName + " - " + "Interchange";
                                    interchangeOutput += interchange + "\n";
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (MRT[LineIndex].getStationList()[StationIndex].StationCode[0].Equals(StationCd))
                        {
                            output = "#" + StationCd + " - " + MRT[LineIndex].getStationList()[StationIndex].StationName;
                            interchangeOutput += output + "\n";
                        }
                        else
                        {
                            output1 = MRT[LineIndex].getStationList()[StationIndex].StationCode[0] + " - " + MRT[LineIndex].getStationList()[StationIndex].StationName;
                            interchangeOutput += output1 + "\n";
                        }
                    }
                    finaloutput = interchangeOutput;
                }
                return finaloutput;
            }



            public static string DisplayFindPath(int lineIndex, int ssIndex, int esIndex)
            {
                string lineCd = MRT[lineIndex].LineCd;
                string output1 = string.Empty;
                string output2 = string.Empty;
                if (esIndex > ssIndex)
                {
                    for (int i = ssIndex; i <= esIndex; i++)
                    {
                        string extractedStatCd = "";
                        if (MRT[lineIndex].getStationList()[i].IsInterchange)
                        {
                            foreach (string statCd in MRT[lineIndex].getStationList()[i].StationCode)
                            {
                                if (statCd.Contains(lineCd))
                                {
                                    extractedStatCd = statCd;

                                }
                            }
                        }
                        else
                        {
                            extractedStatCd = MRT[lineIndex].getStationList()[i].StationCode[0];
                        }
                        Console.WriteLine("{0} - {1}", extractedStatCd, MRT[lineIndex].getStationList()[i].StationName);
                        Console.WriteLine("|");
                        output1 += "\n" + extractedStatCd + " - " + MRT[lineIndex].getStationList()[i].StationName;
                    }
                    return output1;

                }
                else if (ssIndex > esIndex)
                {
                    for (int i = ssIndex; i >= esIndex; i--)
                    {
                        string extractedStatCd = "";
                        if (MRT[lineIndex].getStationList()[i].IsInterchange)
                        {
                            foreach (string statCd in MRT[lineIndex].getStationList()[i].StationCode)
                            {
                                if (statCd.Contains(lineCd))
                                {
                                    extractedStatCd = statCd;

                                }
                            }

                        }
                        else
                        {
                            extractedStatCd = MRT[lineIndex].getStationList()[i].StationCode[0];
                        }
                        Console.WriteLine("{0} - {1}", extractedStatCd, MRT[lineIndex].getStationList()[i].StationName);
                        Console.WriteLine("|");
                        output2 += "\n" + extractedStatCd + " - " + MRT[lineIndex].getStationList()[i].StationName;
                    }
                    return output2;
                }
                else
                {
                    return "Same station for Start and End";
                }
            }



            public static string FindPathV2(string StartingStatCd, string EndingStatCd)
            {
                Station StartStat = SearchByStationCd(StartingStatCd);
                Station EndStat = SearchByStationCd(EndingStatCd);

                List<string[]> statCdPair = new List<string[]>();

                for (int i = 0; i < StartStat.StationCode.Count; i++)
                {
                    for (int j = 0; j < EndStat.StationCode.Count; j++)
                    {

                        string ssLineCd = StartStat.StationCode[i].Substring(0, 2);
                        string esLineCd = EndStat.StationCode[j].Substring(0, 2);
                        string[] cdPair = { ssLineCd, esLineCd };
                        statCdPair.Add(cdPair);


                    }
                }

                bool directRoute = false;
                bool routeAvailable = false;
                string interchangeName = "";
                int selectedStatCdPairIndex = -1;
                for (int i = 0; i < statCdPair.Count; i++)
                {
                    if ((statCdPair[i])[0].Equals((statCdPair[i])[1]))
                    {
                        directRoute = true;
                        routeAvailable = true;
                        selectedStatCdPairIndex = i;
                    }
                }

                if (!directRoute)
                {

                    for (int i = 0; i < statCdPair.Count; i++)
                    {
                        int lineIndex = GetIndexOfLine((statCdPair[i])[0]);

                        foreach (Station stat in MRT[lineIndex].getStationList())
                        {
                            if (stat.IsInterchange)
                            {
                                List<string> lineOffered = new List<string>();
                                foreach (string statCd in stat.StationCode)
                                {
                                    lineOffered.Add(statCd.Substring(0, 2));
                                }

                                if ((lineOffered.Contains(statCdPair[i][0])) && (lineOffered.Contains(statCdPair[i][1])))
                                {
                                    routeAvailable = true;
                                    selectedStatCdPairIndex = i;
                                    interchangeName = stat.StationName;
                                    break;
                                }


                            }
                        }


                    }
                }

                if (routeAvailable)
                {
                    if (directRoute)
                    {
                        int lineIndex = GetIndexOfLine(statCdPair[selectedStatCdPairIndex][0]);

                        int ssIndex = GetStationIndexFromLine(lineIndex, StartStat.StationName);
                        int esIndex = GetStationIndexFromLine(lineIndex, EndStat.StationName);
                        return DisplayFindPath(lineIndex, ssIndex, esIndex);
                    }
                    else
                    {
                        int lineIndex = GetIndexOfLine(statCdPair[selectedStatCdPairIndex][0]);

                        int ssIndex = GetStationIndexFromLine(lineIndex, StartStat.StationName);
                        int icsArrvIndex = GetStationIndexFromLine(lineIndex, interchangeName);
                        Console.WriteLine("Start");
                        DisplayFindPath(lineIndex, ssIndex, icsArrvIndex);

                        int lineIndex2 = GetIndexOfLine(statCdPair[selectedStatCdPairIndex][1]);

                        int icsDeprtIndex = GetStationIndexFromLine(lineIndex2, interchangeName);
                        int esIndex = GetStationIndexFromLine(lineIndex2, EndStat.StationName);

                        return DisplayFindPath(lineIndex, ssIndex, icsArrvIndex) + DisplayFindPath(lineIndex2, icsDeprtIndex, esIndex);
                    }
                }
                else
                {
                    return "No route available";
                }
            }


            public static int GetStationIndexFromLine(int lineIndex, string StationName)
            {
                int index = -1;
                for (int i = 0; i < MRT[lineIndex].getStationList().Count; i++)
                {
                    if (MRT[lineIndex].getStationList()[i].StationName.Equals(StationName))
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }

        }
    }
}

