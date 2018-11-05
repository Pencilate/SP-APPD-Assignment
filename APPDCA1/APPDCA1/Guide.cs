using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPDCA1
{
    class Guide
    {
        static List<Line> MRT = new List<Line>();

        public static void Main(string[] args)
        {
            Guide.initLineArray();

            //for testing
            for (int i = 0; i < MRT[4].getStationList().Count; i++)
            {
                Console.WriteLine(MRT[4].getStationList()[i].getStationName());
                foreach (string str in MRT[4].getStationList()[i].getStationCode())
                {
                    Console.Write("{0,5},", str);
                }
                Console.WriteLine();
            }
            Console.WriteLine(MRT[0].getStationList()[0].getStationName());
            foreach (string str in MRT[0].getStationList()[0].getStationCode())
            {
                Console.Write("{0,5},", str);
            }
            Console.WriteLine();

            Console.ReadKey();
        }


        public static void initLineArray()
        {
            string FilePath = "..\\..\\resources\\MRT.txt";
            MRT = FileIO.textFileReader(FilePath);

        }
    }
}
