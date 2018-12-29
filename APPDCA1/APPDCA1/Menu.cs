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
            //GraphRoute.TestGraph   
            bool repeat = true;
            while (repeat)
            {
                GraphRoute.TestGraphRoute();
                Console.Write("New Route? (Y/N)");
                char response = char.Parse(Console.ReadLine().ToUpper());
                switch (response)
                {
                    case 'Y':
                        repeat = true;
                        break;
                    case 'N':
                        repeat = false;
                        break;
                }
            }


            Console.ReadKey();
        }
    }
}
