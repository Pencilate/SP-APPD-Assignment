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

        public static void FindRoute(string LineCd, string End)
        {
            string InputLineCd = LineCd.Substring(0, 2);
            string EndCd = End.Substring(0, 2);

            int LineIndex = -1;
            for (int index = 0; index < MRT.Count; index++)
            {
                if (InputLineCd.Equals(MRT[index].LineCd))
                {
                    LineIndex = index;
                    break;
                }
            }

            string changeStationCode = string.Empty;
            string changeStationName = string.Empty;
            if (InputLineCd.Equals(EndCd))
                Console.WriteLine("Take the {0} line directly.", InputLineCd);
            else
            {
                for (int StationIndex = 0; StationIndex < MRT[LineIndex].getStationList().Count; StationIndex++)
                {
                    string output1 = MRT[LineIndex].getStationList()[StationIndex].StationCode[0];
                    if (output1.Contains(EndCd))
                    {
                        changeStationCode = output1;
                        Console.WriteLine("{2} {0} - {1}", changeStationCode, SearchByStationCd(output1).StationName, "You should change at:");
                    }
                }
            }
        }

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
            for(int StationIndex = 0; StationIndex < MRT[LineIndex].getStationList().Count; StationIndex++)
            {
                if (MRT[LineIndex].getStationList()[StationIndex].IsInterchange)
                {
                    bool IsStation = false ;
                    for(int i =0; i< MRT[LineIndex].getStationList()[StationIndex].StationCode.Count;i++)
                        {
                        if (MRT[LineIndex].getStationList()[StationIndex].StationCode[i].Equals(StationCd))
                        {
                            IsStation = true;
                        }
                        }

                    if (IsStation)
                    {
                        Console.WriteLine("#{0} - {1} - {2}", StationCd, MRT[LineIndex].getStationList()[StationIndex].StationName, "Interchange");
                    }
                    else
                    {
                        for (int i = 0; i < MRT[LineIndex].getStationList()[StationIndex].StationCode.Count; i++)
                        {
                           if (MRT[LineIndex].getStationList()[StationIndex].StationCode[i].StartsWith((InputLineCd))){
                                Console.WriteLine("{0} - {1} - {2}", MRT[LineIndex].getStationList()[StationIndex].StationCode[i], MRT[LineIndex].getStationList()[StationIndex].StationName, "Interchange");
                                break;
                            }
                        }
                    }



                }
                else
                {
                    if (MRT[LineIndex].getStationList()[StationIndex].StationCode[0].Equals(StationCd))
                    {
                        Console.WriteLine("#{0} - {1}", StationCd, MRT[LineIndex].getStationList()[StationIndex].StationName);
                    }
                    else
                    {
                        Console.WriteLine("{0} - {1}", MRT[LineIndex].getStationList()[StationIndex].StationCode[0], MRT[LineIndex].getStationList()[StationIndex].StationName);
                    }
                }
            }

        }


        public static void DisplayFindPath(string StartingStat, string EndingStat)
        {

            Station StartStat = SearchByStationName(StartingStat);
            Station EndStat = SearchByStationName(EndingStat);

            bool Sameline = false;
            string SamelineCd = "";
            foreach (string ssCd in StartStat.StationCode)
            {
                string ssLindCd = ssCd.Substring(0, 2);

                foreach (string esCd in EndStat.StationCode)
                {
                    string esLineCd = esCd.Substring(0, 2);

                    if (ssLindCd.Equals(esLineCd))
                    {
                        Sameline = true;
                        SamelineCd = esLineCd;
                    }

                }


            }

            if (Sameline)
            {
                int LineIndex = -1;
                for(int i =0; i< MRT.Count; i++)
                {
                    if (MRT[i].LineCd.Equals(SamelineCd))
                    {
                        LineIndex = i;
                    }
                }

                int ssIndex;

            }
            else
            {

            }

            




        }
        







        public static void TestingStationMtd()
        {
            //for testing

            Console.WriteLine("\r\n ------Testing------ \r\n");

            for (int i = 0; i < MRT[4].getStationList().Count; i++)
            {
                Console.WriteLine(MRT[4].getStationList()[i].StationName);
                foreach (string str in MRT[4].getStationList()[i].StationCode)
                {
                    Console.Write("{0,5},", str);
                }
                Console.WriteLine();
            }
            Console.WriteLine(MRT[0].getStationList()[0].StationName);
            foreach (string str in MRT[0].getStationList()[0].StationCode)
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

            foreach (Station stat in MRT[4].getStationList())
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
;


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
            s2.StationCode = SearchByStationName(c).StationCode;
            foreach (char b in Guide.SearchByStationCd(c).StationName)
            {
                Console.Write(b.ToString());
            }

            Console.WriteLine();
            Console.WriteLine();
            FindRoute("CC8","EW22");
            //Console.WriteLine();
            //FindRoute("NS1", "EW1"); //Find route cant do this yet
            Console.ReadKey();
            
        }




    }
}
