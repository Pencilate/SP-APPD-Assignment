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

        private static List<int> visitedIndex = new List<int>();
        private static int[,] distanceTable;


        public static void initStationIndex()
        {
            int count = 0;
            foreach (Line line in Guide.MRTLine)
            {
                foreach (Station stat in line.StationList)
                {
                    if (stat.GraphIndex == -1)
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

            foreach (Line line in Guide.MRTLine)
            {
                for (int i = 0; i < (line.StationList.Count - 1); i++)
                {
                    mrtGraph.addEdge(line.StationList[i].GraphIndex, line.StationList[i + 1].GraphIndex, 1);
                    Console.WriteLine("{0}({1}) - {2}({3})", line.StationList[i].StationName, line.StationList[i].GraphIndex, line.StationList[i + 1].StationName, line.StationList[i + 1].GraphIndex);
                }
            }

        }
        public static Station SearchByStationGraphIndex(int inputGraphIndex)
        {
            Station ResultStation = new Station();
            for (int i = 0; i < Guide.MRTLine.Count; i++)
            {
                for (int j = 0; j < Guide.MRTLine[i].StationList.Count; j++)
                {
                    if (inputGraphIndex.Equals(Guide.MRTLine[i].StationList[j].GraphIndex))
                    {
                        ResultStation = Guide.MRTLine[i].getStation(j);
                        break;
                    }
                }
            }
            return ResultStation;
        }


        public static void initTraverseDijkstra(int sourceGraphIndex, int destinationGraphIndex)
        {

            distanceTable = new int[size, 2];//the first column will have the distance of that node from the starting node, the second column is the index of the node that came before it.
            for (int i = 0; i < size; i++)
            {
                distanceTable[i, 0] = int.MaxValue;
                distanceTable[i, 1] = -1;
            }
            distanceTable[sourceGraphIndex, 0] = 0; //Distance from the starting station from the starting station is 0, thus setting it.
            int currentNodeIndex = sourceGraphIndex;

            while (visitedIndex.Count < size)
            {
                currentNodeIndex = TraverseDijkstra(currentNodeIndex);
            }

            for (int i = 0; i < size; i++)
            {
                Console.WriteLine("{0} - Distance:{1} - Comes From:{2}", i, distanceTable[i, 0], distanceTable[i, 1]);
            }

            List<int> routeGraphIndex = new List<int>() {destinationGraphIndex};
            int currentIndex = destinationGraphIndex;
            while(currentIndex != sourceGraphIndex)
            {
                currentIndex = distanceTable[currentIndex, 1];
                routeGraphIndex.Add(currentIndex);
            }

            routeGraphIndex.Reverse();

            List<string> routeStationName = new List<string>();
            foreach(int gI in routeGraphIndex)
            {
                routeStationName.Add(SearchByStationGraphIndex(gI).StationName);
                Console.WriteLine(SearchByStationGraphIndex(gI).StationName);
            }




        }

        public static int TraverseDijkstra(int currentNodeIndex)
        {
            List<int> currentNodeNeighbour = new List<int>();

            for (int i = 0; i < size; i++)
            {
                if (mrtGraph.isEdge(currentNodeIndex, i))
                {
                    currentNodeNeighbour.Add(i);
                }
            }
            foreach (int visitedIdx in visitedIndex)
            {
                currentNodeNeighbour.Remove(visitedIdx);

            }

            for (int i = 0; i < currentNodeNeighbour.Count; i++)
            {
                int sumDistance = distanceTable[currentNodeIndex, 0] + mrtGraph.edgeDistance(currentNodeIndex, currentNodeNeighbour[i]);

                if (distanceTable[currentNodeNeighbour[i], 0] > sumDistance)
                {
                    distanceTable[currentNodeNeighbour[i], 0] = sumDistance;
                    distanceTable[currentNodeNeighbour[i], 1] = currentNodeIndex;
                }

            }

            visitedIndex.Add(currentNodeIndex);

            int nextNode = -1;
            int shortestDist = int.MaxValue;
            for (int i = 0; i < size; i++)
            {
                if (visitedIndex.Contains(i))
                {
                    continue;
                }
                else
                {
                    if (shortestDist > distanceTable[i, 0])
                    {
                        shortestDist = distanceTable[i, 0];
                        nextNode = i;
                    }
                }
            }


            string neighbourStr = "";
            foreach (int idx in currentNodeNeighbour)
            {
                neighbourStr += idx + " ";
            }
            Console.WriteLine(currentNodeIndex + " " + nextNode + " | " + neighbourStr);
            return nextNode;
        }

        public static void TestGraph()
        {
            string stationCd = "";
            Console.Write("Please enter a station code: ");
            stationCd = Console.ReadLine();
            Station tempStat = Guide.SearchByStationCd(stationCd);
            Console.WriteLine("Station Information:\nStation Name: {0}\nGraphIndex: {1}", tempStat.StationName, tempStat.GraphIndex);
            List<int> neighbourIdx = new List<int>();
            for (int i = 0; i < size; i++)
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
                foreach (Line line in Guide.MRTLine)
                {
                    foreach (Station stat in line.StationList)
                    {
                        if (index == stat.GraphIndex)
                        {
                            neighbourName.Add(stat.StationName);
                            goto end_of_loop;//using goto as a subtitute for java's named loop in c#. from https://stackoverflow.com/questions/359436/c-sharp-equivalent-to-javas-continue-label

                        }
                    }
                }
            end_of_loop: { }
            }
            for (int i = 0; i < neighbourIdx.Count; i++)
            {
                Console.WriteLine("{0,5}|{1}", neighbourIdx[i], neighbourName[i]);
            }

        }
    }
}
