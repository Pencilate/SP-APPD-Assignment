using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPDCA1
{
    public class Guide
    {
        private static List<Line> MRT = new List<Line>();

        public static List<Line> MRTLine
        {
            get { return MRT; }
        }

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
            //foreach (Station sta in MRT[ResultLineIndex].StationList)
            for (int i = 0; i < MRT[ResultLineIndex].StationList.Count; i++)
            {
                bool interchange = MRT[ResultLineIndex].StationList[i].IsInterchange;
                if (MRT[ResultLineIndex].StationList[i].IsInterchange)
                {
                    for (int j = 0; j < MRT[ResultLineIndex].StationList[i].StationCode.Count; j++)
                    {
                        if (searchInput.Equals(MRT[ResultLineIndex].StationList[i].StationCode[j]))
                        {
                            ResultStation = MRT[ResultLineIndex].StationList[i];
                            break;
                        }
                    }
                }
                else
                {
                    if (searchInput.Equals(MRT[ResultLineIndex].StationList[i].StationCode[0]))
                    {
                        ResultStation = MRT[ResultLineIndex].StationList[i];
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
                for (int j = 0; j < MRT[i].StationList.Count; j++)
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

        //public static void FindRoute(Station startStation , Station endStation)
        //{
        //    string LineCd = startStation.StationCode[0];
        //    string End = endStation.StationCode[0];
        //    string InputLineCd = LineCd.Substring(0, 2);
        //    string EndCd = End.Substring(0, 2);

        //    int LineIndex = -1;
        //    for (int index = 0; index < MRT.Count; index++)
        //    {
        //        if (InputLineCd.Equals(MRT[index].LineCd))
        //        {
        //            LineIndex = index;
        //            break;
        //        }
        //    }

        //    string changeStationCode = string.Empty;
        //    string changeStationName = string.Empty;
        //    bool IntStation = false;
        //    if (InputLineCd.Equals(EndCd))
        //        Console.WriteLine("Take the {0} line directly.", InputLineCd);
        //    else
        //    {
        //        for (int StationIndex = 0; StationIndex < MRT[LineIndex].StationList.Count; StationIndex++)
        //        {
        //            string output1 = MRT[LineIndex].StationList[StationIndex].StationCode[0];
        //            if (output1.Contains(EndCd))
        //            {
        //                changeStationCode = output1;
        //                Console.WriteLine("{2} {0} - {1}", changeStationCode, SearchByStationCd(output1).StationName, "You should change at:");
        //            }

        //            else if (!output1.Contains(EndCd))
        //            {
        //                for (int StatIndex = 0; StatIndex < MRT[LineIndex].StationList.Count; StatIndex++)
        //                {
        //                    if (MRT[LineIndex].StationList[StationIndex].IsInterchange)
        //                    {
        //                        for (int i = 0; i < MRT[LineIndex].StationList[StationIndex].StationCode.Count; i++)
        //                        {
        //                            if (MRT[LineIndex].StationList[StationIndex].StationCode[i].Equals(LineCd))
        //                            {
        //                                IntStation = true;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //                Console.WriteLine("Change to the {0} line at {1} and take it directly.",EndCd,SearchByStationCd(LineCd).StationName);
        //                break;
        //            }
        //            else
        //            {
        //                Console.WriteLine(IntStation);
        //                Console.WriteLine("No route found.");
        //            }
        //        }
        //    }
        //}

        public static void DisplayRoute(string StationCd)
        {
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
            for(int StationIndex = 0; StationIndex < MRT[LineIndex].StationList.Count; StationIndex++)
            {
                if (MRT[LineIndex].StationList[StationIndex].IsInterchange)
                {
                    bool IsStation = false ;
                    for(int i =0; i< MRT[LineIndex].StationList[StationIndex].StationCode.Count;i++)
                        {
                        if (MRT[LineIndex].StationList[StationIndex].StationCode[i].Equals(StationCd))
                        {
                            IsStation = true;
                        }
                        }

                    if (IsStation)
                    {
                        Console.WriteLine("#{0} - {1} - {2}", StationCd, MRT[LineIndex].StationList[StationIndex].StationName, "Interchange");
                    }
                    else
                    {
                        for (int i = 0; i < MRT[LineIndex].StationList[StationIndex].StationCode.Count; i++)
                        {
                           if (MRT[LineIndex].StationList[StationIndex].StationCode[i].StartsWith((InputLineCd))){
                                Console.WriteLine("{0} - {1} - {2}", MRT[LineIndex].StationList[StationIndex].StationCode[i], MRT[LineIndex].StationList[StationIndex].StationName, "Interchange");
                                break;
                            }
                        }
                    }



                }
                else
                {
                    if (MRT[LineIndex].StationList[StationIndex].StationCode[0].Equals(StationCd))
                    {
                        Console.WriteLine("#{0} - {1}", StationCd, MRT[LineIndex].StationList[StationIndex].StationName);
                    }
                    else
                    {
                        Console.WriteLine("{0} - {1}", MRT[LineIndex].StationList[StationIndex].StationCode[0], MRT[LineIndex].StationList[StationIndex].StationName);
                    }
                }
            }

        }

        public static void DisplayFindPath(int lineIndex, int ssIndex, int esIndex)
        {
            string lineCd = MRT[lineIndex].LineCd;

            if (esIndex > ssIndex)
            {
                for (int i = ssIndex; i <= esIndex; i++)
                {
                    string extractedStatCd = "";
                    if (MRT[lineIndex].StationList[i].IsInterchange)
                    {
                        foreach (string statCd in MRT[lineIndex].StationList[i].StationCode)
                        {
                            if (statCd.Contains(lineCd))
                            {
                                extractedStatCd = statCd;
                            }
                        }
                       
                    }
                    else
                    {
                        extractedStatCd = MRT[lineIndex].StationList[i].StationCode[0];
                    }
                    Console.WriteLine("{0} - {1}", extractedStatCd, MRT[lineIndex].StationList[i].StationName);
                    Console.WriteLine("|");
                }
            }
            else if (ssIndex > esIndex)
            {
                for (int i = ssIndex; i >= esIndex; i--)
                {
                    string extractedStatCd = "";
                    if (MRT[lineIndex].StationList[i].IsInterchange)
                    {
                        foreach (string statCd in MRT[lineIndex].StationList[i].StationCode)
                        {
                            if (statCd.Contains(lineCd))
                            {
                                extractedStatCd = statCd;
                            }
                        }

                    }
                    else
                    {
                        extractedStatCd = MRT[lineIndex].StationList[i].StationCode[0];
                    }
                    Console.WriteLine("{0} - {1}", extractedStatCd, MRT[lineIndex].StationList[i].StationName);
                    Console.WriteLine("|");
                }
            }
            else
            {
                Console.WriteLine("Same station for Start and End");
            }
        }

        //public static void FindPath(string StartingStatCd, string EndingStatCd)
        //{

        //    Station StartStat = SearchByStationCd(StartingStatCd);
        //    Station EndStat = SearchByStationCd(EndingStatCd);

        //    bool sameline = false;
        //    string SamelineCd = "";
        //    foreach (string ssCd in StartStat.StationCode)
        //    {
        //        string ssLindCd = ssCd.Substring(0, 2);

        //        foreach (string esCd in EndStat.StationCode)
        //        {
        //            string esLineCd = esCd.Substring(0, 2);

        //            if (ssLindCd.Equals(esLineCd))
        //            {
        //                sameline = true;
        //                SamelineCd = esLineCd;
        //            }

        //        }
        //    }
        //    if (sameline == true)
        //    {
        //        int lineIndex = GetIndexOfLine(SamelineCd);

        //        int ssIndex = -1;
        //        for (int i=0; i<MRT[lineIndex].StationList.Count;i++)
        //        {
        //            if(MRT[lineIndex].StationList[i].StationName.Equals(StartStat.StationName)){
        //                ssIndex = i;
        //                break;
        //            }
        //        }
        //        int esIndex = -1;
        //        for (int i=0; i<MRT[lineIndex].StationList.Count;i++)
        //        {
        //            if(MRT[lineIndex].StationList[i].StationName.Equals(EndStat.StationName)){
        //                esIndex = i;
        //                break;
        //            }
        //        }

        //        DisplayFindPath(lineIndex, ssIndex, esIndex);

        //    }
        //    else
        //    {
        //        Console.WriteLine("Different Line");
        //    }

        //}

        public static void FindPathV2(string StartingStatCd, string EndingStatCd)
        {
            Station StartStat = SearchByStationCd(StartingStatCd);
            Station EndStat = SearchByStationCd(EndingStatCd);

            List<string[]> statCdPair = new List<string[]>();

            for(int i =0; i < StartStat.StationCode.Count; i++)
            {
                for(int j = 0; j < EndStat.StationCode.Count; j++)
                {

                    string ssLineCd = StartStat.StationCode[i].Substring(0, 2);
                    string esLineCd = EndStat.StationCode[j].Substring(0, 2);
                    string[] cdPair = {ssLineCd, esLineCd};
                    statCdPair.Add(cdPair);


                }
            }

            bool directRoute = false;
            bool routeAvailable = false;
            string interchangeName = "";
            int selectedStatCdPairIndex = -1;
            for(int i = 0; i < statCdPair.Count; i++)
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

                //int lineIndex = GetIndexOfLine(statCdPair[-1][0]);

                //foreach (Station stat in MRT[lineIndex].StationList)
                //{
                //    if (stat.IsInterchange)
                //    {
                //        //routeAvailable = true;
                //        List<string> lineOffered = new List<string>();
                //        foreach (string statCd in stat.StationCode)
                //        {
                //            lineOffered.Add(statCd.Substring(0, 2));
                //        }

                //        for(int i = 0; i < statCdPair.Count; i++)
                //        {
                //            if (lineOffered.Contains((statCdPair[i])[1]))
                //            {
                //                if (lineOffered.Contains((statCdPair[i])[2]))
                //                {
                //                    routeAvailable = true;
                //                    selectedStatCdPairIndex = i;
                //                }
                //            }

                //        }

                //        if (routeAvailable)
                //        {
                //            interchangeName = stat.StationName;
                //            break;
                //        }

                //    }
                //}

                for (int i = 0; i < statCdPair.Count; i++)
                {
                    int lineIndex = GetIndexOfLine((statCdPair[i])[0]);

                    foreach (Station stat in MRT[lineIndex].StationList)
                    {
                        if (stat.IsInterchange)
                        {
                            List<string> lineOffered = new List<string>();
                            foreach (string statCd in stat.StationCode)
                            {
                                lineOffered.Add(statCd.Substring(0, 2));
                            }

                            if((lineOffered.Contains(statCdPair[i][0]))&& (lineOffered.Contains(statCdPair[i][1])))
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
                    Console.WriteLine("Start");
                    DisplayFindPath(lineIndex, ssIndex, esIndex);
                    Console.WriteLine("End");
                }
                else
                {
                    int lineIndex = GetIndexOfLine(statCdPair[selectedStatCdPairIndex][0]);

                    int ssIndex = GetStationIndexFromLine(lineIndex, StartStat.StationName);
                    int icsArrvIndex = GetStationIndexFromLine(lineIndex, interchangeName);
                    Console.WriteLine("Start");
                    DisplayFindPath(lineIndex, ssIndex, icsArrvIndex);

                    lineIndex = GetIndexOfLine(statCdPair[selectedStatCdPairIndex][1]);

                    int icsDeprtIndex = GetStationIndexFromLine(lineIndex, interchangeName);
                    int esIndex = GetStationIndexFromLine(lineIndex, EndStat.StationName);

                    DisplayFindPath(lineIndex, icsDeprtIndex, esIndex);
                    Console.WriteLine("End");
                }
            }
            else
            {
                Console.WriteLine("No route available");
            }



        }


        public static int GetStationIndexFromLine(int lineIndex, string StationName)
        {
            int index = -1;
            for (int i = 0; i < MRT[lineIndex].StationList.Count; i++)
            {
                if (MRT[lineIndex].StationList[i].StationName.Equals(StationName))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }



        public static void TestingStationMtd()
        {
            //for testing

            Console.WriteLine("\r\n ------Testing------ \r\n");

            for (int i = 0; i < MRT[4].StationList.Count; i++)
            {
                Console.WriteLine(MRT[4].StationList[i].StationName);
                foreach (string str in MRT[4].StationList[i].StationCode)
                {
                    Console.Write("{0,5},", str);
                }
                Console.WriteLine();
            }
            Console.WriteLine(MRT[0].StationList[0].StationName);
            foreach (string str in MRT[0].StationList[0].StationCode)
            {
                Console.Write("{0,5},", str);
            }
            Console.WriteLine();

            Console.WriteLine("--------");
            Console.WriteLine(Guide.SearchByStationCd("EW4").StationName);
            Console.WriteLine("--------");
            Console.WriteLine(Guide.SearchByStationCd("EW8").StationName);
            Console.WriteLine("--------");
            Console.WriteLine(Guide.SearchByStationCd("CC1").StationName);
            Console.WriteLine("--------");

            string output ="";
            foreach (string str in Guide.SearchByStationName("Jurong East").StationCode)
            {
                output += str+" ";
            }
            Console.WriteLine(output);

            output = "";
            foreach (string str in Guide.SearchByStationName("Tanah Merah").StationCode)
            {
                output += str + " ";
            }
            Console.WriteLine(output);

            foreach (Station stat in MRT[4].StationList)
            {
                Console.WriteLine(stat.StationName);
                foreach (string str in stat.StationCode)
                {
                    Console.WriteLine(str);
                }
            }


            Console.WriteLine("DisplayingRoute");

            Station testStat = SearchByStationName("Dhoby Ghaut");
            foreach (string StationCodeStr in testStat.StationCode)
            {
                DisplayRoute(StationCodeStr);
            }

            Console.WriteLine("Display path WIP");
            FindPathV2("NS1","EW1");
            DisplayRoute("EW2");
            Console.ReadKey();
        }

        public static void Testing()
        {
            Console.Write("Enter Station Name : ");
            string stationname = Console.ReadLine();
            Station s = new Station();
            foreach (string a in Guide.SearchByStationName(stationname).StationCode)
            {
                Console.WriteLine(a);
                s.StationCode = SearchByStationName(a).StationCode;
            }

            Station s2 = new Station();
            Console.Write("Enter Station Code : ");
            string c = Console.ReadLine();
            foreach (char b in Guide.SearchByStationCd(c).StationName)
            {
                Console.Write(b.ToString());
            }

            Station s3 = new Station("CC9", "Jurong East");
            Station s4 = new Station("NE12","Pasir Ris");
            Console.WriteLine();
            Console.WriteLine();
            //FindRoute(s3, s4);
            //Console.WriteLine();
            //FindRoute("NS1", "EW1"); // Find route cant do this yet
            Console.ReadKey();
            
        }

        public static void TestStationRoute()
        {
            Console.Clear();
            Console.WriteLine("Option 1: Find Line of Station");
            Console.WriteLine("Option 2: Find Route Path ");
            Console.Write("Please enter your selection: ");
            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
                    Console.WriteLine("Option 1 Selected:");
                    Console.Write("Enter the station code:" );
                    string input = Console.ReadLine();
                    Station stat = SearchByStationCd(input);
                    foreach (string statCd in stat.StationCode)
                    {
                        DisplayRoute(statCd);
                        Console.WriteLine();
                    }
                    break;
                case 2:
                    Console.WriteLine("Option 2 Selected:");
                    Console.Write("Enter the starting station code:");
                    string startStatCd = Console.ReadLine();
                    Console.Write("Enter the ending station code:");
                    string endStatCd = Console.ReadLine();
                    FindPathV2(startStatCd, endStatCd);
                    break;
            }




        }


    }
}
