using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class GraphRoute
    {
        private static Graph mrtGraph;
        private static int size;


        public static void initStationIndex() //initializes the station indexes
        {
            int count = 0; //counter
            foreach (Line line in Guide.MRTLine) //outer foreach loop
            {
                foreach (Station stat in line.StationList) //inner foreach loop
                {
                    if (stat.GraphIndex == -1)
                    {
                        stat.GraphIndex = count;
                        count++;
                    }
                }
            }
            size = count; //assigns value of count to size variable
        }

        public static void initGraph(char mode) //initializes the Graph
        {
            mrtGraph = new Graph(size); //creates the Graph with size found in initStationIndex()

            foreach (Line line in Guide.MRTLine) //foreach loop
            {
                for (int i = 0; i < (line.StationList.Count - 1); i++) //for loop
                {
                    //mrtGraph.addEdge(line.StationList[i].GraphIndex, line.StationList[i + 1].GraphIndex, 1);
                    double weight = 0-1;
                    switch (mode){
                        case 'F':
                            weight = double.Parse(DBGuide.QueryFareFromDatabase(line.StationList[i].StationCode[0],line.StationList[i+1].StationCode[0])[0]);
                            break;
                        case 'T':
                            weight = double.Parse(DBGuide.QueryFareFromDatabase(line.StationList[i].StationCode[0], line.StationList[i + 1].StationCode[0])[2]);
                            break;
                    }
                    mrtGraph.addEdge(line.StationList[i].GraphIndex, line.StationList[i + 1].GraphIndex, weight);

                    Console.WriteLine("{0}({1}) - {2}({3})", line.StationList[i].StationName, line.StationList[i].GraphIndex, line.StationList[i + 1].StationName, line.StationList[i + 1].GraphIndex);

                }
            }

        }

        public static Station SearchByStationGraphIndex(int inputGraphIndex) //search for a station by the graph index
        {
            Station ResultStation = new Station(); //creates a new Station object
            for (int i = 0; i < Guide.MRTLine.Count; i++) //nested for loop
            {
                for (int j = 0; j < Guide.MRTLine[i].StationList.Count; j++) //inner for loop
                {
                    if (inputGraphIndex.Equals(Guide.MRTLine[i].StationList[j].GraphIndex)) //if a match for the input graph index is found
                    {
                        ResultStation = Guide.MRTLine[i].StationList[j];
                        break;
                    }
                }
            }
            return ResultStation; //return station object
        }

        public static string initTraverseDijkstra(int sourceGraphIndex, int destinationGraphIndex) //find route.  Dijikstra algorithm referenced from here: https://www.codingame.com/playgrounds/1608/shortest-paths-with-dijkstras-algorithm/dijkstras-algorithm
        {
            double[,] distanceTable = new double[size, 2];//the first column will have the distance of that node from the starting node, the second column is the index of the node that came before it.
            List<int> visitedIndex = new List<int>(); //new list

            for (int i = 0; i < size; i++) //for loop
            {
                distanceTable[i, 0] = int.MaxValue;
                distanceTable[i, 1] = -1;
            }
            distanceTable[sourceGraphIndex, 0] = 0; //Distance from the starting station from the starting station is 0, thus setting it.
            int currentNodeIndex = sourceGraphIndex; //sets value of current node index to value of the source graph index

            while (visitedIndex.Count < size) //while loop
            {
                currentNodeIndex = TraverseDijkstra(distanceTable, visitedIndex, currentNodeIndex); //invokes TraverseDijkstra method
            }

            for (int i = 0; i < size; i++) //for loop
            {
                Console.WriteLine("{0} - Distance:{1} - Comes From:{2}", i, distanceTable[i, 0], distanceTable[i, 1]);//for debug to list the entire distance table value
            }

            List<int> routeGraphIndex = new List<int>() { destinationGraphIndex }; //new list
            int currentIndex = destinationGraphIndex;
            while (currentIndex != sourceGraphIndex) //while loop
            {
                currentIndex = (int) distanceTable[currentIndex, 1];
                routeGraphIndex.Add(currentIndex); //add current index to routeGraphIndex list
            }

            routeGraphIndex.Reverse(); //reverses the routeGraphIndex list

            List<Station> routeStation = new List<Station>(); //new list
            foreach (int gI in routeGraphIndex) //foreach loop
            {
                routeStation.Add(SearchByStationGraphIndex(gI)); //add station objects to the routeStation list
            }

            List<string> routeStationCd = new List<string>(); //new list
            for (int idx = 0; idx < routeStation.Count; idx++) //for loop
            {
                string prevStatLineCd = "";
                string nextStatLineCd = "";
                if ((idx == 0) && (routeStation[idx].IsInterchange)) //if index = 0 and the station is an interchange
                {
                    List<string> stationLineCd = new List<string>(); //new list
                    foreach (string statCd in routeStation[idx].StationCode) //foreach loop
                    {
                        stationLineCd.Add(statCd.Substring(0, 2)); //add station code to the stationLineCd list
                    }
                    foreach (string statCd in routeStation[idx + 1].StationCode) //foreach loop
                    {
                        if (stationLineCd.Contains(statCd.Substring(0, 2))) //if stationLineCd list contains station code of next station in routeStation list
                        {
                            nextStatLineCd = statCd.Substring(0, 2); //set nextStatLineCd to be equals to the station code
                        }
                    }

                    foreach (string statCd in routeStation[idx].StationCode) //foreach loop
                    {
                        if (statCd.Contains(nextStatLineCd)) //if routeStation list contains nextStatLineCd station code
                        {
                            routeStationCd.Add(statCd); //add the station code found to the routeStationCd list
                        }
                    }

                }
                else if ((idx == routeStation.Count - 1) && (routeStation[idx].IsInterchange)) //else if 
                {
                    List<string> stationLineCd = new List<string>(); //new list
                    foreach (string statCd in routeStation[idx].StationCode) //foreach loop
                    {
                        stationLineCd.Add(statCd.Substring(0, 2)); //add station code to the stationLineCd list
                    }
                    foreach (string statCd in routeStation[idx - 1].StationCode) //foreach loop
                    {
                        if (stationLineCd.Contains(statCd.Substring(0, 2))) //if stationLineCd list contains station code of previous station in routeStation list
                        {
                            prevStatLineCd = statCd.Substring(0, 2); //set prevStatLineCd to be equals to the station code
                        }
                    }

                    foreach (string statCd in routeStation[idx].StationCode) //foreach loop
                    {
                        if (statCd.Contains(prevStatLineCd)) //if routeStation List contains prevStatLineCd station code
                        {
                            routeStationCd.Add(statCd); //add station code to the routeStationCd list
                        }
                    }
                }
                else if (routeStation[idx].IsInterchange) //else if station in RouteStation list is an interchange
                {

                    List<string> stationLineCd = new List<string>(); //new list
                    foreach (string statCd in routeStation[idx].StationCode) //foreach loop
                    {
                        stationLineCd.Add(statCd.Substring(0, 2)); //add stationcode to stationLineCd list
                    }

                    foreach (string statCd in routeStation[idx - 1].StationCode) //foreach loop
                    {
                        if (stationLineCd.Contains(statCd.Substring(0, 2))) //if stationLineCd list contains station code of previous station in routeStation list
                        {
                            prevStatLineCd = statCd.Substring(0, 2); //set prevStatLineCd to be equal to the station code
                        }
                    }

                    foreach (string statCd in routeStation[idx + 1].StationCode) //foreach loop
                    {
                        if (stationLineCd.Contains(statCd.Substring(0, 2))) //if stationLineCd list contains station code of next station in routeStation list
                        {
                            nextStatLineCd = statCd.Substring(0, 2); //set nextStatLineCd to be equal to the station code
                        }
                    }


                    foreach (string statCd in routeStation[idx].StationCode) //foreach loop
                    {
                        if (statCd.Contains(prevStatLineCd)) //if routeStation list contains prevStatLineCd
                        {
                            routeStationCd.Add(statCd); //add station code to the routeStationCd list
                        }
                    }
                    if (!prevStatLineCd.Equals(nextStatLineCd)) //if prevStatLineCd is not equal to nextStatLineCd
                    {
                        foreach (string statCd in routeStation[idx].StationCode) //foreach loop
                        {
                            if (statCd.Contains(nextStatLineCd)) //if routeStation list contains nextStatLineCd
                            {
                                routeStationCd.Add(statCd); //add station code to the routeStationCd list
                            }
                        }
                    }
                }
                else //else
                {
                    routeStationCd.Add(routeStation[idx].StationCode[0]); //add Station Code from routeStation list to routeStationCd list
                }
            }

            string outputRoute = string.Format("Display Route from {0} to {1} - Taking {2} stations\r\n", routeStation[0].StationName, routeStation[routeStation.Count - 1].StationName, routeStation.Count); //output string
            outputRoute += "-- Start of Route --\r\n";
            foreach (string statCd in routeStationCd) //foreach loop
            {
                outputRoute += string.Format("{0} - {1}\r\n", statCd, Guide.SearchByStationCd(statCd).StationName);
            }
            outputRoute += "-- End of Route --";

            return outputRoute;
        }

        public static int TraverseDijkstra(double[,] distanceTable, List<int> visitedIndex, int currentNodeIndex)
        {
            List<int> currentNodeNeighbour = new List<int>(); //new list

            for (int i = 0; i < size; i++) //for loop
            {
                if (mrtGraph.isEdge(currentNodeIndex, i)) //if Graph has an edge of (currentNodeIndex,i)
                {
                    currentNodeNeighbour.Add(i); //add i to currentNodeNeighbour list
                }
            }
            foreach (int visitedIdx in visitedIndex) //foreach loop
            {
                currentNodeNeighbour.Remove(visitedIdx); //remove visitedIdx from currentNodeNeighbour list
            }

            for (int i = 0; i < currentNodeNeighbour.Count; i++) //for loop
            {
                double sumDistance = distanceTable[currentNodeIndex, 0] + mrtGraph.edgeDistance(currentNodeIndex, currentNodeNeighbour[i]);

                if (distanceTable[currentNodeNeighbour[i], 0] > sumDistance) //if distance is greater than the sum of all the distances
                {
                    distanceTable[currentNodeNeighbour[i], 0] = sumDistance; //set new value of sumDistance
                    distanceTable[currentNodeNeighbour[i], 1] = currentNodeIndex; //set new value of currentNodeIndex
                }

            }

            visitedIndex.Add(currentNodeIndex); //add currentNodeIndex to the visitedIndex list

            int nextNode = -1;
            double shortestDist = double.MaxValue;
            for (int i = 0; i < size; i++) //for loop
            {
                if (visitedIndex.Contains(i)) //if visitedIndex list contains int i
                {
                    continue;
                }
                else //else
                {
                    if (shortestDist > distanceTable[i, 0]) //if shortestDist is greater than value in distanceTable
                    {
                        shortestDist = distanceTable[i, 0];
                        nextNode = i; //set value of nextNode to the value of i
                    }
                }
            }
            return nextNode;
        }
    }
}
