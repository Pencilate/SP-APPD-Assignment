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
            MRT = FileIO.textFileReader(FilePath);

        }

        public static Station SearchByStationCd(string searchInput)
        {
            string lineCd = searchInput.Substring(0, 2);
            int ResultLineIndex = -1;

            for (int index = 0; index < MRT.Count; index++)
            {
                if (lineCd.Equals(MRT[index].LineCd))
                {
                    ResultLineIndex = index;
                    break;
                }
            }
            Station ResultStation = new Station();
            //foreach (Station sta in MRT[ResultLineIndex].getStationList())
            for(int i = 0; i<MRT[ResultLineIndex].getStationList().Count;i++)
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
            for (int i =0; i < MRT.Count; i++)
            {
                for (int j=0; j < MRT[i].getStationList().Count; j++)
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

        public static void FindRoute(Station station)
        {

        }
        
        public static void DisplayRoute(string StationCd)
        {
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

            for(int StationIndex = 0; StationIndex < MRT[LineIndex].getStationList().Count; StationIndex++)
            {

            }

        }






        public static void TestingStationMtd()
        {
            //for testing
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

            Console.ReadKey();
        }


    }
}
