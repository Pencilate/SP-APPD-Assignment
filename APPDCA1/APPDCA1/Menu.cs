using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPDCA1
{
    public class Menu
    {
        public static void Main(string[] args)
        {
            Guide.initLineArray();
            //Guide.TestStationRoute();
            //Guide.TestingStationMtd();
            //Guide.Testing();
            Console.WriteLine("INITIALISING GRAPH");
            GraphRoute.initStationIndex();
            GraphRoute.initGraph();
            //GraphRoute.TestGraph();
            GraphRoute.initTraverseDijkstra(Guide.SearchByStationCd("EW21").GraphIndex, Guide.SearchByStationCd("NS2").GraphIndex);



            Console.ReadKey();
        }
    }
}
