using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPDCA1
{
    class GraphRoute
    {
        private static Graph mrtGraph;
        private static int size;

        public static void initStationIndex()
        {
            int count = 0;
            foreach(Line line in Guide.GetMRTLine())
            {
                foreach(Station stat in line.getStationList())
                {
                    if(stat.GraphIndex == -1)
                    {
                        stat.GraphIndex = count;
                        count++;
                    }
                }
            }
            size = count;
        }

        public static void initGraph()
        {
            mrtGraph = new Graph(size);

            foreach (Line line in Guide.GetMRTLine())
            {
                for (int i = 0; i < (line.getStationList().Count-1); i++)
                {
                    mrtGraph.addEdge(line.getStationList()[i].GraphIndex, line.getStationList()[i + 1].GraphIndex, 1);
                    Console.WriteLine("{0}({1}) - {2}({3})", line.getStationList()[i].StationName, line.getStationList()[i].GraphIndex, line.getStationList()[i + 1].StationName, line.getStationList()[i + 1].GraphIndex);
                }
            }
            
        }

        public static void TestGraph()
        {
            string stationCd = "";
            Console.Write("Please enter a station code: ");
            stationCd = Console.ReadLine();
            Station tempStat = Guide.SearchByStationCd(stationCd);
            Console.WriteLine("Station Information:\nStation Name: {0}\nGraphIndex: {1}",tempStat.StationName, tempStat.GraphIndex);
            List<int> neighbourIdx = new List<int>();
            for(int i =0; i<size; i++)
            {
                if (mrtGraph.isEdge(tempStat.GraphIndex, i))
                {
                    neighbourIdx.Add(i);
                    Console.WriteLine(i);
                }
            }
            Console.WriteLine("Neighbouring Stations: \nIndex|StationName\n");
            List<string> neighbourName = new List<string>();
            foreach (int index in neighbourIdx)
            {
                foreach(Line line in Guide.GetMRTLine())
                {
                    foreach(Station stat in line.getStationList())
                    {
                        if(index == stat.GraphIndex)
                        {
                            neighbourName.Add(stat.StationName);
                            goto end_of_loop;//using goto as a subtitute for java's named loop in c#. from https://stackoverflow.com/questions/359436/c-sharp-equivalent-to-javas-continue-label

                        }
                    }
                }
            end_of_loop: { }
            }
            for(int i = 0; i < neighbourIdx.Count; i++)
            {
                Console.WriteLine("{0,5}|{1}",neighbourIdx[i],neighbourName[i]);
            }

        }
    }
}
